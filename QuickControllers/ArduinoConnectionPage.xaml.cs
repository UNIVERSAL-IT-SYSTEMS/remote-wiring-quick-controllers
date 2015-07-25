using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using QuickControllers.Comm;
using Microsoft.Maker.Firmata;
using Microsoft.Maker.Serial;
using Windows.Devices.Enumeration;
using Microsoft.Maker.RemoteWiring;
using System.Collections.ObjectModel;

namespace QuickControllers
{

    public sealed partial class ArduinoConnectionPage : Page
    {
        private ObservableCollection<Connection> _connections = null;


        public ArduinoConnectionPage()
        {
            this.InitializeComponent();

            Collection<String> options = new Collection<string>();
            options.Add("Bluetooth");
            options.Add("Bluetooth LE");

            comboBox.ItemsSource = options;
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private void RefreshDeviceList()
        {
            if (comboBox.SelectedValue != null)
            {
                if (comboBox.SelectedValue.Equals("Bluetooth LE"))
                {
                    DfRobotBleSerial.listAvailableDevicesAsync().AsTask<DeviceInformationCollection>().ContinueWith(listTask =>
                    {
                        var action = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, new Windows.UI.Core.DispatchedHandler(() =>
                        {
                            _connections = new ObservableCollection<Connection>();
                            foreach (DeviceInformation device in listTask.Result)
                            {
                                _connections.Add(new Connection(device.Name, device));
                            }
                            connectList.ItemsSource = _connections;

                            // autoconnect if only 1 device is paired
                            if (_connections.Count == 1)
                            {
                                connectList.SelectedItem = _connections[0];
                                Reconnect_Click(null, null);
                            }
                        }));
                    });
                }
                else if (comboBox.SelectedValue.Equals("Bluetooth"))
                {
                    BluetoothSerial.listAvailableDevicesAsync().AsTask<DeviceInformationCollection>().ContinueWith(listTask =>
                    {
                        var action = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, new Windows.UI.Core.DispatchedHandler(() =>
                        {
                            _connections = new ObservableCollection<Connection>();
                            foreach (DeviceInformation device in listTask.Result)
                            {
                                _connections.Add(new Connection(device.Name, device));
                            }
                            connectList.ItemsSource = _connections;

                            // autoconnect if only 1 device is paired
                            if (_connections.Count == 1)
                            {
                                connectList.SelectedItem = _connections[0];
                                Reconnect_Click(null, null);
                            }
                        }));
                    });
                }

                
            }
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
                if (comboBox.SelectedValue.Equals("Bluetooth"))
                {
                    App.bluetooth = new BluetoothSerial(device);
                    ((BluetoothSerial)App.bluetooth).ConnectionEstablished += Bluetooth_ConnectionEstablished;
                    ((BluetoothSerial)App.bluetooth).ConnectionFailed += Bluetooth_ConnectionFailed;
                    ((BluetoothSerial)App.bluetooth).ConnectionLost += Bluetooth_ConnectionLost;
                }
                else if (comboBox.SelectedValue.Equals("Bluetooth LE"))
                {
                    App.bluetooth = new DfRobotBleSerial(device);
                    ((DfRobotBleSerial)App.bluetooth).ConnectionEstablished += Bluetooth_ConnectionEstablished;
                    ((DfRobotBleSerial)App.bluetooth).ConnectionFailed += Bluetooth_ConnectionFailed;
                    ((DfRobotBleSerial)App.bluetooth).ConnectionLost += Bluetooth_ConnectionLost;
                }

                App.bluetooth.begin(115200, SerialConfig.SERIAL_8N1);

                App.firmata = new UwpFirmata();
                App.firmata.begin(App.bluetooth);

                App.arduino = new RemoteDevice(App.firmata);
            }
        }

        private void Bluetooth_ConnectionLost(string message)
        {
            if (comboBox.SelectedValue.Equals("Bluetooth"))
            {
                ((BluetoothSerial)App.bluetooth).ConnectionEstablished -= Bluetooth_ConnectionEstablished;
                ((BluetoothSerial)App.bluetooth).ConnectionFailed -= Bluetooth_ConnectionFailed;
                ((BluetoothSerial)App.bluetooth).ConnectionLost -= Bluetooth_ConnectionLost;
            }
            else if (comboBox.SelectedValue.Equals("Bluetooth LE"))
            {
                ((DfRobotBleSerial)App.bluetooth).ConnectionEstablished -= Bluetooth_ConnectionEstablished;
                ((DfRobotBleSerial)App.bluetooth).ConnectionFailed -= Bluetooth_ConnectionFailed;
                ((DfRobotBleSerial)App.bluetooth).ConnectionLost -= Bluetooth_ConnectionLost;
            }

            var action = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, new Windows.UI.Core.DispatchedHandler(() =>
            {
                setButtonsEnabled(true);
                mTextBlock.Text = "Connection lost...";
            }));
        }

        private void Bluetooth_ConnectionFailed(String error)
        {

            if (comboBox.SelectedValue.Equals("Bluetooth"))
            {
                ((BluetoothSerial)App.bluetooth).ConnectionEstablished -= Bluetooth_ConnectionEstablished;
                ((BluetoothSerial)App.bluetooth).ConnectionFailed -= Bluetooth_ConnectionFailed;
                ((BluetoothSerial)App.bluetooth).ConnectionLost -= Bluetooth_ConnectionLost;
            }
            else if (comboBox.SelectedValue.Equals("Bluetooth LE"))
            {
                ((DfRobotBleSerial)App.bluetooth).ConnectionEstablished -= Bluetooth_ConnectionEstablished;
                ((DfRobotBleSerial)App.bluetooth).ConnectionFailed -= Bluetooth_ConnectionFailed;
                ((DfRobotBleSerial)App.bluetooth).ConnectionLost -= Bluetooth_ConnectionLost;
            }

            var action = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, new Windows.UI.Core.DispatchedHandler(() =>
            {
                setButtonsEnabled(true);
                mTextBlock.Text = "Connection failed.";
            }));
        }

        private void Bluetooth_ConnectionEstablished()
        {

            if (comboBox.SelectedValue.Equals("Bluetooth"))
            {
                ((BluetoothSerial)App.bluetooth).ConnectionEstablished -= Bluetooth_ConnectionEstablished;
            }
            else if (comboBox.SelectedValue.Equals("Bluetooth LE"))
            {
                ((DfRobotBleSerial)App.bluetooth).ConnectionEstablished -= Bluetooth_ConnectionEstablished;
            }

            App.setupComplete = true;
            var action = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, new Windows.UI.Core.DispatchedHandler(() =>
            {
                Frame.Navigate(typeof(MainPage));
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

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshDeviceList();
        }
    }
}