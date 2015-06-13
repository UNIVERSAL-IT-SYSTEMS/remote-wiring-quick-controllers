using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UWPRobotController
{
    public sealed partial class ControllerConnectionPage : Page
    {

        public ControllerConnectionPage()
        {
            this.InitializeComponent();
            Collection<String> options = new Collection<string>();
            options.Add("Socket");
            options.Add("WiFi Direct");

            comboBox.ItemsSource = options;
            Refresh.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            ipAddressTextBox.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            // enumerate WiFi Direct Devices
        }
        
        private void Skip_Click(object sender, RoutedEventArgs e)
        {
            App.setupComplete = true;
            Frame.Navigate(typeof(MainPage));
        }

        private async void Reconnect_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox.SelectedValue.Equals("WiFi Direct"))
            {
                // TODO: connect to wifi direct device
            }
            if (comboBox.SelectedValue.Equals("Socket"))
            {
                try
                {
                    // create socket connection
                    await RobotDirect.setupDirectSocket(new Uri(ipAddressTextBox.Text));

                    // return to controller were done now
                    App.setupComplete = true;
                    Frame.Navigate(typeof(MainPage));
                }
                catch
                { }

            }
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox.SelectedValue.Equals("Socket"))
            {
                Refresh.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                ipAddressTextBox.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            if (comboBox.SelectedValue.Equals("WiFi Direct"))
            {
                Refresh.Visibility = Windows.UI.Xaml.Visibility.Visible;
                ipAddressTextBox.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

        
    }
}
