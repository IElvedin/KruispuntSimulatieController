using KruispuntSimulatieController.ProtocolModels;
using KruispuntSimulatieController.Route.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KruispuntSimulatieController
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
            int amount = 1;
            if (_TrafficLights.ContainsKey(key))
            {
                _TrafficLights[key] += amount;
                Console.WriteLine($"Changed route: {key} to amount: {_TrafficLights[key]}");
            }
            else
                Console.WriteLine("Route not found.");
        }

        public List<int> GetPriorityRoutes(List<int> oldPriorityList)
        {
            List<int> newPriorityList = new List<int>();

            int count = 0;            

            foreach (KeyValuePair<int, int> route in _TrafficLights.OrderBy(route => route.Value))
            {
                if (route.Value != 0)
                {
                    if (oldPriorityList != null && oldPriorityList.Contains(route.Key))
                    {
                        newPriorityList.Insert(count, route.Key);
                    }
                    else
                    {
                        newPriorityList.Insert(count, route.Key);
                        count++;
                    }
                }
            }

            return newPriorityList;
        }
    }
}