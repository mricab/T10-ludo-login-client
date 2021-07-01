using System;
namespace GameClient
{
    public interface IGClient
    {
        void OnGConnected(GConnectedEvent e);
        void OnGDisconnected(GDisconnectedEvent e);
    }
}
