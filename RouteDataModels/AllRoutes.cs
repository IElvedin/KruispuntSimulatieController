using System.Collections.Generic;
using KruispuntSimulatieController.RouteDataModels.AllRouteModels;

namespace KruispuntSimulatieController.RouteDataModels
{
    public class AllRoutes
    {
        private readonly List<List<int>> _allRoutesList;
        private readonly List<int> _carRoutes = new CarRoutes().carRoutesList;
        private readonly List<int> _bicyclesRoutes = new BicyclesRoutes().bicyclesRoutesList;
        private readonly List<int> _pedestriansRoutes = new PedestriansRoutes().pedestriansRoutesList;
        private readonly List<int> _boatsRoutes = new BoatsRoutes().boatsRoutesList;

        public AllRoutes()
        {
            _allRoutesList = new List<List<int>>()
            {
                //Cars
                _carRoutes,
                //Bicycles
                _bicyclesRoutes,
                //Pedestrians
                _pedestriansRoutes,
                //Boats
                _boatsRoutes
            };
        }

        public List<List<int>> GetAllRoutes()
        {
            return _allRoutesList;
        }
    }
}
