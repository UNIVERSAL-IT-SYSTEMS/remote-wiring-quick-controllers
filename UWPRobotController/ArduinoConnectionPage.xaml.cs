using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using UWPRobotController.Comm;
using Microsoft.Maker.Serial;
using Windows.Devices.Enumeration;
using Microsoft.Maker.RemoteWiring;
using System.Collections.ObjectModel;

namespace UWPRobotController 
{

    public sealed partial class ArduinoConnectionPage : Page
    {
        private ObservableCollection<Connection> _connections = null;


        public ArduinoConnectionPage()
        {
            this.InitializeComponent();

            Collection<String> options = new Collection<string>();
            options.Add("Bluetooth RFCOMM");
            options.Add("Bluetooth LE");
            //options.Add("Usb Serial");

            comboBox.ItemsSource = options;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private void RefreshDeviceList()
        {
            //invoke the listAvailableDevicesAsync method of BluetoothSerial. Since it is Async, we will wrap it in a Task and add a llambda to execute when finished
            BluetoothSerial.listAvailableDevicesAsync().AsTask<DeviceInformationCollection>().ContinueWith(listTask =>
            {
                //store the result and populate the device list on the UI thread
                var action = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, new Windows.UI.Core.DispatchedHandler(() =>
                {
                    _connections = new ObservableCollection<Connection>();
                    foreach (DeviceInformation device in listTask.Result)
                    {
                        _connections.Add(new Connection(device.Name, device));
                    }
                    connectList.ItemsSource = _connections;
                }));
            });
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshDeviceList();
        }

        private void Reconnect_Click(object sender, RoutedEventArgs e)
        {
            if (connectList.SelectedItem != null)
            {
                setButtonsEnabled(false);
                mTextBlock.Text = "Connecting...";

                var selectedConnection = connectList.SelectedItem as Connection;
                var device = selectedConnection.Source as DeviceInformation;

                //construct the bluetooth serial object with the specified device
                App.bluetooth = new BluetoothSerial(device);

                App.bluetooth.ConnectionEstablished += Bluetooth_ConnectionEstablished;
                App.bluetooth.ConnectionFailed += Bluetooth_ConnectionFailed;
                App.arduino = new RemoteDevice(App.bluetooth);
                App.bluetooth.begin(115200, 0);
            }
        }

        private void Bluetooth_ConnectionFailed(String error)
        {
            App.bluetooth.ConnectionEstablished -= Bluetooth_ConnectionEstablished;
            App.bluetooth.ConnectionFailed -= Bluetooth_ConnectionFailed;
            var action = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, new Windows.UI.Core.DispatchedHandler(() =>
            {
                setButtonsEnabled(true);
                mTextBlock.Text = "Connection failed.";
            }));
        }

        private void Bluetooth_ConnectionEstablished()
        {
            App.bluetooth.ConnectionEstablished -= Bluetooth_ConnectionEstablished;
            App.bluetooth.ConnectionFailed -= Bluetooth_ConnectionFailed;
            var action = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, new Windows.UI.Core.DispatchedHandler(() =>
            {
                Frame.Navigate(typeof(ControllerConnectionPage));
            }));
        }



        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void setButtonsEnabled(bool enabled)
        {
            var action = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, new Windows.UI.Core.DispatchedHandler(() =>
            {
                Reconnect.IsEnabled = enabled;
                Refresh.IsEnabled = enabled;
            }));
        }



    }
}