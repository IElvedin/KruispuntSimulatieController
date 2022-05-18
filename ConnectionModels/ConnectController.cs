namespace KruispuntSimulatieController.ConnectionModels
{
    public class ConnectController
    {
        public string eventType { get; set; } = "CONNECT_CONTROLLER";
        public ConnectionDataModel data { get; set; }
    }
}
