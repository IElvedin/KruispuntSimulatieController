using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruispuntSimulatieController.ProtocolModels
{
    public class ErrorMalformedMessageModelData
    {
        public string receivedMessage { get; set; }
        public string[] errors { get; set; }
    }
}
