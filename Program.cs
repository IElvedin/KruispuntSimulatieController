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
            Console.WriteLine("Traffic simualator started!");
            TrafficSimulatorController traffic_Simulator_Controller = new TrafficSimulatorController();
            traffic_Simulator_Controller.Run();
            Console.ReadLine();
        }
    }
}