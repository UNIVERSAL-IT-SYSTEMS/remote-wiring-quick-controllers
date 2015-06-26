using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace UWPRobotController
{
    /// <summary>
    /// MainPage shows controller and sets up robot interface
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            this.InitializeComponent();

            // disable buttons until we're connected to the robot
            enableButtons(false);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Check if our robot connection is setup 
            if (!App.setupComplete)
            {
                var action = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, new Windows.UI.Core.DispatchedHandler(() =>
                {
                    this.Frame.Navigate(typeof(ArduinoConnectionPage));
                }
                ));

            }
            else
            {
                // Create robot control interface
                App.control = new RobotControl();
                enableButtons(true);
            }
        }

        private void button_forward_Click(object sender, RoutedEventArgs e)
        {
            App.control.forward(255, 255);
        }

        private void button_stop_Click(object sender, RoutedEventArgs e)
        {
            App.control.stop();
        }

        private void button_left_Click(object sender, RoutedEventArgs e)
        {
            App.control.left(127, 127);
        }

        private void button_right_Click(object sender, RoutedEventArgs e)
        {
            App.control.right(127, 127);
        }

        private void button_backward_Click(object sender, RoutedEventArgs e)
        {
            App.control.backward(255, 255);
        }

        private void Reconnect_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ArduinoConnectionPage));
        }
        private void TankController_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(TankControlPage));
        }

        private void buttonA_Click(object sender, RoutedEventArgs e)
        {
            ((RobotControl)App.control).activateLaser();   
        }

        private void buttonB_Click(object sender, RoutedEventArgs e)
        {
            // add cool functionality here! 
        }

        /// <summary>
        /// Enables or disables buttons
        /// </summary>
        /// <param name="enable"></param>
        private void enableButtons(bool enable)
        {
            buttonUp.IsEnabled = enable;
            buttonDown.IsEnabled = enable;
            buttonLeft.IsEnabled = enable;
            buttonRight.IsEnabled = enable;
            buttonStop.IsEnabled = enable;
        }
    }
}
