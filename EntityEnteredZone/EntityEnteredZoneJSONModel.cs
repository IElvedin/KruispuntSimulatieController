using KruispuntSimulatieController.EntityEnteredZone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruispuntSimulatieController.EntityEnteredZone
{
    public class EntityEnteredZoneJSONModel
    {
        public string eventType { get; set; }
        public EntityEnteredZoneJSONModelData data { get; set; }
    }
}
