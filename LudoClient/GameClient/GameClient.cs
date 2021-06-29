using System;
using client;
using LudoProtocol;

namespace GameClient
{
    public class GClient : IClient
    {
        // Properties
        private static Client TClient;

        // Constructor
        public GClient(int ServerPort)
        {
            TClient = new Client(ServerPort, typeof(LPackage));
            TClient.RegisterObserver(this);
        }

        // Main Methods
        public void Start()
        {
            TClient.Start();
        }

        public void Send(string actionName, string[] contents = null)
        {
            LPackage lPackage = new LPackage();
            switch (actionName)
            {
                case "login":       lPackage = LProtocol.GetPackage(actionName, contents); break;
                case "logout":      lPackage = LProtocol.GetPackage(actionName); break;
                default:            break;
            }
            if(lPackage != null) { TClient.Send(lPackage); }
        }

        // IClient
        public void OnMessageReceived(MessageEvent e)
        {
            LPackage lPackage = (LPackage)e.data.obj;
            Console.WriteLine("(GClient)\tNew message [{0}].", lPackage.ToString());
        }
    }
}
