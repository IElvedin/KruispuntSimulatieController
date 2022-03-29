using KruispuntSimulatieController.Parsers.EntityEnteredZone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruispuntSimulatieController.Parsers
{
    class EntityEnteredZoneModel
    {
        public string eventType { get; set; }
        public EntityEnteredZoneModelData data { get; set; }
    }
}
