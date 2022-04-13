using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WebSocketSharp;

namespace KruispuntSimulatieController
{
    internal class Program
    {
        private static WebSocket webSocket = new WebSocket("ws://keyslam.com:8080 ");
        private static string sessionName = "elvedin";

        private static void Main(string[] args)
        {
            Routes routes = new Routes();
            List<List<int>> allroutes = routes.allRoute;
            List<List<int>> possibleCarRouteCombos = routes.possibleRoutes;
            List<List<int>> routesLists = routes.baseRouteLists;
            List<EventTypeRouteIdState> routeStatuses = new List<EventTypeRouteIdState>();

            using (webSocket)
            {
                //Maak connectie met broker
                webSocket.Connect();
                ConnectController connectController = new ConnectController("CONNECT_CONTROLLER", sessionName, 1, false, false, false, false);
                string strConnection = JsonConvert.SerializeObject(connectController);
                webSocket.Send(strConnection);

                webSocket.OnMessage += (sender, e) =>
                {
                    if (e.Data.Contains("\"SESSION_START\""))
                    {
                        Console.WriteLine("SESSION_START");
                        routes.FillRouteStateList(routeStatuses, allroutes, webSocket);
                    }

                    if (e.Data.Contains("\"SESSION_STOP\""))
                    {
                        Console.WriteLine("SESSION_STOP");
                        webSocket.Close();
                    }

                    if (e.Data.Contains("\"ENTITY_ENTERED_ZONE\"")) //EventTypeRouteIdSensorId
                    {
                        EventTypeRouteIdSensorId eventTypeRouteIdSensorId = JsonConvert.DeserializeObject<EventTypeRouteIdSensorId>(e.Data);



                    }

                    if (e.Data.Contains("\"ENTITY_EXITED_ZONE\""))  //EventTypeRouteIdSensorId
                    {
                        EventTypeRouteIdSensorId eventTypeRouteIdSensorId = JsonConvert.DeserializeObject<EventTypeRouteIdSensorId>(e.Data);

                    }

                    if (e.Data.Contains("\"ACKNOWLEDGE_BRIDGE_STATE\""))    //EventTypeState
                    {
                        EventTypeRouteIdState eventTypeRouteIdState = JsonConvert.DeserializeObject<EventTypeRouteIdState>(e.Data);
                    }

                    if (e.Data.Contains("\"ACKNOWLEDGE_BARRIERS_STATE\""))  //EventTypeState
                    {
                        EventTypeRouteIdState eventTypeRouteIdState = JsonConvert.DeserializeObject<EventTypeRouteIdState>(e.Data);
                    }

                    if (e.Data.Contains("\"ACKNOWLEDGE_BRIDGE_ROAD_EMPTY\""))   //EventType
                    {
                        EvenType evenType = JsonConvert.DeserializeObject<EvenType>(e.Data);
                    }

                    if (e.Data.Contains("\"ACKNOWLEDGE_BRIDGE_WATER_EMPTY\""))  //EventType
                    {
                        EvenType evenType = JsonConvert.DeserializeObject<EvenType>(e.Data);
                    }
                };
                Console.ReadKey();
            }
        }
    }
}