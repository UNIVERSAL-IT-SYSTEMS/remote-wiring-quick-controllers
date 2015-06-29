using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPRobotController.Interfaces
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DpadControlPage : Page
    {
        public DpadControlPage()
        {
            this.InitializeComponent();
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

        

        private void buttonA_Click(object sender, RoutedEventArgs e)
        {
            ((RomeoControl)App.control).activateLaser();   
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

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
