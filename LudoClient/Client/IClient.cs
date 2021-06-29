using System;
namespace client
{
    public interface IClient
    {
        void OnMessageReceived(MessageEvent e);
    }
}
