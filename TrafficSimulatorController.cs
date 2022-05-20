using KruispuntSimulatieController.ProtocolModels;
using KruispuntSimulatieController.RouteDataModels;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using KruispuntSimulatieController.ConnectionModels;
using KruispuntSimulatieController.RouteDataModels.AllRouteModels;
using WebSocketSharp;

namespace KruispuntSimulatieController
{
    public class TrafficSimulatorController
    {
        private readonly string _sessionName = "alsjeblieft";
        private readonly string _url = "ws://keyslam.com:8080";
        private readonly WebSocket _webSocket;
        private bool _sessionActive;
        
        private Thread _mainThread;
        private Thread _boatThread;

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
                case { } a when a.Contains("\"eventType\"" + ":" + "\"ERROR_UNKNOWN_EVENT_TYPE\""):
                    ErrorUnknownEventTypeModel aData = JsonSerializer.Deserialize<ErrorUnknownEventTypeModel>(e.Data);
                    if (aData != null)
                    {
                        Console.WriteLine($"Error received:     {aData.eventType}");
                        Console.WriteLine($"Invalid message:    {aData.data.receivedMessage}");
                        Console.WriteLine($"Invalid message:    {aData.data.validEventTypes}");
                    }

                    break;

                case { } b when b.Contains("\"eventType\"" + ":" + "\"ERROR_NOT_PARSEABLE\""):
                    ErrorNotParseableModel bData = JsonSerializer.Deserialize<ErrorNotParseableModel>(e.Data);
                    if (bData != null)
                    {
                        Console.WriteLine($"Error received:     {bData.eventType}");
                        Console.WriteLine($"Invalid message:    {bData.data.receivedMessage}");
                        Console.WriteLine($"Given exception:    {bData.data.exception}");
                    }

                    break;

                case { } c when c.Contains("\"eventType\"" + ":" + "\"ERROR_MALFORMED_MESSAGE\""):
                    ErrorMalformedMessageModel cData = JsonSerializer.Deserialize<ErrorMalformedMessageModel>(e.Data);
                    if (cData != null)
                    {
                        Console.WriteLine($"Error received:     {cData.eventType}");
                        Console.WriteLine($"Invalid message:    {cData.data.receivedMessage}");
                        Console.WriteLine("Given Errors:");
                        foreach (string error in cData.data.errors)
                        {
                            Console.WriteLine($"~Error: {error}");
                        }
                    }

                    break;

                case { } d when d.Contains("\"eventType\"" + ":" + "\"ERROR_INVALID_STATE\""):
                    ErrorInvalidStateModel dData = JsonSerializer.Deserialize<ErrorInvalidStateModel>(e.Data);
                    if (dData != null)
                    {
                        Console.WriteLine($"Error received:     {dData.eventType}");
                        Console.WriteLine($"Invalid message:    {dData.data.receivedMessage}");
                        Console.WriteLine($"Given error:        {dData.data.error}");
                    }

                    break;

                case { } f when f.Contains("\"eventType\"" + ":" + "\"SESSION_START\""):
                    Console.Clear();
                    EventTypeModel fData = JsonSerializer.Deserialize<EventTypeModel>(e.Data);
                    if (fData != null)
                    {
                        Console.WriteLine($"{fData.eventType}");
                    }
                    _sessionActive = true;
                    _mainThread = new Thread(MainLoop);
                    _boatThread = new Thread(BoatLoop);
                    _mainThread.Start();
                    _boatThread.Start();
                    break;

                case { } g when g.Contains("\"eventType\"" + " : " + "\"SESSION_STOP\""):
                    EventTypeModel gData = JsonSerializer.Deserialize<EventTypeModel>(e.Data);
                    if (gData != null)
                    {
                        Console.WriteLine($"{gData.eventType}");
                    }
                    _sessionActive = false;
                    break;

                case { } h when h.Contains("\"eventType\"" + ":" + "\"ENTITY_ENTERED_ZONE\""):
                    EventTypeRouteIdSensorIdModel hData = JsonSerializer.Deserialize<EventTypeRouteIdSensorIdModel>(e.Data);
                    if (hData != null)
                    {
                        Console.WriteLine($"{hData.eventType}");
                        List<int> boatRoutes = new BoatsRoutes().boatsRoutesList;
                        if (boatRoutes.Contains(hData.data.routeId))
                        {
                            BridgeLightState.GetInstance().SetRouteId(hData.data.routeId);
                            BridgeLightState.GetInstance().ChangeBoatRouteState(hData.eventType);
                        }
                        else
                        {
                            TrafficLightEntityAmount.GetInstance().ChangeTrafficLightAmount(hData.data.routeId);
                        }
                        
                    }
                    break;
                
