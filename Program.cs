using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace KruispuntSimulatieController
{
    class Program
    {
        private static readonly string address = "ws://keyslam.com:8080 ";

        static void Main(string[] args)
        {
            //Maakt connectie met broker
            using (WebSocket websocket = new WebSocket(address))
            {
                //Maak connectie met broker
                websocket.OnMessage += Websocket_OnMessage;
                websocket.Connect();

                //Connect Controller
                JSONControllerConnectData jSONMessageDataModel = new JSONControllerConnectData()
                {
                    eventType = "CONNECT_CONTROLLER",
                    data = new ControllerConnectDataModel()
                    {
                        sessionName = "MoffelMaffia",
                        sessionVersion = 1,
                        discardParseErrors = false,
                        discardEventTypeErrors = false,
                        discardMalformedDataErrors = false,
                        discardInvalidStateErrors = false
                    }
                };
                string strConnection = JsonConvert.SerializeObject(jSONMessageDataModel);
                Console.WriteLine(strConnection);
                websocket.Send(strConnection);

                Console.WriteLine();

                if (websocket.IsAlive)
                {
                    Console.WriteLine("Tried making connection");
                }

                Console.WriteLine();

                //Set Automobile Route State
                JSONSetAutombileRouteState jSONSetAutombileRouteState = new JSONSetAutombileRouteState()
                {
                    eventType = "SET_AUTOMOBILE_ROUTE_STATE",
                    data = new SetAutomobileRouteStateModel()
                    {
                        routeId = new int[]{ 1, 2, 3, 4, 5, 7, 8, 9, 10, 11, 12, 15},
                        state = new string[] { "GREEN", "ORANGE", "RED" }
                    }
                };
                string strSetAutomobileRouteState = JsonConvert.SerializeObject(jSONSetAutombileRouteState);
                Console.WriteLine(strSetAutomobileRouteState);
                websocket.Send(strSetAutomobileRouteState);

                Console.ReadKey();
            }
            
        }

        private static void Websocket_OnMessage(object sender, MessageEventArgs e)
        {
            Console.WriteLine("Broker zegt: " + e.Data);
        }
    }
}
