using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using WebSocketSharp;

namespace KruispuntSimulatieController
{
    internal class Program
    {
        private static WebSocket webSocket = new WebSocket("ws://keyslam.com:8080 ");
        private static string sessionName = "nhlstenden";
        System.Timers.Timer timer = new System.Timers.Timer(8000);

        private static void Main(string[] args)
        {
            Routes routes = new Routes();
            List<List<int>> allroutes = routes.allRoute;
            List<EventTypeRouteIdState> routeStatuses = new List<EventTypeRouteIdState>();
            Queue<EventTypeRouteIdSensorId> incomingQueue = new Queue<EventTypeRouteIdSensorId>();
            Queue<EventTypeRouteIdSensorId> outgoingQueue = new Queue<EventTypeRouteIdSensorId>();

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
                                incomingQueue.Clear();
                                outgoingQueue.Clear();
                                routeStatuses.Clear();
                            }

                            if (e.Data.Contains("\"ENTITY_ENTERED_ZONE\"")) //EventTypeRouteIdSensorId
                            {
                                EventTypeRouteIdSensorId eventTypeRouteIdSensorId = JsonConvert.DeserializeObject<EventTypeRouteIdSensorId>(e.Data);
                                Console.WriteLine($"ENTITY_ENTERED_ZONE: {eventTypeRouteIdSensorId.data.routeId}");
                                if (eventTypeRouteIdSensorId.data.sensorId == 1 || eventTypeRouteIdSensorId.data.sensorId == 3)
                                {
                                    incomingQueue.Enqueue(eventTypeRouteIdSensorId);
                                    Console.WriteLine($"Entered queue count: {incomingQueue.Count}");

                                    if (incomingQueue.Count > 0)
                                    {
                                        EventTypeRouteIdState eventTypeRouteIdState = new EventTypeRouteIdState("SET_AUTOMOBILE_ROUTE_STATE", incomingQueue.Peek().data.routeId, "GREEN");
                                        string strSetRouteState = JsonConvert.SerializeObject(eventTypeRouteIdState);
                                        webSocket.Send(strSetRouteState);
                                    }
                                } 
                            }

                            if (e.Data.Contains("\"ENTITY_EXITED_ZONE\""))  //EventTypeRouteIdSensorId
                            {
                                EventTypeRouteIdSensorId eventTypeRouteIdSensorId = JsonConvert.DeserializeObject<EventTypeRouteIdSensorId>(e.Data);
                                Console.WriteLine($"ENTITY_EXITED_ZONE: {eventTypeRouteIdSensorId.data.routeId}");

                                if (eventTypeRouteIdSensorId.data.sensorId == 1 || eventTypeRouteIdSensorId.data.sensorId == 3)
                                {
                                    outgoingQueue.Enqueue(eventTypeRouteIdSensorId);
                                    Console.WriteLine($"Exited queue count: {outgoingQueue.Count}");

                                    if (incomingQueue.Count > 0)
                                    {
                                        EventTypeRouteIdState eventTypeRouteIdState = new EventTypeRouteIdState("SET_AUTOMOBILE_ROUTE_STATE", incomingQueue.Peek().data.routeId, "RED");
                                        string strSetRouteState = JsonConvert.SerializeObject(eventTypeRouteIdState);
                                        webSocket.Send(strSetRouteState);
                                        Console.WriteLine($"This route is put on RED: {eventTypeRouteIdState.data.routeId}");
                                        incomingQueue.Dequeue();
                                        outgoingQueue.Dequeue();
                                    }
                                }     
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