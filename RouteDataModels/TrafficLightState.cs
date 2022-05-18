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

        public void SendChangedStates(List<int> bestRouteCombination, WebSocket webSocket, string targetState)
        {
            List<List<int>> routesList = new AllRoutes().GetAllRoutes();
            string eventType = "";
            
            foreach(int route in bestRouteCombination)
            {
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
                    eventType = "SET_BOAT_ROUTE_STATE";
                }
                string json = JsonSerializer.Serialize(FillChangedState(route, targetState, eventType));
                webSocket.Send(json);
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