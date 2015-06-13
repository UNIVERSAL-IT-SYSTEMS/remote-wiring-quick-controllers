using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPRobotController.Comm
{
    public class Connection
    {
        public string DisplayName { get; set; }
        public object Source { get; set; }

        public Connection(string displayName, object source)
        {
            this.DisplayName = displayName;
            this.Source = source;
        }
    }
}