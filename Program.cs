using System;

namespace KruispuntSimulatieController
{
    public class Program
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