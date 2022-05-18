using KruispuntSimulatieController.ProtocolModels;
using KruispuntSimulatieController.Route.Data;
using KruispuntSimulatieController.Route.Data.AllRouteModels;
using KruispuntSimulatieController.RouteDataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WebSocketSharp;

namespace KruispuntSimulatieController
{
    public sealed class TrafficLightState
    {
        private readonly Dictionary<int, string> _TrafficLightStates = new Dictionary<int, string>()
        {
            //Cars
            { 1, "RED" }, { 2, "RED" }, { 3, "RED"}, { 4, "RED"}, { 5, "RED"}, { 7, "RED"}, { 8, "RED"}, { 9, "RED"}, { 10, "RED"}, { 11, "RED"}, { 12, "RED"}, { 15, "RED"},
            //Bicycles
            { 21, "RED"}, { 22, "RED"}, { 23, "RED"}, { 24, "RED"},
            //Pedestrians
            { 31, "RED"}, { 32, "RED"}, { 33, "RED"}, { 34, "RED"}, { 35, "RED"}, { 36, "RED"}, { 37, "RED"}, { 38, "RED"},
            //Boats
            { 41, "RED"}, { 42, "RED"}
        };

        private static TrafficLightState _instance;

        private static readonly object Padlock = new object();

        private TrafficLightState() { }

        public static TrafficLightState GetInstance()
        {
            if (_instance == null)
            {
                lock (Padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new TrafficLightState();
                    }
                }
            }
            return _instance;
        }

        public void ChangeStateGreen(List<int> routes)
        {
            routes.Sort();

            int count = 0;

            
                foreach (int key in _TrafficLightStates.Keys.ToList())
                {
                    if (count < routes.Count && key == routes[count])
                    {
                        _TrafficLightStates[key] = "GREEN";
                        count++;
                    }
                    else
                    {
                        _TrafficLightStates[key] = "RED";
                    }

                    Console.WriteLine($"Changed to GREEN: route: {key} state: {_TrafficLightStates[key]}");
                }
        }

        public void ChangeStateOrange()
        {
            List<List<int>> allRoutes = new AllRoutes().GetAllRoutes();
            foreach (int key in _TrafficLightStates.Where(item => item.Value == "GREEN").Select(item => item.Key).ToList())
            {
                if (allRoutes[3].Contains(key))
                {
                    //voeg boot logica toe
                }
                else
                {
                    _TrafficLightStates[key] = "ORANGE";
                }
                Console.WriteLine($"Changed to ORANGE: route: {key} state: {_TrafficLightStates[key]}");
            }
        }

        public void ChangeStateRed()
        {
            foreach (int key in _TrafficLightStates.Where(item => item.Value == "ORANGE" || item.Value == "BLINKING").Select(item => item.Key).ToList())
            {
                _TrafficLightStates[key] = "RED";
                Console.WriteLine($"Changed to RED: route: {key} state: {_TrafficLightStates[key]}");
            }
        }

        private string eventType { get; set; }

        public async Task SendChangedStates(List<int> bestRouteCombination, WebSocket webSocket, string stateType)
        {
            List<List<int>> routesList = new AllRoutes().GetAllRoutes();

            foreach(int route in bestRouteCombination)
            {
                foreach (KeyValuePair<int, string> lightState in _TrafficLightStates)
                {
                    if (route == lightState.Key && lightState.Value == stateType)
                    {
                        switch (routesList)
                        {
                            case List<List<int>> a when routesList[0].Contains(route):
                                eventType = "SET_AUTOMOBILE_ROUTE_STATE";
                                goto default;
                            case List<List<int>> b when routesList[1].Contains(route):
                                eventType = "SET_CYCLIST_ROUTE_STATE";
                                goto default;
                            case List<List<int>> c when routesList[2].Contains(route):
                                eventType = "SET_PEDESTRIAN_ROUTE_STATE";
                                goto default;
                            case List<List<int>> d when routesList[3].Contains(route):
                                eventType = "SET_BOAT_ROUTE_STATE";
                                break;
                            default:
                                string json = JsonSerializer.Serialize(FillChangedState(lightState.Key, lightState.Value, eventType));
                                webSocket.SendAsync(json, async (bool completed) =>
                                {
                                    Console.WriteLine($"Message send: {completed}");
                                });
                                break;
                        }
                    }
                }
            }            
        }

        private EventTypeRouteIdStateModel FillChangedState(int route, string state, string eventType)
        {
            if (state == "ORANGE")
            {
                if (route == 31 || route == 32 || route == 33 || route == 34 || route == 35 || route == 36 || route == 37 || route == 38)
                {
                    state = "BLINKING";
                }
            }            

            EventTypeRouteIdStateModelData eventTypeStateModelData = new EventTypeRouteIdStateModelData()
            {
                routeId = route,
                state = state
            };

            EventTypeRouteIdStateModel eventTypeRouteIdStateModel = new EventTypeRouteIdStateModel()
            {
                eventType = eventType,
                data = eventTypeStateModelData
            };
            return eventTypeRouteIdStateModel;
        }
    }
}