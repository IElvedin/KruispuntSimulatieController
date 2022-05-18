using System;
using System.Collections.Generic;
using System.Linq;

namespace KruispuntSimulatieController.RouteDataModels
{
    public sealed class TrafficLightEntityAmount
    {
        private Dictionary<int, int> _TrafficLights = new Dictionary<int, int>()
        {
            //Cars
            { 1, 0 }, { 2, 0 }, { 3, 0}, { 4, 0}, { 5, 0}, { 7, 0}, { 8, 0}, { 9, 0}, { 10, 0}, { 11, 0}, { 12, 0}, { 15, 0},
            //Bicycles
            { 21, 0}, { 22, 0}, { 23, 0}, { 24, 0},
            //Pedestrians
            { 31, 0}, { 32, 0}, { 33, 0}, { 34, 0}, { 35, 0}, { 36, 0}, { 37, 0}, { 38, 0},
            //Boats
            { 41, 0}, { 42, 0}
        };

        private static TrafficLightEntityAmount _instance;


        private static readonly object Padlock = new object();

        private int _requestCount = 0;

        private TrafficLightEntityAmount() { }

        public static TrafficLightEntityAmount GetInstance()
        {
            if (_instance == null)
            {
                lock (Padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new TrafficLightEntityAmount();
                    }
                }
            }
            return _instance;
        }

        public void ChangeTrafficLightAmount(int key)
        {
            _requestCount++;
            if (_TrafficLights.ContainsKey(key))
            {
                if (_TrafficLights[key] == 0)
                {
                    _TrafficLights[key] += _requestCount;
                }                
                
            }
            else
            {
                Console.WriteLine("Route not found.");
            }

        }        

        public List<int> GetPriorityRoutes()
        {
            List<int> newPriorityList = new List<int>();

            IEnumerable<KeyValuePair<int, int>> orderedList = _TrafficLights.OrderBy(route => route.Value);

            foreach (KeyValuePair<int, int> route in orderedList)
            {
                if (route.Value != 0)
                {
                    if (RoutesCombinations.GetInstance().IsRouteCompatible(route.Key, newPriorityList))
                    {
                        newPriorityList.Add(route.Key);
                    }
                }
            }

            return newPriorityList;
        }

        public void ResetFromList(List<int> trafficlights)
        {
            foreach (int item in trafficlights)
            {
                _TrafficLights[item] = 0;
            }
        }
    }
}