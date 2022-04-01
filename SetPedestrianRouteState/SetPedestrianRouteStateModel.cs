using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruispuntSimulatieController.SetPedestrianRouteState
{
    class SetPedestrianRouteStateJSONModel
    {
        public string eventType { get; set; }
        public SetPedestrianRouteStateJSONModelData data {get; set;}
    }
}
