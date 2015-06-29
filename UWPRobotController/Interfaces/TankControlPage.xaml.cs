using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

using Windows.System.Threading;

namespace UWPRobotController.Interfaces
{
    public sealed partial class TankControlPage : Page
    {
        DispatcherTimer timer;
        private int left;
        private int right;

        public TankControlPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

        private void right_motor_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            UpdateControls();
        }

        private void left_motor_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            UpdateControls();
        }

        private void UpdateControls()
        {
            left = (int)left_motor.Value;
            right = (int)right_motor.Value;

            byte l = (byte)Math.Abs(left_motor.Value);
            byte r = (byte)Math.Abs(right_motor.Value);

            if (right >= 0 && left >= 0)
            {
                App.control.forward(l, r);
            }
            else if (right <= 0 && left <= 0)
            {
                App.control.backward(l, r);
            }
            else if (right >= 0 && left <= 0)
            {
                App.control.left(l, r);
            }
            else if (right <= 0 && left >= 0)
            {
                App.control.right(l, r);
            }
            if (right == 0 && left == 0)
                App.control.stop();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
