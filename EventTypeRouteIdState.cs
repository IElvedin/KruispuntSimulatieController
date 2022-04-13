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

        public void JSONMessageConverter(EventTypeRouteIdState routeState, WebSocket webSocket, List<EventTypeRouteIdState> routeStatuses)
        {
            string strSetRouteState = JsonConvert.SerializeObject(routeState);
            webSocket.Send(strSetRouteState);
            routeStatuses.Add(routeState);
        }
    }    
}