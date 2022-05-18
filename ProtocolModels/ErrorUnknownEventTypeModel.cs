using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruispuntSimulatieController.ProtocolModels
{
    public class ErrorUnknownEventTypeModel
    {
        public string eventType { get; set; }
        public ErrorUnknownEventTypeModelData data { get; set; }
    }
}
