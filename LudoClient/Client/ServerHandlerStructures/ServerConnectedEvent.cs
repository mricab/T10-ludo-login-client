using System;
namespace client
{
    public class ServerConnectedEvent
    {
        public ServerConnectedData data;

        public ServerConnectedEvent(ServerConnectedData data)
        {
            this.data = data;
        }
    }
}
