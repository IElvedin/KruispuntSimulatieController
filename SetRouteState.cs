namespace KruispuntSimulatieController
{
    public class SetRouteState
    {
        public string eventType;
        public Data data;

        public class Data
        {
            public int routeId { get; set; }
            public string state { get; set; }
        }

        public SetRouteState (string eventType, int routeId, string state)
        {
            this.eventType = eventType;
            data = new Data()
            {
                routeId = routeId,
                state = state
            };
        }
    }
}
