using System;
using System.Collections.ObjectModel;
using QuickControllers.Interfaces;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace QuickControllers 
{
    /// <summary>
    /// MainPage shows controller and sets up robot interface
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            this.InitializeComponent();
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
                App.control = new RomeoControl();
            }

            Collection<String> controllers = new Collection<string>();
            controllers.Add("Dpad Controller");
            controllers.Add("Tank Controller");
            // Add new controllers here

            listBox.ItemsSource = controllers;
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var action = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, new Windows.UI.Core.DispatchedHandler(() =>
            {
                switch (listBox.SelectedValue.ToString())
                {
                    case "Dpad Controller":
                        this.Frame.Navigate(typeof(DpadControlPage));
                        break;
                    case "Tank Controller":
                        this.Frame.Navigate(typeof(TankControlPage));
                        break;
                    // add case for new controllers
                }
            }
            ));
        }

        private void Reconnect_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ArduinoConnectionPage));
        }
    }
}
