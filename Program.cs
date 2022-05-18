using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using WebSocketSharp;

namespace KruispuntSimulatieController
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Traffic simulation started!");
            TrafficSimulatorController trafficSimulatorController = new TrafficSimulatorController();
            trafficSimulatorController.Run();
            Console.ReadLine();
        }
    }
}