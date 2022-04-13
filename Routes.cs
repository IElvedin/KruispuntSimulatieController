using System;
using System.Collections.Generic;
using WebSocketSharp;

namespace KruispuntSimulatieController
{
    public class Routes
    {
        private List<List<int>> _allRoute = new List<List<int>>();
        private List<List<int>> _possibleRoute = new List<List<int>>();
        private List<List<int>> _baseRouteLists = new List<List<int>>();

        public List<List<int>> allRoute
        {
            get
            {
                //Auto's
                _allRoute.Add(new List<int> { 1, 2, 3, 4, 5, 7, 8, 9, 10, 11, 12, 15 });
                //Fietsen
                _allRoute.Add(new List<int> { 21, 22, 23, 24 });
                //Voetgangers
                _allRoute.Add(new List<int> { 31, 32, 33, 34, 35, 36, 37, 38 });
                //Boten
                _allRoute.Add(new List<int> { 41, 42 });

                return _allRoute;
            }
        }

        public List<List<int>> possibleRoutes
        {
            get
            {
                _possibleRoute.Add(new List<int> { 1, 2, 7, 8 });
                _possibleRoute.Add(new List<int> { 1, 2, 3, 4 });
                _possibleRoute.Add(new List<int> { 1, 4, 10, 11 });
                _possibleRoute.Add(new List<int> { 1, 4, 7, 10 });
                _possibleRoute.Add(new List<int> { 1, 7, 10, 12 });
                _possibleRoute.Add(new List<int> { 3, 4, 9, 10 });
                _possibleRoute.Add(new List<int> { 4, 5, 7 });
                _possibleRoute.Add(new List<int> { 4, 7, 9 });
                _possibleRoute.Add(new List<int> { 7, 8, 9, 10 });
                return _possibleRoute;
            }
        }

        public List<List<int>> baseRouteLists
        {
            get
            {
                _baseRouteLists.Add(new List<int> { }); //0
                _baseRouteLists.Add(new List<int> { }); //1
                _baseRouteLists.Add(new List<int> { }); //2
                _baseRouteLists.Add(new List<int> { }); //3
                _baseRouteLists.Add(new List<int> { }); //4
                _baseRouteLists.Add(new List<int> { }); //5
                _baseRouteLists.Add(new List<int> { }); //6
                _baseRouteLists.Add(new List<int> { }); //7
                _baseRouteLists.Add(new List<int> { }); //8
                return _baseRouteLists;
            }
        }

        public List<EventTypeRouteIdState> FillRouteStateList(List<EventTypeRouteIdState> routeStatuses, List<List<int>> allRoutes, WebSocket webSocket)
        {
            EventTypeRouteIdState routeState;
            allRoutes = new Routes().allRoute;
            Console.WriteLine(allRoutes.Count);
            foreach (List<int> routelist in allRoutes)
            {
                foreach (int index in routelist)
                {
                    switch (allRoutes.IndexOf(routelist))
                    {
                        case 0:
                            routeState = new EventTypeRouteIdState("SET_AUTOMOBILE_ROUTE_STATE", routelist.IndexOf(index), "RED");
                            Console.WriteLine(routeState.eventType + "\t" + routeState.data.routeId + "\t" + routeState.data.state);
                            routeState.JSONMessageConverter(routeState, webSocket, routeStatuses);
                            break;

                        case 1:
                            routeState = new EventTypeRouteIdState("SET_CYCLIST_ROUTE_STATE", routelist.IndexOf(index), "RED");
                            routeState.JSONMessageConverter(routeState, webSocket, routeStatuses);
                            break;

                        case 2:
                            routeState = new EventTypeRouteIdState("SET_PEDESTRIAN_ROUTE_STATE", routelist.IndexOf(index), "RED");
                            routeState.JSONMessageConverter(routeState, webSocket, routeStatuses);
                            break;

                        case 3:
                            routeState = new EventTypeRouteIdState("SET_BOAT_ROUTE_STATE", routelist.IndexOf(index), "RED");
                            routeState.JSONMessageConverter(routeState, webSocket, routeStatuses);
                            break;
                    }
                }
            }
            return routeStatuses;
        }
    }
}