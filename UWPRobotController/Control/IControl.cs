using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPRobotController
{
    interface IControl
    {
        void forward(byte a, byte b);
        void backward(byte a, byte b);
        void left(byte a, byte b);
        void right(byte a, byte b);
        void stop();
    }
}
