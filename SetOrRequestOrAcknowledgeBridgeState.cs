namespace KruispuntSimulatieController
{
    public class SetOrRequestOrAcknowledgeBridgeState
    {
        public string eventType;
        public Data data;

        public class Data
        {
            public string state { get; set; }
        }

        public SetOrRequestOrAcknowledgeBridgeState(string eventType, string state)
        {
            this.eventType = eventType;
            data = new Data()
            {
                state = state
            };
        }
    }
}