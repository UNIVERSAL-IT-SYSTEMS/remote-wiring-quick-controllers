using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maker.Serial;
using Microsoft.Maker.RemoteWiring;

namespace UWPRobotController
{
    public class RobotControl : IControl
    {

        byte E1 = 0x05;
        byte E2 = 0x06;
        byte M1 = 0x04;
        byte M2 = 0x07;


        public RobotControl()
        {
            // setup
            for (byte i = 4; i <= 7; i++)
                App.arduino.pinMode(i, PinMode.OUTPUT);

        }

        public void forward(byte a, byte b)
        {
            App.arduino.analogWrite(E1, a);      //PWM Speed Control
            App.arduino.digitalWrite(M1, PinState.HIGH);
            App.arduino.analogWrite(E2, b);
            App.arduino.digitalWrite(M2, PinState.HIGH);
        }

        public void backward(byte a, byte b)
        {
            App.arduino.analogWrite(E1, a);
            App.arduino.digitalWrite(M1, PinState.LOW);
            App.arduino.analogWrite(E2, b);
            App.arduino.digitalWrite(M2, PinState.LOW);
        }

        public void left(byte a, byte b)
        {
            App.arduino.analogWrite(E1, a);
            App.arduino.digitalWrite(M1, PinState.LOW);
            App.arduino.analogWrite(E2, b);
            App.arduino.digitalWrite(M2, PinState.HIGH);
        }

        public void right(byte a, byte b)
        {
            App.arduino.analogWrite(E1, a);
            App.arduino.digitalWrite(M1, PinState.HIGH);
            App.arduino.analogWrite(E2, b);
            App.arduino.digitalWrite(M2, PinState.LOW);
        }

        public void stop()
        {
            App.arduino.digitalWrite(E1, PinState.LOW);
            App.arduino.digitalWrite(E2, PinState.LOW);
        }
    }
}
