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
            App.arduino.pinMode(E1, PinMode.OUTPUT);
            App.arduino.pinMode(E2, PinMode.OUTPUT);
            App.arduino.pinMode(M1, PinMode.OUTPUT);
            App.arduino.pinMode(M2, PinMode.OUTPUT);

        }

        public void forward(byte a, byte b)
        {
            App.arduino.digitalWrite(M1, PinState.HIGH);
            App.arduino.digitalWrite(M2, PinState.HIGH);
            App.arduino.analogWrite(E1, a);      //PWM Speed Control
            App.arduino.analogWrite(E2, b);
        }

        public void backward(byte a, byte b)
        {
            App.arduino.digitalWrite(M1, PinState.LOW);
            App.arduino.digitalWrite(M2, PinState.LOW);
            App.arduino.analogWrite(E1, a);
            App.arduino.analogWrite(E2, b);
        }

        public void left(byte a, byte b)
        {
            App.arduino.digitalWrite(M1, PinState.LOW);
            App.arduino.digitalWrite(M2, PinState.HIGH);
            App.arduino.analogWrite(E1, a);
            App.arduino.analogWrite(E2, b);
        }

        public void right(byte a, byte b)
        {
            App.arduino.digitalWrite(M1, PinState.HIGH);
            App.arduino.digitalWrite(M2, PinState.LOW);
            App.arduino.analogWrite(E1, a);
            App.arduino.analogWrite(E2, b);
        }

        public void stop()
        {
            App.arduino.digitalWrite(E1, PinState.LOW);
            App.arduino.digitalWrite(E2, PinState.LOW);
        }
    }
}
