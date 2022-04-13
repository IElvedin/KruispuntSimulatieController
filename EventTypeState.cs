namespace KruispuntSimulatieController
{
    public class EventTypeState
    {
        public string eventType;
        public Data data;

        public class Data
        {
            public string state { get; set; }
        }

        public EventTypeState(string eventType, string state)
        {
            this.eventType = eventType;
            data = new Data()
            {
                state = state
            };
        }
    }
}