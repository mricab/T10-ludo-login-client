using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using TcpProtocol;

namespace client
{
    public class Client : IServerHandler, IKeepAlive
    {
        // Properties
        private static TcpClient ClientSocket;
        private static ServerHandler Handler;
        private static KeepAlive Keeper;
        private static bool Disconnected = true;

        // Received Properties
        private int ServerPort;
        private string ServerIP = "localhost";
        private int ReconnectDelay = 4000; // 4s
        private Type ApplicationPackageType;

        // Events properties
        private List<IClient> Observers;

        // Constructor
        public Client(int ServerPort, Type PackageType)
        {
            this.ServerPort = ServerPort;
            this.ApplicationPackageType = PackageType;
            Observers = new List<IClient>();
        }

        // Main Functions
        public void Start()
        {
            try
            {
                if (AttemptConnection())
                {
                    InitilizeClient();
                    Console.WriteLine("(Client)\tClient connected.");
                    Keeper.Start();
                    Handler.Start();
                }
                else
                {
                    throw new Exception("Server not available.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("(Client)\tClient couldn't be started.");
                Console.WriteLine(e.ToString());
            }

        }

        public void Stop()
        {
            Keeper.Stop();
            Handler.Stop();
        }

        // Client actions
        public void Send(Object obj)
        {
            Package package = Protocol.GetPackage("message", obj);
            Handler.Send(package);
        }


        // Helper Functions
        private void InitilizeClient()
        {
            // Initializing Main Processes
            Handler = new ServerHandler(ClientSocket, ApplicationPackageType);
            Keeper = new KeepAlive(ClientSocket, ApplicationPackageType);
            // Events Subscriptions
            Handler.RegisterObserver(this);
            Keeper.RegisterObserver(this);
        }

        private bool AttemptConnection()
        {
            uint Attempt = 1;

            while (Disconnected && Attempt <= 100)
            {
                // Initializing ClientSocket
                ClientSocket = new TcpClient();
                ClientSocket.ReceiveTimeout = 5000; // 5s
                ClientSocket.SendTimeout = 5000; // 5s

                if (ClientSocket != null)
                {
                    try
                    {
                        Console.WriteLine("(Client)\tConnection attemp#{0}.", Attempt);
                        ClientSocket.Connect(ServerIP, ServerPort); //Raises error if fails
                        if (ClientSocket.Connected)
                        {
                            Disconnected = false;
                            return true;
                        }
                        
                    }
                    catch (SocketException)
                    {
                        Console.WriteLine("(Client)\tAttemp#{0} failed.", Attempt);
                        ClientSocket.Close();
                    }
                }
                else
                {
                    throw new Exception("ClientSocket is not initialized.");
                }

                ++Attempt;
                Thread.Sleep(ReconnectDelay);
            }
            return false;  
        }

        // IServerHandler
        public void OnServerMessageReceived(ServerMessageEvent e)
        {
            Console.WriteLine("(Client)\tMessage Received");
            OnMessageReceived(e.Data.obj);
        }

        public void OnUserQuitted(UserQuitEvent e)
        {
            Stop();
        }

        // IKeepAlive
        public void OnServerDown(ServerDownEvent e)
        {
            Console.WriteLine("(Client)\tServer cant be reached.");
            Stop();
            Disconnected = true;
        }

        public void OnKeepAliveDown(KeepAliveDownEvent e)
        {
            if(Disconnected) Start();
        }

        // Interface Methods
        public void RegisterObserver(IClient observer)
        {
            Observers.Add(observer);
        }

        public void RemoveObserver(IClient observer)
        {
            Observers.Remove(observer);
        }

        // IClient Dispatchers
        public void OnMessageReceived(Object obj)
        {
            MessageData message = new MessageData(obj);
            MessageEvent e = new MessageEvent(this, message);
            foreach (IClient observer in Observers)
            {
                observer.OnMessageReceived(e);
            }
        }

    }
}
