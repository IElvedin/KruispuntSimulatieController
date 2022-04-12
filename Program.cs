using KruispuntSimulatieController.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WebSocketSharp;

namespace KruispuntSimulatieController
{
    internal class Program
    {
        private static readonly string address = "ws://keyslam.com:8080 ";
        private static readonly int onTrafficlightWait = 5000;
        private static int[] allRoutes = { 1, 2, 3, 4, 5, 7, 8, 9, 10, 11, 12, 15, 21, 22, 23, 24, 31, 32, 33, 34, 35, 36, 37, 38 };

        private static void Main(string[] args)
        {
            ConnectControllerModel connectModel = new ConnectControllerModel();
            List<SetRouteState> routeStatuses = new List<SetRouteState>();

            List<List<int>> possibleCarRouteCombos = new List<List<int>>();
            possibleCarRouteCombos.Add(new List<int> { 1, 2, 7, 8 });
            possibleCarRouteCombos.Add(new List<int> { 1, 2, 3, 4 });
            possibleCarRouteCombos.Add(new List<int> { 1, 4, 10, 11 });
            possibleCarRouteCombos.Add(new List<int> { 1, 4, 7, 10 });
            possibleCarRouteCombos.Add(new List<int> { 1, 7, 10, 12 });
            possibleCarRouteCombos.Add(new List<int> { 3, 4, 9, 10 });
            possibleCarRouteCombos.Add(new List<int> { 4, 5, 7 });
            possibleCarRouteCombos.Add(new List<int> { 4, 7, 9 });
            possibleCarRouteCombos.Add(new List<int> { 7, 8, 9, 10 });

            List<List<int>> routesLists = new List<List<int>>();
            routesLists.Add(new List<int> { }); //0
            routesLists.Add(new List<int> { }); //1
            routesLists.Add(new List<int> { }); //2
            routesLists.Add(new List<int> { }); //3
            routesLists.Add(new List<int> { }); //4
            routesLists.Add(new List<int> { }); //5
            routesLists.Add(new List<int> { }); //6
            routesLists.Add(new List<int> { }); //7
            routesLists.Add(new List<int> { }); //8

            using (WebSocket websocket = new WebSocket(address))
            {
                //Maak connectie met broker
                websocket.Connect();
                ConnectController connectController = new ConnectController("CONNECT_CONTROLLER", "groep8", 1, false, false, false, false);
                string strConnection = JsonConvert.SerializeObject(connectController);
                websocket.Send(strConnection);

                websocket.OnMessage += (sender, e) =>
                {
                    if (e.Data.Contains("\"ENTITY_ENTERED_ZONE\"") || e.Data.Contains("\"ENTITY_EXITED_ZONE\"")) ;
                    {
                        EntityEnteredOrExitedZone entityEnteredOrExitedZone = JsonConvert.DeserializeObject<EntityEnteredOrExitedZone>(e.Data);
                        foreach (List<int> route in possibleCarRouteCombos)
                        {
                            Console.WriteLine("new routeId: " + entityEnteredOrExitedZone.data.routeId);
                            if (route.Contains(entityEnteredOrExitedZone.data.routeId))
                            {
                                routesLists[possibleCarRouteCombos.IndexOf(route)].Add(entityEnteredOrExitedZone.data.routeId);
                            }
                            Console.WriteLine("amount of cars: " + routesLists[possibleCarRouteCombos.IndexOf(route)].Count);
                        }
                    }

                    if (e.Data.Contains("\"ACKNOWLEDGE_BRIDGE_STATE\"") || e.Data.Contains("\"ACKNOWLEDGE_BARRIERS_STATE\""))
                    {
                        SetOrRequestOrAcknowledgeBridgeState setOrRequestOrAcknowledge = JsonConvert.DeserializeObject<SetOrRequestOrAcknowledgeBridgeState>(e.Data);
                    }

                    if (e.Data.Contains("\"ACKNOWLEDGE_BRIDGE_STATE\"") || e.Data.Contains("\"ACKNOWLEDGE_BARRIERS_STATE\""))
                    {
                        SetOrRequestOrAcknowledgeBridgeState setOrRequestOrAcknowledge = JsonConvert.DeserializeObject<SetOrRequestOrAcknowledgeBridgeState>(e.Data);
                    }
                };

                Console.ReadKey();
            }
        }

        /*private static void fillRoutesWithStateModels(List<SetRouteState> routeStatuses, int[] allRoutes, WebSocket webSocket)
        {
            for (int i = 0; i < allRoutes.Length; i++)
            {
                SetRouteState routeStatus = new SetRouteState("SET_AUTOMOBILE_ROUTE_STATE", allRoutes[i], "RED");
                string strSetAutomobileRouteState = JsonConvert.SerializeObject(routeStatus);
                webSocket.Send(strSetAutomobileRouteState);
                routeStatuses.Add(routeStatus);
            }
        }*/

        public static void Connect(WebSocket webSocket, ConnectControllerModel connectModel)
        {
            webSocket.Connect();
            ConnectController connectController = new ConnectController(connectModel.eventType, connectModel.sessionName, connectModel.sessionVersion, connectModel.discardParseErrors, connectModel.discardEventTypeErrors, connectModel.discardMalformedDataErrors, connectModel.discardInvalidStateErrors);
            webSocket.Send(JsonConvert.SerializeObject(connectController));
        }
    }
}