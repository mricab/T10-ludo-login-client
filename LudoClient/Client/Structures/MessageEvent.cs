using System;
namespace client
{
    public class MessageEvent
    {
        public MessageData data;

        public MessageEvent(object source, MessageData data)
        {
            this.data = data;
        }
    }
}
