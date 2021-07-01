using System;
using System.Collections.Generic;
using client;
using LudoProtocol;

namespace GameClient
{
    public class GClient : IClient
    {
        // Properties
        private static Client TClient;

        // Events properties
        private List<IGClient> Observers;

        // Constructor
        public GClient(int ServerPort)
        {
            TClient = new Client(ServerPort, typeof(LPackage));
            TClient.RegisterObserver(this);
            Observers = new List<IGClient>();
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

        public void OnConnected(ConnectedEvent e)
        {
            Console.WriteLine("(GClient)\tConnection {0} assigned by server.", e.data.connectionId);
            OnGConnected();
        }

        public void OnDisconnected(DisconnectedEvent e)
        {
            Console.WriteLine("(GClient)\tConnection lost.");
            OnGDisconnected();
        }

        // GClient dispatchers
        public void OnGConnected()
        {
            GConnectedEvent e = new GConnectedEvent(this);
            foreach (IGClient observer in Observers)
            {
                observer.OnGConnected(e);
            }
        }

        public void OnGDisconnected()
        {
            GDisconnectedEvent e = new GDisconnectedEvent(this);
            foreach (IGClient observer in Observers)
            {
                observer.OnGDisconnected(e);
            }
        }

        // Interface Methods
        public void RegisterObserver(IGClient observer)
        {
            Observers.Add(observer);
        }

        public void RemoveObserver(IGClient observer)
        {
            Observers.Remove(observer);
        }
    }
}
