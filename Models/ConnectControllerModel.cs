using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruispuntSimulatieController.Models
{
    class ConnectControllerModel
    {
        public string eventType { get { return eventType; } set  { eventType = "CONNECT_CONTROLLER"; } }
        public string sessionName { get { return sessionName; } set { sessionName = "groep8"; } }
        public int sessionVersion { get { return sessionVersion; } set { sessionVersion = 1; } }
        public bool discardParseErrors { get { return discardParseErrors; } set { discardParseErrors = false; } }
        public bool discardEventTypeErrors { get { return discardEventTypeErrors; } set { discardEventTypeErrors = false; } }
        public bool discardMalformedDataErrors { get { return discardMalformedDataErrors; } set { discardMalformedDataErrors = false; } }
        public bool discardInvalidStateErrors { get { return discardInvalidStateErrors; } set { discardInvalidStateErrors = false; } }
    }
}
