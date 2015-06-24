namespace UWPRobotController.Comm
{
    public class Connection
    {
        public string DisplayName { get; set; }
        public object Source { get; set; }

        public Connection(string displayName, object source)
        {
            DisplayName = displayName;
            Source = source;
        }
    }
}