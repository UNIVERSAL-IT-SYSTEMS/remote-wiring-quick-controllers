using Microsoft.Maker.RemoteWiring;

namespace UWPRobotController
{
    public class RomeoControl : IControl
    {

        byte E1 = 0x05;
        byte E2 = 0x06;
        byte M1 = 0x04;
        byte M2 = 0x07;

        public RomeoControl()
        {
            // setup
            App.arduino.pinMode(E1, PinMode.PWM);
            App.arduino.pinMode(E2, PinMode.PWM);
            App.arduino.pinMode(M1, PinMode.OUTPUT);
            App.arduino.pinMode(M2, PinMode.OUTPUT);
        }

        public void forward(byte l, byte r)
        {
            App.firmata.@lock();
            App.firmata.write(0x90);
            App.firmata.write(0x00);
            App.firmata.write(0x00);

            App.firmata.write(0xE5);
            App.firmata.sendValueAsTwo7bitBytes(r);
            App.firmata.write(0xE6);
            App.firmata.sendValueAsTwo7bitBytes(l);

            App.firmata.flush();
            App.firmata.@unlock();
        }

        public void backward(byte l, byte r)
        {
            App.firmata.@lock();
            App.firmata.write(0x90);
            App.firmata.write(0x10);
            App.firmata.write(0x01);

            App.firmata.write(0xE5);
            App.firmata.sendValueAsTwo7bitBytes(r);
            App.firmata.write(0xE6);
            App.firmata.sendValueAsTwo7bitBytes(l);
            App.firmata.flush();
            App.firmata.@unlock();

        }

        public void left(byte l, byte r)
        {
            App.firmata.@lock();
            App.firmata.write(0x90);
            App.firmata.write(0x00);
            App.firmata.write(0x01);

            App.firmata.write(0xE5);
            App.firmata.sendValueAsTwo7bitBytes(r);
            App.firmata.write(0xE6);
            App.firmata.sendValueAsTwo7bitBytes(l);
            App.firmata.flush();
            App.firmata.@unlock();
        }

        public void right(byte l, byte r)
        {
            App.firmata.@lock();
            App.firmata.write(0x90);
            App.firmata.write(0x10);
            App.firmata.write(0x00);

            App.firmata.write(0xE5);
            App.firmata.sendValueAsTwo7bitBytes(r);
            App.firmata.write(0xE6);
            App.firmata.sendValueAsTwo7bitBytes(l);
            App.firmata.flush();
            App.firmata.@unlock();
        }

        public void stop()
        {
            App.firmata.@lock();
            App.firmata.write(0xE5);
            App.firmata.sendValueAsTwo7bitBytes(0x00);
            App.firmata.write(0xE6);
            App.firmata.sendValueAsTwo7bitBytes(0x00);
            App.firmata.flush();
            App.firmata.@unlock();
        }

        bool isLaserOn = false;

        public void activateLaser()
        {
            App.arduino.pinMode(13, PinMode.OUTPUT);
            if (isLaserOn)
                App.arduino.digitalWrite(13, PinState.LOW);
            else
                App.arduino.digitalWrite(13, PinState.HIGH);
            isLaserOn = !isLaserOn;
        }
    }
}
