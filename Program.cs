using KruispuntSimulatieController.ConnectController;
using KruispuntSimulatieController.EntityEnteredZone;
using KruispuntSimulatieController.SetAutombileRouteState;
using KruispuntSimulatieController.SetCyclistRouteState;
using KruispuntSimulatieController.SetPedestrianRouteState;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WebSocketSharp;

namespace KruispuntSimulatieController
{
    class Program
    {
        private static readonly string address = "ws://keyslam.com:8080 ";

        static void Main(string[] args)
        {
            int sleeperTime = 10000;
            //Maakt connectie met broker
            using (WebSocket websocket = new WebSocket(address))
            {
                //Maak connectie met broker
                websocket.Connect();

                //Connect Controller
                ControllerConnectJSONModel jSONMessageDataModel = new ControllerConnectJSONModel()
                {
                    eventType = "CONNECT_CONTROLLER",
                    data = new ControllerConnectJSONModelData()
                    {
                        sessionName = "KFC",
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

                int[] routeSize = {1, 2, 3, 4, 5, 7, 8, 9, 10, 11, 12, 15, 21, 22, 23, 24, 31, 32, 33, 34, 35, 36, 37, 38};

                foreach (int route in routeSize)
                {
                    if (route == 1 || route == 2 || route == 3 || route == 4 || route == 5 || route == 7 || route == 8 || route == 9 || route == 10 || route == 11 || route == 12 || route == 15)
                    {
                        //Set Automobile Route State
                        SetAutomobileRouteStateJSONModel SetAutombileRouteStateJSONModel = new SetAutomobileRouteStateJSONModel()
                        {
                            eventType = "SET_AUTOMOBILE_ROUTE_STATE",
                            data = new SetAutomobileRouteStateJSONModelData()
                            {
                                routeId = route,
                                state = "GREEN"
                            }
                        };
                        string strSetAutomobileRouteState = JsonConvert.SerializeObject(SetAutombileRouteStateJSONModel);
                        websocket.Send(strSetAutomobileRouteState);
                        Console.WriteLine(strSetAutomobileRouteState);
                    }
                    if (route == 21 || route == 22 || route == 23 || route == 24)
                    {
                        SetCyclistRouteStateJSONModel setCyclistRouteStateJSONModel = new SetCyclistRouteStateJSONModel()
                        {
                            eventType = "SET_CYCLIST_ROUTE_STATE",
                            data = new SetCyclistRouteStateJSONModelJSONModelData()
                            {
                                routeId = route,
                                state = "GREEN"
                            }
                        };
                        string strSetCyclistRouteStateJSONModel = JsonConvert.SerializeObject(setCyclistRouteStateJSONModel);
                        websocket.Send(strSetCyclistRouteStateJSONModel);
                        Console.WriteLine(strSetCyclistRouteStateJSONModel);
                    }
                    if (route == 31 || route == 32 || route == 33 || route == 34 || route == 35 || route == 36 || route == 37 || route == 38)
                    {
                        SetPedestrianRouteStateJSONModel setPedestrianRouteStateJSONModel = new SetPedestrianRouteStateJSONModel()
                        {
                            eventType = "SET_PEDESTRIAN_ROUTE_STATE",
                            data = new SetPedestrianRouteStateJSONModelData()
                            {
                                routeId = route,
                                state = "GREEN"
                            }
                        };
                        string strSetPedestrianRouteStateJSONModel = JsonConvert.SerializeObject(setPedestrianRouteStateJSONModel);
                        websocket.Send(strSetPedestrianRouteStateJSONModel);
                        Console.WriteLine(strSetPedestrianRouteStateJSONModel);
                    }
                }

                System.Threading.Thread.Sleep(sleeperTime);

                foreach (int route in routeSize)
                {
                    if (route == 1 || route == 2 || route == 3 || route == 4 || route == 5 || route == 7 || route == 8 || route == 9 || route == 10 || route == 11 || route == 12 || route == 15)
                    {
                        //Set Automobile Route State
                        SetAutomobileRouteStateJSONModel SetAutombileRouteStateJSONModel = new SetAutomobileRouteStateJSONModel()
                        {
                            eventType = "SET_AUTOMOBILE_ROUTE_STATE",
                            data = new SetAutomobileRouteStateJSONModelData()
                            {
                                routeId = route,
                                state = "ORANGE"
                            }
                        };
                        string strSetAutomobileRouteState = JsonConvert.SerializeObject(SetAutombileRouteStateJSONModel);
                        websocket.Send(strSetAutomobileRouteState);
                        Console.WriteLine(strSetAutomobileRouteState);
                    }
                    if (route == 21 || route == 22 || route == 23 || route == 24)
                    {
                        SetCyclistRouteStateJSONModel setCyclistRouteStateJSONModel = new SetCyclistRouteStateJSONModel()
                        {
                            eventType = "SET_CYCLIST_ROUTE_STATE",
                            data = new SetCyclistRouteStateJSONModelJSONModelData()
                            {
                                routeId = route,
                                state = "ORANGE"
                            }
                        };
                        string strSetCyclistRouteStateJSONModel = JsonConvert.SerializeObject(setCyclistRouteStateJSONModel);
                        websocket.Send(strSetCyclistRouteStateJSONModel);
                        Console.WriteLine(strSetCyclistRouteStateJSONModel);
                    }
                    if (route == 31 || route == 32 || route == 33 || route == 34 || route == 35 || route == 36 || route == 37 || route == 38)
                    {
                        SetPedestrianRouteStateJSONModel setPedestrianRouteStateJSONModel = new SetPedestrianRouteStateJSONModel()
                        {
                            eventType = "SET_PEDESTRIAN_ROUTE_STATE",
                            data = new SetPedestrianRouteStateJSONModelData()
                            {
                                routeId = route,
                                state = "BLINKING"
                            }
                        };
                        string strSetPedestrianRouteStateJSONModel = JsonConvert.SerializeObject(setPedestrianRouteStateJSONModel);
                        websocket.Send(strSetPedestrianRouteStateJSONModel);
                        Console.WriteLine(strSetPedestrianRouteStateJSONModel);
                    }
                }

                System.Threading.Thread.Sleep(sleeperTime);

                foreach (int route in routeSize)
                {
                    if (route == 1 || route == 2 || route == 3 || route == 4 || route == 5 || route == 7 || route == 8 || route == 9 || route == 10 || route == 11 || route == 12 || route == 15)
                    {
                        //Set Automobile Route State
                        SetAutomobileRouteStateJSONModel SetAutombileRouteStateJSONModel = new SetAutomobileRouteStateJSONModel()
                        {
                            eventType = "SET_AUTOMOBILE_ROUTE_STATE",
                            data = new SetAutomobileRouteStateJSONModelData()
                            {
                                routeId = route,
                                state = "RED"
                            }
                        };
                        string strSetAutomobileRouteState = JsonConvert.SerializeObject(SetAutombileRouteStateJSONModel);
                        websocket.Send(strSetAutomobileRouteState);
                        Console.WriteLine(strSetAutomobileRouteState);
                    }
                    if (route == 21 || route == 22 || route == 23 || route == 24)
                    {
                        SetCyclistRouteStateJSONModel setCyclistRouteStateJSONModel = new SetCyclistRouteStateJSONModel()
                        {
                            eventType = "SET_CYCLIST_ROUTE_STATE",
                            data = new SetCyclistRouteStateJSONModelJSONModelData()
                            {
                                routeId = route,
                                state = "RED"
                            }
                        };
                        string strSetCyclistRouteStateJSONModel = JsonConvert.SerializeObject(setCyclistRouteStateJSONModel);
                        websocket.Send(strSetCyclistRouteStateJSONModel);
                        Console.WriteLine(strSetCyclistRouteStateJSONModel);
                    }
                    if (route == 31 || route == 32 || route == 33 || route == 34 || route == 35 || route == 36 || route == 37 || route == 38)
                    {
                        SetPedestrianRouteStateJSONModel setPedestrianRouteStateJSONModel = new SetPedestrianRouteStateJSONModel()
                        {
                            eventType = "SET_PEDESTRIAN_ROUTE_STATE",
                            data = new SetPedestrianRouteStateJSONModelData()
                            {
                                routeId = route,
                                state = "RED"
                            }
                        };
                        string strSetPedestrianRouteStateJSONModel = JsonConvert.SerializeObject(setPedestrianRouteStateJSONModel);
                        websocket.Send(strSetPedestrianRouteStateJSONModel);
                        Console.WriteLine(strSetPedestrianRouteStateJSONModel);
                    }
                }

                System.Threading.Thread.Sleep(sleeperTime);


                int[] greenRouteSize = { 1, 2, 8, 22, 33, 34 };

                foreach (int route in greenRouteSize)
                {
                    if (route == 1 || route == 2 || route == 3 || route == 4 || route == 5 || route == 7 || route == 8 || route == 9 || route == 10 || route == 11 || route == 12 || route == 15)
                    {
                        //Set Automobile Route State
                        SetAutomobileRouteStateJSONModel SetAutombileRouteStateJSONModel = new SetAutomobileRouteStateJSONModel()
                        {
                            eventType = "SET_AUTOMOBILE_ROUTE_STATE",
                            data = new SetAutomobileRouteStateJSONModelData()
                            {
                                routeId = route,
                                state = "GREEN"
                            }
                        };
                        string strSetAutomobileRouteState = JsonConvert.SerializeObject(SetAutombileRouteStateJSONModel);
                        websocket.Send(strSetAutomobileRouteState);
                        Console.WriteLine(strSetAutomobileRouteState);
                    }
                    if (route == 21 || route == 22 || route == 23 || route == 24)
                    {
                        SetCyclistRouteStateJSONModel setCyclistRouteStateJSONModel = new SetCyclistRouteStateJSONModel()
                        {
                            eventType = "SET_CYCLIST_ROUTE_STATE",
                            data = new SetCyclistRouteStateJSONModelJSONModelData()
                            {
                                routeId = route,
                                state = "GREEN"
                            }
                        };
                        string strSetCyclistRouteStateJSONModel = JsonConvert.SerializeObject(setCyclistRouteStateJSONModel);
                        websocket.Send(strSetCyclistRouteStateJSONModel);
                        Console.WriteLine(strSetCyclistRouteStateJSONModel);
                    }
                    if (route == 31 || route == 32 || route == 33 || route == 34 || route == 35 || route == 36 || route == 37 || route == 38)
                    {
                        SetPedestrianRouteStateJSONModel setPedestrianRouteStateJSONModel = new SetPedestrianRouteStateJSONModel()
                        {
                            eventType = "SET_PEDESTRIAN_ROUTE_STATE",
                            data = new SetPedestrianRouteStateJSONModelData()
                            {
                                routeId = route,
                                state = "GREEN"
                            }
                        };
                        string strSetPedestrianRouteStateJSONModel = JsonConvert.SerializeObject(setPedestrianRouteStateJSONModel);
                        websocket.Send(strSetPedestrianRouteStateJSONModel);
                        Console.WriteLine(strSetPedestrianRouteStateJSONModel);
                    }
                }


                //Bij inkomende berichten voer deze taken uit
                websocket.OnMessage += (sender, e) =>
                {
                    EntityEnteredZoneJSONModel entityEnteredZoneModel = JsonConvert.DeserializeObject<EntityEnteredZoneJSONModel>(e.Data);
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
