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
        //private bool _bridgeCycleActive = false;

        public TrafficSimulatorController() { }
        
        public async void Run()
        {
            WebSocket webSocket = new WebSocket(_url);
            TrafficLightState trafficLightState = TrafficLightState.GetInstance();
            TimerModel timerModel = new TimerModel();

            webSocket.OnOpen += (sender, e) =>
            {
                WebSocketConnect(webSocket);     
            };

            webSocket.OnMessage += _onMessage;

            webSocket.OnClose += (sender, e) =>
            {
                Console.WriteLine($"closed; Reason was: {e.Reason}");
            };

            webSocket.OnError += (sander, e) =>
            {
                Console.WriteLine("Server error");
                Console.WriteLine(e.Message);
            };

            webSocket.Connect();

            while (webSocket.IsAlive)
            {
                if (_sessionActive == true)
                {
                    Console.WriteLine($"Current intersection cycle: {timerModel.timerClass}");
                    string currentTimerType = "";

                    switch (timerModel.timerClass)
                    {
                        case TimerModel.TimerClass.Starter:
                            Thread.Sleep(2500);

                            trafficLightState.ChangeStateGreen(GetTheBestCombination());
                            timerModel.SetClass(TimerModel.TimerClass.GreenCycle);

                            currentTimerType = "GREEN";
                            break;

                        case TimerModel.TimerClass.GreenCycle:
                            trafficLightState.ChangeStateOrange();
                            timerModel.SetClass(TimerModel.TimerClass.OrangeCycle);

                            currentTimerType = "ORANGE";
                            break;

                        case TimerModel.TimerClass.OrangeCycle:
                            trafficLightState.ChangeStateRed();
                            timerModel.SetClass(TimerModel.TimerClass.RedCycle);

                            currentTimerType = "RED";
                            break;

                        case TimerModel.TimerClass.RedCycle:
                            trafficLightState.ChangeStateGreen(GetTheBestCombination());
                            timerModel.SetClass(TimerModel.TimerClass.GreenCycle);

                            currentTimerType = "GREEN";
                            break;
                    }
                    await trafficLightState.SendChangedStates(GetTheBestCombination(), webSocket, currentTimerType);
                    Console.WriteLine($"waiting time: {timerModel.time}");
                    Thread.Sleep(timerModel.time);
                }               

                #region SOMETHING
                /*while (_bridgeCycleActive)
                {
                    EventTypeStateModelData eventTypeStateModelData = new EventTypeStateModelData()
                    {
                        state = "ON"
                    };
                    EventTypeStateModel eventTypeStateModel = new EventTypeStateModel()
                    {

                    };
                }*/
                #endregion
            }
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
        private void WebSocketConnect(WebSocket webSocket)
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
            webSocket.Send(JsonSerializer.Serialize(connectController));
        }

        private List<int> GetTheBestCombination()
        {
            TrafficLightEntityAmount amount = TrafficLightEntityAmount.GetInstance();
            RoutesCombinations combinations = RoutesCombinations.GetInstance();

            List<int> newCycle = new List<int>();
            List<int> orderedAmount = amount.GetPriorityRoutes(_lastCycle);

            if (orderedAmount.Count > 0)
            {
                foreach (int key in orderedAmount)
                {
                    bool notFound = true;

                    foreach (int route in newCycle)
                    {
                        if (combinations.GetRouteSpecefiek(route).Contains(key))
                        {
                            notFound = false;
                            break;
                        }
                    }

                    if (notFound)
                    {
                        /*if (key == 41 || key == 42)
                        {
                            _bridgeCycleActive = true;
                        }*/

                        newCycle.Add(key);
                    }
                }

                GetCycle(newCycle);
                _lastCycle = newCycle;
            }
            return newCycle;
        }

        private void GetCycle(List<int> cycle)
        {
            Console.WriteLine("Current active routes:");
            foreach (int route in cycle)
            {
                Console.WriteLine($"{route} ");
            }
        }
    }
}
