using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WebSocketSharp;

namespace KruispuntSimulatieController
{
    public class EventTypeRouteIdState
    {
        public string eventType;
        public Data data;

        public class Data
        {
            public int routeId { get; set; }
            public string state { get; set; }
        }

        public EventTypeRouteIdState(string eventType, int routeId, string state)
        {
            this.eventType = eventType;
            data = new Data()
            {
                routeId = routeId,
                state = state
            };
        }

        //public EventTypeRouteIdState buildRouteState(string eventType, List<int> routeList, int index, string state, WebSocket webSocket, List<EventTypeRouteIdState> routeStatuses)
        public void buildRouteState(EventTypeRouteIdState routeIdState, WebSocket webSocket)
        {
            string strSetRouteState = JsonConvert.SerializeObject(routeIdState);
            webSocket.Send(strSetRouteState);
        }

        public void JSONMessageConverter(EventTypeRouteIdState routeState, WebSocket webSocket, List<EventTypeRouteIdState> routeStatuses)
        {
            string strSetRouteState = JsonConvert.SerializeObject(routeState);
            webSocket.Send(strSetRouteState);
            routeStatuses.Add(routeState);
        }
    }    
}