                case { } i when i.Contains("\"eventType\"" + ":" + "\"ACKNOWLEDGE_BRIDGE_ROAD_EMPTY\""):
                    EventTypeModel iData = JsonSerializer.Deserialize<EventTypeModel>(e.Data);
                    if (iData != null)
                    {
                        Console.WriteLine($"{iData.eventType}");
                        BridgeLightState.GetInstance().ChangeBoatRouteEventType(iData.eventType);
                    }
                    break;
                
                case { } j when j.Contains("\"eventType\"" + ":" + "\"ACKNOWLEDGE_BARRIERS_STATE\""):
                    EventTypeStateModel jData = JsonSerializer.Deserialize<EventTypeStateModel>(e.Data);
                    if (jData != null)
                    {
                        Console.WriteLine($"{jData.eventType}");
                        BridgeLightState.GetInstance().ChangeBoatRouteEventType(jData.eventType);
                        BridgeLightState.GetInstance().ChangeBoatRouteState(jData.data.state);
                    }
                    break;
                
                case { } k when k.Contains("\"eventType\"" + ":" + "\"ACKNOWLEDGE_BRIDGE_STATE\""):
                    EventTypeStateModel kData = JsonSerializer.Deserialize<EventTypeStateModel>(e.Data);
                    if (kData != null)
                    {
                        Console.WriteLine($"{kData.eventType}");
                        BridgeLightState.GetInstance().ChangeBoatRouteEventType(kData.eventType);
                        BridgeLightState.GetInstance().ChangeBoatRouteState(kData.data.state);
                    }
                    break;
                
                case { } l when l.Contains("\"eventType\"" + ":" + "\"ACKNOWLEDGE_BRIDGE_WATER_EMPTY\""):
                    EventTypeStateModel lData = JsonSerializer.Deserialize<EventTypeStateModel>(e.Data);
                    if (lData != null)
                    {
                        Console.WriteLine($"{lData.eventType}");
                        BridgeLightState.GetInstance().ChangeBoatRouteEventType(lData.eventType);
                    }
                    break;
                
                
            }
        }

        private void BoatLoop()
        {
            BridgeInformation bridgeInformation = new BridgeInformation();
            List<int> boatsRoutes = new BoatsRoutes().boatsRoutesList;
            while (_sessionActive)
            {
                if (!boatsRoutes.Contains(bridgeInformation.routeId))
                {
                    Thread.Sleep(10);
                }
                else
                {
                    Console.WriteLine("Boat arrived. Bridge sequence starting");
                    BridgeLightState.GetInstance().BridgeSequence(_webSocket);
                    if (bridgeInformation.routeId == 0)
                    {
                        Console.WriteLine("Boat passed. Bridge sequence finished");
                    }
                }
            }
        }

        private void MainLoop()
        {
            while (_sessionActive)
            {
                List<int> trafficLight = TrafficLightEntityAmount.GetInstance().GetPriorityRoutes();
                if (trafficLight.Count == 0)
                {
                    Thread.Sleep(10);
                }
                else
                {
                    Console.WriteLine("Starting new traffic light cycle");
                    TrafficLightState.GetInstance().SendChangedStates(trafficLight, _webSocket, "GREEN");
                    Thread.Sleep(8000);
                    TrafficLightState.GetInstance().SendChangedStates(trafficLight, _webSocket, "ORANGE");
                    Thread.Sleep(3000);
                    TrafficLightState.GetInstance().SendChangedStates(trafficLight, _webSocket, "RED");
                    TrafficLightEntityAmount.GetInstance().ResetFromList(trafficLight);
                }
            }
        }

        private void _webSocketConnect()
        {
            ConnectionDataModel connectionDataModel = new ConnectionDataModel
            {
                sessionName = _sessionName,
                sessionVersion = 1,
                discardParseErrors = false,
                discardEventTypeErrors = false,
                discardMalformedDataErrors = false,
                discardInvalidStateErrors = false
            };

            ConnectController connectController = new ConnectController { data = connectionDataModel };
            _webSocket.Send(JsonSerializer.Serialize(connectController));
        }
    }
}
