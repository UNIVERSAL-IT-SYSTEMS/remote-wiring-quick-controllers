using System;
using Windows.Storage.Streams;
using Windows.Data.Json;

namespace UWPRobotController.Control
{

    class SocketServerControl : IControl
    {
        DataWriter commandWriter;

        public void setTransport(DataWriter transport)
        {
            this.commandWriter = transport;
        }

        public SocketServerControl()
        {
            this.commandWriter = App.commandWriter;
        }

        public async void forward(byte a, byte b)
        {
            commandWriter.WriteString(createJsonCommand("forward", a, b));
            await commandWriter.StoreAsync();
        }

        public async void backward(byte a, byte b)
        {
            commandWriter.WriteString(createJsonCommand("backward", a, b));
            await commandWriter.StoreAsync();
        }

        public async void left(byte a, byte b)
        {
            commandWriter.WriteString(createJsonCommand("left", a, b));
            await commandWriter.StoreAsync();
        }

        public async void right(byte a, byte b)
        {
            commandWriter.WriteString(createJsonCommand("right", a, b));
            await commandWriter.StoreAsync();
        }

        public async void stop()
        {
            commandWriter.WriteString(createJsonCommand("stop", 0, 0));
            await commandWriter.StoreAsync();
        }

        private string createJsonCommand(string command, byte a, byte b)
        {
            JsonObject cmd = new JsonObject();
            cmd["command"] = JsonValue.CreateStringValue(command);
            cmd["a"] = JsonValue.CreateNumberValue(a);
            cmd["b"] = JsonValue.CreateNumberValue(b);
            return cmd.Stringify();
        }
    }
}
