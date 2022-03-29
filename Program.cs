using KruispuntSimulatieController.Parsers;
using Newtonsoft.Json;
using System;
using WebSocketSharp;

namespace KruispuntSimulatieController
{
    class Program
    {
        private static readonly string address = "ws://keyslam.com:8080 ";

        static void Main(string[] args)
        {
            //Maakt connectie met broker
            using (WebSocket websocket = new WebSocket(address))
            {
                //Maak connectie met broker
                websocket.Connect();

                //Connect Controller
                JSONControllerConnectData jSONMessageDataModel = new JSONControllerConnectData()
                {
                    eventType = "CONNECT_CONTROLLER",
                    data = new ControllerConnectDataModel()
                    {
                        sessionName = "discordtest12",
                        sessionVersion = 1,
                        discardParseErrors = false,
                        discardEventTypeErrors = false,
                        discardMalformedDataErrors = false,
                        discardInvalidStateErrors = false
                    }
                };
                string strConnection = JsonConvert.SerializeObject(jSONMessageDataModel);
                Console.WriteLine(strConnection);
                websocket.Send(strConnection);

                Console.WriteLine();

                //Check of er pogingen worden gedaan voor connectie maken
                if (websocket.IsAlive)
                {
                    Console.WriteLine("Tried making connection");
                }

                Console.WriteLine();

                //Set Automobile Route State
                JSONSetAutombileRouteState jSONSetAutombileRouteState = new JSONSetAutombileRouteState()
                {
                    eventType = "SET_AUTOMOBILE_ROUTE_STATE",
                    data = new SetAutomobileRouteStateModel()
                    {
                        routeId = 1,
                        state = "GREEN"
                    }
                };
                string strSetAutomobileRouteState = JsonConvert.SerializeObject(jSONSetAutombileRouteState);
                websocket.Send(strSetAutomobileRouteState);
                Console.WriteLine(strSetAutomobileRouteState);

                //Bij inkomende berichten voer deze taken uit
                websocket.OnMessage += (sender, e) =>
                {
                    EntityEnteredZoneModel entityEnteredZoneModel = JsonConvert.DeserializeObject<EntityEnteredZoneModel>(e.Data);
                    Console.WriteLine();
                    Console.WriteLine(entityEnteredZoneModel.eventType);
                    Console.WriteLine(entityEnteredZoneModel.data.routeId);
                    Console.WriteLine(entityEnteredZoneModel.data.sensorId);
                };

                Console.ReadKey();
            }
            
        }
    }
}
