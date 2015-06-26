using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace UWPRobotController
{
    public sealed partial class TankControlPage : Page
    {
        private int left;
        private int right;

        public TankControlPage()
        {
            this.InitializeComponent();
        }

        private void right_motor_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            right = (int)e.NewValue;
            updateControl();
        }

        private void left_motor_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            left = (int)e.NewValue;
            updateControl();
        }

        public void updateControl()
        {
            byte a = (byte)Math.Abs(left);
            byte b = (byte)Math.Abs(right);

            if (right >= 0 && left >= 0)
            {
                App.control.forward(a, b);
            }
            else if (right <= 0 && left <= 0)
            {
                App.control.backward(a, b);
            }
            else if (right >= 0 && left <= 0)
            {
                App.control.left(a, b);
            }
            else if (right <= 0 && left >= 0)
            {
                App.control.right(a, b);
            }
            if (right == 0 && left == 0)
                App.control.stop();
        }

        private void Reconnect_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ArduinoConnectionPage));
        }

        private void DpadController_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
