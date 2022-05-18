using KruispuntSimulatieController.Route.Data.AllRouteModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruispuntSimulatieController.Route.Data
{
    public class AllRoutes
    {
        private List<List<int>> AllRoutesList;
        private List<int> _CarRoutes = new CarRoutes().CarRoutesList;
        private List<int> _BicyclesRoutes = new BicyclesRoutes().BicyclesRoutesList;
        private List<int> _PedestriansRoutes = new PedestriansRoutes().PedestriansRoutesList;
        private List<int> _BoatsRoutes = new BoatsRoutes().BoatsRoutesList;

        public AllRoutes()
        {
            AllRoutesList = new List<List<int>>()
            {
                //Cars
                _CarRoutes,
                //Bicycles
                _BicyclesRoutes,
                //Pedestrians
                _PedestriansRoutes,
                //Boats
                _BoatsRoutes

            };
        }

        public List<List<int>> GetAllRoutes()
        {
            return AllRoutesList;
        }
    }
}
