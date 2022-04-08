using Newtonsoft.Json;
using System;
using WebSocketSharp;

namespace KruispuntSimulatieController
{
    internal class Program
    {
        private static readonly string address = "ws://keyslam.com:8080 ";
        private static readonly int onTrafficlightWait = 5000;

        private static void Main(string[] args)
        {
            SetRouteState setRouteState = new SetRouteState(address, onTrafficlightWait, address);
            //Maakt connectie met broker
            using (WebSocket websocket = new WebSocket(address))
            {
                //Maak connectie met broker
                websocket.Connect();

                ConnectController connectController = new ConnectController("CONNECT_CONTROLLER", "elvedin", 1, false, false, false, false);
                string strConnection = JsonConvert.SerializeObject(connectController);
                websocket.Send(strConnection);

                //Check of er pogingen worden gedaan voor connectie maken
                if (websocket.IsAlive)
                {
                    Console.WriteLine("Tried making connection \n");
                }

                int[] routeSize = { 1, 2, 3, 4, 5, 7, 8, 9, 10, 11, 12, 15, 21, 22, 23, 24, 31, 32, 33, 34, 35, 36, 37, 38 };

                int[][] routePossibilities = new int[][]
                {
                    new int[] { 1, 2, 7, 8},
                    new int[] { 1, 2, 3 ,4},
                    new int[] { 1, 4, 10, 11 },
                    new int[] { 1, 4, 7, 10 },
                    new int[] { 1, 7, 10, 12 },
                    new int[] { 1, 10, 11, 12 },
                    new int[] { 3, 4, 9, 10 },
                    new int[] { 4,5,7 },
                    new int[] { 4, 7, 9 },
                    new int[] { 7, 8, 9, 10 }
                };

                //Bij inkomende berichten voer deze taken uit
                websocket.OnMessage += (sender, e) =>
                {
                    EntityEnteredZone entityEnteredZone = JsonConvert.DeserializeObject<EntityEnteredZone>(e.Data);
                    foreach (int[] array in routePossibilities)
                    {
                        int[] currentArray = array;

                        foreach (int possibleRoute in array)
                        {
                            if (entityEnteredZone.eventType == "ENTITY_ENTERED_ZONE")
                            {
                                SetRouteState setRouteState = new SetRouteState(entityEnteredZone.eventType, entityEnteredZone.data.routeId, "GREEN");
                                string strSetAutomobileRouteState = JsonConvert.SerializeObject(setRouteState);
                                websocket.Send(strSetAutomobileRouteState);
                                Console.WriteLine(strSetAutomobileRouteState);
                            }
                        }

                        foreach (int possibleRoute in array)
                        {
                            if (entityEnteredZone.eventType == "ENTITY_EXITED_ZONE")
                            {
                                SetRouteState setRouteState = new SetRouteState(entityEnteredZone.eventType, entityEnteredZone.data.routeId, "RED");

                                string strSetAutomobileRouteState = JsonConvert.SerializeObject(setRouteState);
                                websocket.Send(strSetAutomobileRouteState);
                                Console.WriteLine(strSetAutomobileRouteState);
                            }
                        }
                    }
                };
                Console.ReadKey();
            }
        }
    }
}