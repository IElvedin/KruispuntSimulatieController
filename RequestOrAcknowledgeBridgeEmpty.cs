using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruispuntSimulatieController
{
    public class RequestOrAcknowledgeBridgeEmpty
    {
        public string eventType;

        public RequestOrAcknowledgeBridgeEmpty(string eventType)
        {
            this.eventType = eventType;
        }
    }
}
