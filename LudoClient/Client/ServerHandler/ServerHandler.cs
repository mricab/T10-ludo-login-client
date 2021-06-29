using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using System.Xml.Serialization;
using TcpProtocol;

namespace client
{
    public class ServerHandler
    {
        // Properties
        private static bool Handle;
        private Thread ClientThread;
        private bool quit = false;

        // Received Properties
        private TcpClient Client;
        private Type AppPackageType;

        // Events properties
        private List<IServerHandler> Observers;

        // Methods
        public ServerHandler(TcpClient Client, Type AppPackageType)
        {
            this.Client = Client;
            this.AppPackageType = AppPackageType;
            this.Observers = new List<IServerHandler>();
        }

        public void Start()
        {
            Handle = true;
            ClientThread = new Thread(new ThreadStart(Receive));
            ClientThread.Start();
        }

        public void Stop()
        {
            Handle = false;
        }

        // Handler Actions
        public void Send(Package package)
        {
            NetworkStream networkStream = Client.GetStream();
            XmlSerializer serializer = new XmlSerializer(typeof(Package), new Type[] { AppPackageType });
            serializer.Serialize(networkStream, package);
        }

        private void Receive()
        {
            Console.WriteLine("(Handler)\tHandler up.");
            byte[] bytes; // Incoming data buffer.
            NetworkStream networkStream = Client.GetStream();
            XmlSerializer deserializer = new XmlSerializer(typeof(Package), new Type[] { AppPackageType });

            while (Handle)
            {
                try
                {
                    bytes = new byte[Client.ReceiveBufferSize]; // 8192 Bytes
                    int BytesRead = networkStream.Read(bytes, 0, (int)Client.ReceiveBufferSize); // Receiving response

                    MemoryStream memoryStream = new MemoryStream(bytes);
                    Package package = (Package)deserializer.Deserialize(memoryStream);
                    if (package.type == Protocol.TypeCode("message")) 
                    {
                        Console.WriteLine("(Handler)\tMessage package received ({0} Bytes).", BytesRead);
                        OnServerMessageReceived(package.obj);
                    }
                    else
                    {
                        //Console.WriteLine("(Handler)\tKeep package received ({0} Bytes).", BytesRead);
                    }
                }
                //catch (InvalidOperationException) // System.Xml.XmlException: Root element is missing,
                catch (System.Xml.XmlException)
                {
                    Console.WriteLine("(Handler)\tServer stopped transfering!");
                    break;
                }
                catch (IOException) // Timeout
                {
                    Console.WriteLine("(Handler)\tServer timed out!");
                    break;
                }
                catch (SocketException)
                {
                    Console.WriteLine("(Handler)\tConection broken!");
                    break;
                }
                Thread.Sleep(200); // 0.2s
            }

            networkStream.Close();
            Client.Close();
            Console.WriteLine("(Handler)\tHandler stopped.");
        }

        // Interface Methods
        public void RegisterObserver(IServerHandler observer)
        {
            Observers.Add(observer);
        }

        public void RemoveObserver(IServerHandler observer)
        {
            Observers.Remove(observer);
        }

        // Dispachers
        public void OnUserQuitted()
        {
            //UserQuitEvent e = new UserQuitEvent(this);
            //foreach (IServerHandler observer in Observers)
            //{
            //    observer.OnUserQuitted(e);
            //}
        }

        public void OnServerMessageReceived(Object obj)
        {
            ServerMessageData data = new ServerMessageData(obj);
            ServerMessageEvent e = new ServerMessageEvent(this, data);
            foreach (IServerHandler observer in Observers)
            {
                observer.OnServerMessageReceived(e);
            }
        }

    }


}
