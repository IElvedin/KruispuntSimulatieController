using System.Text.Json;
using KruispuntSimulatieController.ProtocolModels;
using WebSocketSharp;

namespace KruispuntSimulatieController.RouteDataModels
{
    public sealed class BridgeLightState
    {
        private static BridgeLightState _instance;
        private readonly BridgeInformation _bridgeInformation;
        private static readonly object Padlock = new object();

        private BridgeLightState()
        {
            _bridgeInformation = new BridgeInformation();
        }
        public static BridgeLightState GetInstance()
        {
            if (_instance == null)
            {
                lock (Padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new BridgeLightState();
                    }
                }
            }

            return _instance;
        }
        public void SetRouteId(int routeId)
        {
            _bridgeInformation.routeId = routeId;
        }
        public void ChangeBoatRouteEventType(string eventType)
        {
            if (_bridgeInformation.routeId != 0)
            {
                _bridgeInformation.eventType = eventType;
            }
        }
        public void ChangeBoatRouteState(string state)
        {
            if (_bridgeInformation.routeId != 0)
            {
                _bridgeInformation.state = state;
            }
        }
        public void BridgeSequence(WebSocket webSocket)
        {
            string json = "";

            if (_bridgeInformation.eventType == "ENTITY_ENTERED_ZONE")
            {
                _bridgeInformation.eventType = "SET_BRIDGE_WARNING_LIGHT_STATE";
                _bridgeInformation.state = "ON";
                json = JsonSerializer.Serialize(FillChangedBridgeState(_bridgeInformation.state, _bridgeInformation.eventType));
            }
            else if (_bridgeInformation.eventType == "SET_BRIDGE_WARNING_LIGHT_STATE" || _bridgeInformation.state == "ON")
            {
                _bridgeInformation.eventType = "REQUEST_BRIDGE_ROAD_EMPTY";
                json = JsonSerializer.Serialize(FillBridgeRequestEmpty(_bridgeInformation.eventType));
            }
            else if (_bridgeInformation.eventType == "ACKNOWLEDGE_BRIDGE_ROAD_EMPTY")
            {
                _bridgeInformation.eventType = "REQUEST_BARRIERS_STATE";
                _bridgeInformation.state = "DOWN";
                json = JsonSerializer.Serialize(FillChangedBridgeState(_bridgeInformation.state, _bridgeInformation.eventType));
            }
            else if (_bridgeInformation.eventType == "ACKNOWLEDGE_BRIDGE_ROAD_EMPTY" || _bridgeInformation.state == "DOWN")
            {
                _bridgeInformation.eventType = "SET_BOAT_ROUTE_STATE";
                _bridgeInformation.state = "REDGREEN";
                json = JsonSerializer.Serialize(FillChangedBoatRouteState(_bridgeInformation.routeId ,_bridgeInformation.state, _bridgeInformation.eventType));
            }
            else if (_bridgeInformation.eventType == "ACKNOWLEDGE_BRIDGE_ROAD_EMPTY" || _bridgeInformation.state == "UP")
            {
                _bridgeInformation.eventType = "SET_BRIDGE_WARNING_LIGHT_STATE";
                _bridgeInformation.state = "OFF";
                json = JsonSerializer.Serialize(FillChangedBridgeState(_bridgeInformation.state, _bridgeInformation.eventType));
                _bridgeInformation.routeId = 0; //Reset the routeId
            }
            else if (_bridgeInformation.eventType == "ACKNOWLEDGE_BRIDGE_STATE" || _bridgeInformation.state == "UP")
            {
                _bridgeInformation.eventType = "SET_BOAT_ROUTE_STATE";
                _bridgeInformation.state = "GREEN";
                json = JsonSerializer.Serialize(FillChangedBoatRouteState(_bridgeInformation.routeId ,_bridgeInformation.state, _bridgeInformation.eventType));
            }
            else if (_bridgeInformation.eventType == "SET_BOAT_ROUTE_STATE" || _bridgeInformation.state == "GREEN")
            {
                _bridgeInformation.eventType = "REQUEST_BRIDGE_WATER_EMPTY";
                json = JsonSerializer.Serialize(FillBridgeRequestEmpty(_bridgeInformation.eventType));
            }
            else if (_bridgeInformation.eventType == "ACKNOWLEDGE_BRIDGE_STATE" || _bridgeInformation.state == "DOWN")
            {
                _bridgeInformation.eventType = "REQUEST_BARRIERS_STATE";
                _bridgeInformation.state = "UP";
                json = JsonSerializer.Serialize(FillChangedBridgeState(_bridgeInformation.state, _bridgeInformation.eventType));
            }
            else if (_bridgeInformation.eventType == "ACKNOWLEDGE_BRIDGE_WATER_EMPTY")
            {
                _bridgeInformation.eventType = "SET_BOAT_ROUTE_STATE";
                _bridgeInformation.state = "RED";
                json = JsonSerializer.Serialize(FillChangedBoatRouteState(_bridgeInformation.routeId ,_bridgeInformation.state, _bridgeInformation.eventType));
            }
            else if (_bridgeInformation.eventType == "SET_BOAT_ROUTE_STATE" || _bridgeInformation.state == "RED")
            {
                _bridgeInformation.eventType = "REQUEST_BRIDGE_STATE";
                _bridgeInformation.state = "DOWN";
                json = JsonSerializer.Serialize(FillChangedBridgeState(_bridgeInformation.state, _bridgeInformation.eventType));
            }

            #region SwitchCaseMogelijkheid

            /*string eventType = "";
            switch (_bridgeInformation.eventType)
            {
                case "ENTITY_ENTERED_ZONE":
                    eventType = "SET_BRIDGE_WARNING_LIGHT_STATE";
                    json = JsonSerializer.Serialize(FillChangedBridgeState("ON", eventType));
                    webSocket.Send(json);

                    eventType = "REQUEST_BRIDGE_ROAD_EMPTY";
                    json = JsonSerializer.Serialize(FillBridgeRequestEmpty(eventType));
                    webSocket.Send(json);
                    break;
                
                case "ACKNOWLEDGE_BRIDGE_ROAD_EMPTY":
                    eventType = "REQUEST_BARRIERS_STATE";
                    json = JsonSerializer.Serialize(FillChangedBridgeState("DOWN", eventType));
                    webSocket.Send(json);
                    break;
                
                case "ACKNOWLEDGE_BARRIERS_STATE":
                    if (_bridgeInformation.state == "DOWN")
                    {
                        eventType = "SET_BOAT_ROUTE_STATE";
                        json = JsonSerializer.Serialize(FillChangedBoatRouteState(_bridgeInformation.routeId, "REDGREEN", eventType));
                        webSocket.Send(json);

                        eventType = "REQUEST_BRIDGE_STATE";
                        json = JsonSerializer.Serialize(FillChangedBridgeState("UP", eventType));
                    }
                    else if (_bridgeInformation.state == "UP")
                    {
                        eventType = "SET_BRIDGE_WARNING_LIGHT_STATE";
                        json = JsonSerializer.Serialize(FillChangedBridgeState("OFF", eventType));
                        webSocket.Send(json);
                        _bridgeInformation.routeId = 0;
                    }
                    break;
                
                case "ACKNOWLEDGE_BRIDGE_STATE":
                    if (_bridgeInformation.state == "UP")
                    {
                        eventType = "SET_BOAT_ROUTE_STATE";
                        json = JsonSerializer.Serialize(FillChangedBoatRouteState(_bridgeInformation.routeId, "GREEN", eventType));
                        webSocket.Send(json);

                        eventType = "REQUEST_BRIDGE_WATER_EMPTY";
                        json = JsonSerializer.Serialize(FillBridgeRequestEmpty(eventType));
                        webSocket.Send(json);
                    }
                    else if (_bridgeInformation.state == "DOWN")
                    {
                        eventType = "REQUEST_BARRIERS_STATE";
                        json = JsonSerializer.Serialize(FillChangedBridgeState("UP", eventType));
                        webSocket.Send(json);
                    }
                    break;
                
                case  "ACKNOWLEDGE_BRIDGE_WATER_EMPTY":
                    eventType = "SET_BOAT_ROUTE_STATE";
                    json = JsonSerializer.Serialize(FillChangedBoatRouteState(_bridgeInformation.routeId, "RED", eventType));
                    webSocket.Send(json);

                    eventType = "REQUEST_BRIDGE_STATE";
                    json = JsonSerializer.Serialize(FillChangedBridgeState("DOWN", eventType));
                    break;
                
            }*/

            #endregion
            webSocket.Send(json);
        }
        private EventTypeStateModel FillChangedBridgeState(string state, string eventType)
        {
            EventTypeStateModelData eventTypeStateModelData = new EventTypeStateModelData()
            {
                state = state
            };
            EventTypeStateModel eventTypeStateModel = new EventTypeStateModel()
            {
                eventType = eventType,
                data = eventTypeStateModelData
            };
            return eventTypeStateModel;
        }
        private EventTypeRouteIdStateModel FillChangedBoatRouteState(int routeId, string state, string eventType)
        {
            EventTypeRouteIdStateModelData eventTypeRouteIdStateModelData = new EventTypeRouteIdStateModelData()
            {
                routeId = routeId,
                state = state
            };
            EventTypeRouteIdStateModel eventTypeRouteIdStateModel = new EventTypeRouteIdStateModel()
            {
                eventType = eventType,
                data = eventTypeRouteIdStateModelData
            };
            return eventTypeRouteIdStateModel;
        }
        private EventTypeModel FillBridgeRequestEmpty(string eventType)
        {
            EventTypeModel eventTypeModel = new EventTypeModel()
            {
                eventType = eventType
            };
            return eventTypeModel;
        }
    }
}