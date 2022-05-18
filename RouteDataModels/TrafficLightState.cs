using System.Collections.Generic;
using System.Text.Json;
using KruispuntSimulatieController.ProtocolModels;
using WebSocketSharp;

namespace KruispuntSimulatieController.RouteDataModels
{
    public sealed class TrafficLightState
    {
        private static TrafficLightState _instance;

        private static readonly object Padlock = new object();

        private TrafficLightState()
        {
        }

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

        public void SendChangedStates(List<int> bestRouteCombination, WebSocket webSocket, string targetState)
        {
            List<List<int>> routesList = new AllRoutes().GetAllRoutes();
            List<int> boatRoutes = new List<int>() { 41, 42 };

            foreach (int route in bestRouteCombination)
            {
                string eventType = "";
                if (routesList[0].Contains(route))
                {
                    eventType = "SET_AUTOMOBILE_ROUTE_STATE";
                }
                else if (routesList[1].Contains(route))
                {
                    eventType = "SET_CYCLIST_ROUTE_STATE";
                }
                else if (routesList[2].Contains(route))
                {
                    eventType = "SET_PEDESTRIAN_ROUTE_STATE";
                }
                else //(routesList[3].Contains(route))
                {
                    continue;
                }

                if (!boatRoutes.Contains(route))
                {
                    string json = JsonSerializer.Serialize(FillChangedState(route, targetState, eventType));
                    webSocket.Send(json);
                }
            }
        }

        private EventTypeRouteIdStateModel FillChangedState(int route, string state, string eventType)
        {
            List<int> pedestrianRoutes = new AllRoutes().GetAllRoutes()[2];
            if (pedestrianRoutes.Contains(route) && state == "ORANGE")
            {
                state = "BLINKING";
            }

            EventTypeRouteIdStateModelData EventTypeRouteIdStateModelData = new EventTypeRouteIdStateModelData()
            {
                routeId = route,
                state = state
            };

            EventTypeRouteIdStateModel eventTypeRouteIdStateModel = new EventTypeRouteIdStateModel()
            {
                eventType = eventType,
                data = EventTypeRouteIdStateModelData
            };
            return eventTypeRouteIdStateModel;
        }
    }
}