namespace KruispuntSimulatieController
{
    internal class EntityEnteredOrExitedZone
    {
        public string eventType;
        public Data data;

        public class Data
        {
            public int routeId { get; set; }
            public int sensorId { get; set; }
        }

        public EntityEnteredOrExitedZone(string eventType, int routeId, int sensorId)
        {
            this.eventType = eventType;
            data = new Data()
            {
                routeId = routeId,
                sensorId = sensorId
            };
        }
    }
}