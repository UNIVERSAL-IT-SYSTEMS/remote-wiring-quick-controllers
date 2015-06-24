using System;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.WiFiDirect;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace UWPRobotController
{
    class RobotDirect
    {
        bool isWiFiDirectWorking = false;


        /// <summary>
        /// Setup direct websocket, currently only supports server connection
        /// </summary>
        /// <param name="server"></param>
        public static async Task setupDirectSocket(Uri server)
        {

            if (App.commandSock == null)
            {
                // Create WebSocket for commands and set message type
                App.commandSock = new MessageWebSocket();
                App.commandSock.Control.MessageType = SocketMessageType.Utf8;

                //App.commandSock.MessageReceived += CommandReceived;

                // connect to server
                await App.commandSock.ConnectAsync(server);

                // Set up datawriter for socket
                App.commandWriter = new DataWriter(App.commandSock.OutputStream);
            }

        }



        // WiFi Direct functions

        public async Task<DeviceInformationCollection> enumerateRobots()
        {
            // Get all wifi direct devices advertising
            String deviceSelector = WiFiDirectDevice.GetDeviceSelector(WiFiDirectDeviceSelectorType.AssociationEndpoint);

            DeviceInformationCollection devInfoCollection = await DeviceInformation.FindAllAsync(deviceSelector);
            return devInfoCollection;
        }

        /// <summary>
        /// Advertise if the app is the robot 
        /// </summary>
        public void startAdvertising()
        {
            // create publisher
            var publisher = new WiFiDirectAdvertisementPublisher();

            // turn on listen state
            publisher.Advertisement.ListenStateDiscoverability = WiFiDirectAdvertisementListenStateDiscoverability.Normal;

            var listener = new WiFiDirectConnectionListener();
            listener.ConnectionRequested += OnConnectionRequested;
            publisher.Start();
        }

        private async void OnConnectionRequested(WiFiDirectConnectionListener sender, WiFiDirectConnectionRequestedEventArgs args)
        {
            var connectionRequest = args.GetConnectionRequest();

            // prompt for connection

            WiFiDirectDevice wfdDevice = await WiFiDirectDevice.FromIdAsync(connectionRequest.DeviceInformation.Id);

            var endpointPairs = wfdDevice.GetConnectionEndpointPairs();

            App.commandSock = new Windows.Networking.Sockets.MessageWebSocket();

        }

    }
}
