namespace KruispuntSimulatieController
{
    internal class ConnectController
    {
        public string eventType;
        public Data data;

        public class Data
        {
            public string sessionName { get; set; }
            public int sessionVersion { get; set; }
            public bool discardParseErrors { get; set; }
            public bool discardEventTypeErrors { get; set; }
            public bool discardMalformedDataErrors { get; set; }
            public bool discardInvalidStateErrors { get; set; }
        }

        public ConnectController(string evenType, string sessionName, int sessionVersion, bool discardParseErrors, bool discardEventTypeErrors, bool discardMalformedDataErrors, bool discardInvalidStateErrors)
        {
            this.eventType = evenType;
            data = new Data()
            {
                sessionName = sessionName,
                sessionVersion = sessionVersion,
                discardParseErrors = discardParseErrors,
                discardEventTypeErrors = discardEventTypeErrors,
                discardMalformedDataErrors = discardMalformedDataErrors,
                discardInvalidStateErrors = discardInvalidStateErrors
            };
        }
    }
}