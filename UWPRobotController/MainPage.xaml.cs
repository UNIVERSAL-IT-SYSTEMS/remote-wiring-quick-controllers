using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Proximity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using UWPRobotController.Control;
using Windows.Data.Json;
using Windows.System.Profile;

namespace UWPRobotController
{
    /// <summary>
    /// MainPage shows controller 
    /// </summary>
    public sealed partial class MainPage : Page
    {

        IControl control;
        private bool isRobot;

        public MainPage()
        {
            this.InitializeComponent();

            // disable buttons until were connected
            enableButtons(false);

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Check if our connections are setup 
            if (!App.setupComplete)
            {
                var action = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, new Windows.UI.Core.DispatchedHandler(() =>
                {
                    if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
                    {
                        this.Frame.Navigate(typeof(ArduinoConnectionPage));
                    }
                    else
                    {
                        this.Frame.Navigate(typeof(ControllerConnectionPage));
                    }
                }
                ));

            }
            else
            {
                // robot will always be on mobile device
                if (Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
                {
                    isRobot = true;
                    setupRobot();
                    enableButtons(true);
                }
                else
                {
                    isRobot = false;
                    setupController();
                    enableButtons(true);
                }
            }
       }


        /// <summary>
        /// Set up controller
        /// </summary>
        private async void setupController()
        {

            // Send initializing command
            if (App.commandWriter != null)
            {
                App.commandWriter.WriteString("{\"type\": \"controller\"}");
                await App.commandWriter.StoreAsync();
            }

            control = new SocketServerControl();

        }

        /// <summary>
        /// Sets up robot
        /// </summary>
        private async void setupRobot()
        {

            // setup message callbacks

            // initialize connection with server
            if (App.commandWriter != null)
            {
                App.commandWriter.WriteString("{\"type\": \"robot\"}");
                await App.commandWriter.StoreAsync();

                App.commandSock.MessageReceived += CommandReceived;
            }


            control = new RobotControl();
        }


        /*
        private async void connectCommandSocket(bool isRobot)
        {
            // set up video recv sock and command sock 
            bool connecting = true;

            try
            {
                if (App.commandSock == null)
                {
                    Uri server;

                    // Controller IP should be localhost e.g. where the socket server is running, robot should connect to IP of that machine
                    if (isRobot)
                        server = new Uri("ws://192.168.1.106:8080"); // TODO: allow user to input IP
                    else
                        server = new Uri("ws://localhost:8080");

                    // Create WebSocket for commands and set message type
                    App.commandSock = new MessageWebSocket();
                    App.commandSock.Control.MessageType = SocketMessageType.Utf8;

                    if (isRobot)
                        App.commandSock.MessageReceived += CommandReceived;

                    // connect to server
                    await App.commandSock.ConnectAsync(server);

                    // Set up datawriter for socket
                    App.commandWriter = new DataWriter(App.commandSock.OutputStream);
                }

                connecting = false;

                // Initialize connection
                // tell the server if we are the robot or the controller 
                if (isRobot)
                    App.commandWriter.WriteString("{\"type\": \"robot\"}");
                else
                    App.commandWriter.WriteString("{\"type\": \"controller\"}");

                //send the initialization message for commands
                await App.commandWriter.StoreAsync();

            }
            catch
            {

            }
        }

    */

        /// <summary>
        /// Event callback for socket message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void CommandReceived(MessageWebSocket sender, MessageWebSocketMessageReceivedEventArgs args)
        {
            try
            {
                // Read data from message
                using (DataReader reader = args.GetDataReader())
                {
                    reader.UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8;
                    string read = reader.ReadString(reader.UnconsumedBufferLength);

                    // Get JSON from value
                    JsonValue obj = JsonValue.Parse(read);
                    byte a = (byte)obj.GetObject().GetNamedNumber("a");
                    byte b = (byte)obj.GetObject().GetNamedNumber("b");

                    // Make call to the main thread because windows
                    var thing = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, new Windows.UI.Core.DispatchedHandler(() =>
                    {
                        switch (obj.GetObject().GetNamedString("command"))
                        {
                            case "left":
                                control.left(a, b);
                                break;
                            case "right":
                                control.right(a, b);
                                break;
                            case "backward":
                                control.backward(a, b);
                                break;
                            case "forward":
                                control.forward(a, b);
                                break;
                            case "stop":
                                control.stop();
                                break;

                        }
                    }));
                }
            }
            catch
            {

            }

        }

        private void button_forward_Click(object sender, RoutedEventArgs e)
        {
            control.forward(255, 255);
        }

        private void button_stop_Click(object sender, RoutedEventArgs e)
        {
            control.stop();
        }

        private void button_left_Click(object sender, RoutedEventArgs e)
        {
            control.right(255, 255);
        }

        private void button_right_Click(object sender, RoutedEventArgs e)
        {
            control.left(255, 255);
        }

        private void button_backward_Click(object sender, RoutedEventArgs e)
        {
            control.backward(255, 255);
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ArduinoConnectionPage));
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
