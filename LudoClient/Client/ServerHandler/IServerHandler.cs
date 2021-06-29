using System;
namespace client
{
    public interface IServerHandler
    {
        void OnServerMessageReceived(ServerMessageEvent e);
        void OnUserQuitted(UserQuitEvent e);
    }
}
