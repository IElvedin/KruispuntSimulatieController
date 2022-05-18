using KruispuntSimulatieController.Connection.Handler;
using KruispuntSimulatieController.ProtocolModels;
using KruispuntSimulatieController.Route.Data;
using KruispuntSimulatieController.RouteDataModels;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using WebSocketSharp;

namespace KruispuntSimulatieController
{
    public class TrafficSimulatorController
    {
        private readonly string _sessionName = "elvedin";
        private readonly string _url = "ws://keyslam.com:8080";
        private List<int> _lastCycle;
        private bool _sessionActive = false;
        private WebSocket _webSocket;
        private Thread _mainThread;

        public TrafficSimulatorController() 
        {
            _webSocket = new WebSocket(_url);
        }
        
        public void Run()
        {
            _webSocket.OnOpen += (sender, e) =>
            {
                _webSocketConnect();     
            };

            _webSocket.OnMessage += _onMessage;

            _webSocket.OnClose += (sender, e) =>
            {
                Console.WriteLine($"closed; Reason was: {e.Reason}");
            };

            _webSocket.OnError += (sander, e) =>
            {
                Console.WriteLine("Server error");
                Console.WriteLine(e.Message);
            };

            _webSocket.Connect();
        }

        private void _onMessage(object sender, MessageEventArgs e)
        {
            switch (e.Data)
            {
                case string a when a.Contains("\"eventType\"" + ":" + "\"ERROR_UNKNOWN_EVENT_TYPE\""):
                    ErrorUnknownEventTypeModel aData = JsonSerializer.Deserialize<ErrorUnknownEventTypeModel>(e.Data);
                    Console.WriteLine($"Error received:     {aData.eventType}");
                    Console.WriteLine($"Invalid message:    {aData.data.receivedMessage}");
                    break;

                case string b when b.Contains("\"eventType\"" + ":" + "\"ERROR_NOT_PARSEABLE\""):
                    ErrorNotParseableModel bData = JsonSerializer.Deserialize<ErrorNotParseableModel>(e.Data);
                    Console.WriteLine($"Error received:     {bData.eventType}");
                    Console.WriteLine($"Invalid message:    {bData.data.receivedMessage}");
                    Console.WriteLine($"Given exception:    {bData.data.exception}");
                    break;

                case string c when c.Contains("\"eventType\"" + ":" + "\"ERROR_MALFORMED_MESSAGE\""):
                    ErrorMalformedMessageModel cData = JsonSerializer.Deserialize<ErrorMalformedMessageModel>(e.Data);
                    Console.WriteLine($"Error received:     {cData.eventType}");
                    Console.WriteLine($"Invalid message:    {cData.data.receivedMessage}");
                    Console.WriteLine("Given Errors:");
                    for (int i = 0; i < cData.data.errors.Length; i++)
                    {
                        Console.WriteLine($"~Error: {cData.data.errors[i]}");
                    }
                    break;

                case string d when d.Contains("\"eventType\"" + ":" + "\"ERROR_INVALID_STATE\""):
                    ErrorInvalidStateModel dData = JsonSerializer.Deserialize<ErrorInvalidStateModel>(e.Data);
                    Console.WriteLine($"Error received:     {dData.eventType}");
                    Console.WriteLine($"Invalid message:    {dData.data.receivedMessage}");
                    Console.WriteLine($"Given error:        {dData.data.error}");
                    break;

                case string f when f.Contains("\"eventType\"" + ":" + "\"SESSION_START\""):
                    Console.Clear();
                    EventTypeModel fData = JsonSerializer.Deserialize<EventTypeModel>(e.Data);
                    Console.WriteLine($"{fData.eventType}");
                    _sessionActive = true;
                    _mainThread = new Thread(MainLoop);
                    _mainThread.Start();
                    break;

                case string g when g.Contains("\"eventType\"" + " : " + "\"SESSION_STOP\""):
                    EventTypeModel gData = JsonSerializer.Deserialize<EventTypeModel>(e.Data);
                    Console.WriteLine($"{gData.eventType}");
                    _sessionActive = false;
                    break;

                case string h when h.Contains("\"eventType\"" + ":" + "\"ENTITY_ENTERED_ZONE\""):
                    EventTypeRouteIdSensorIdModel hData = JsonSerializer.Deserialize<EventTypeRouteIdSensorIdModel>(e.Data);
                    Console.WriteLine($"{hData.eventType}");
                    TrafficLightEntityAmount.GetInstance().ChangeTrafficLightAmount(hData.data.routeId);
                    break;
            }
        }

        private void MainLoop()
        {
            while (_sessionActive)
            {
                List<int> trafficlights = TrafficLightEntityAmount.GetInstance().GetPriorityRoutes();
                if (trafficlights.Count == 0)
                {
                    Thread.Sleep(10);
                }
                else
                {
                    Console.WriteLine("Starting trafficlight cycle");
                    TrafficLightState.GetInstance().SendChangedStates(trafficlights, _webSocket, "GREEN");
                    Thread.Sleep(8000);
                    TrafficLightState.GetInstance().SendChangedStates(trafficlights, _webSocket, "ORANGE");
                    Thread.Sleep(3000);
                    TrafficLightState.GetInstance().SendChangedStates(trafficlights, _webSocket, "RED");
                    TrafficLightEntityAmount.GetInstance().ResetFromList(trafficlights);
                }
            }
        }

        private void _webSocketConnect()
        {
            ConnectionDataModel ConnectionDataModel = new ConnectionDataModel
            {
                sessionName = _sessionName,
                sessionVersion = 1,
                discardParseErrors = false,
                discardEventTypeErrors = false,
                discardMalformedDataErrors = false,
                discardInvalidStateErrors = false
            };

            ConnectController connectController = new ConnectController { data = ConnectionDataModel };
            _webSocket.Send(JsonSerializer.Serialize(connectController));
        }
    }
}
