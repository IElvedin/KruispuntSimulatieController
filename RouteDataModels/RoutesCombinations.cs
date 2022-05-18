using System;
using System.Collections.Generic;

namespace KruispuntSimulatieController.RouteDataModels
{
    public class RoutesCombinations
    {
        private readonly Dictionary<int, List<int>> _routes = new Dictionary<int, List<int>>()
        {
            { 1, new List<int>() { 5, 9, 21, 24, 31, 38 } },
            { 2, new List<int>() { 5, 9, 10, 11, 12, 21, 23, 31, 36 } },
            { 3, new List<int>() { 5, 7, 8, 11, 12, 15, 21, 22, 31, 34} },
            { 4, new List<int>() { 8, 12, 15, 21, 22, 32, 33} },
            { 5, new List<int>() { 1, 2, 3, 8, 9, 11, 12, 15, 22, 23, 24, 32, 33} },
            { 7, new List<int>() { 3, 11, 15, 22, 23, 34, 35 } },
            { 8, new List<int>() { 3, 4, 5, 11, 12, 21, 23, 32, 35 } },
            { 9, new List<int>() { 1, 2, 5, 11, 12, 23, 24, 35, 38 } },
            { 10, new List<int>() { 2, 23, 24, 36, 37 } },
            { 11, new List<int>() { 2, 3, 5, 7, 8, 9, 15, 22, 24, 34, 37 } },
            { 12, new List<int>() { 2, 3, 4, 5, 8, 9, 21, 24, 32, 37 } },
            { 15, new List<int>() { 3, 4, 5, 7, 11 } },
            { 21, new List<int>() { 1, 2, 3, 4, 12 } },
            { 22, new List<int>() { 3, 4, 5, 7, 11 } },
            { 23, new List<int>() { 2, 5, 7, 8, 9, 10 } },
            { 24, new List<int>() { 2, 5, 7, 8, 9, 10 } },
            { 31, new List<int>() { 1, 2, 3 } },
            { 32, new List<int>() { 4, 8, 12 } },
            { 33, new List<int>() { 4, 5 } },
            { 34, new List<int>() { 3, 7, 11 } },
            { 35, new List<int>() { 1, 2, 3 } },
            { 36, new List<int>() { 2, 5, 10 } },
            { 37, new List<int>() { 10, 11, 12 } },
            { 38, new List<int>() { 1, 5, 9 } },
            { 41, new List<int>() { 42 } },
            { 42, new List<int>() { 41 } },
        };

        private static RoutesCombinations _instance;
        
        private static readonly object Padlock = new object();

        private RoutesCombinations() { }

        public static RoutesCombinations GetInstance()
        {
            if (_instance == null)
            {
                lock (Padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new RoutesCombinations();
                    }
                }
            }
            return _instance;
        }
        
        public bool IsRouteCompatible(int key, List<int> routesList)
        {
            if (routesList.Count < 1)
            {
                return true;
            }

            if (!_routes.ContainsKey(key))
            {
                throw new Exception($"key not found {key}");
            }
            foreach (int item in routesList)
            {
                if (_routes[key].Contains(item))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
