using System;
using System.IO;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using System.Xml.Serialization;
using TcpProtocol;

namespace client
{
    public class KeepAlive
    {

        // Properties
        private static bool Keep;
        private Thread KeeperThread;

        // Received properties
        private TcpClient Client;
        private Type AppPackageType;

        // Events properties
        private List<IKeepAlive> Observers;

        // Methods
        public KeepAlive(TcpClient Client, Type AppPackageType)
        {
            this.Client = Client;
            this.AppPackageType = AppPackageType;
            this.Observers = new List<IKeepAlive>();
        }

        public void Start()
        {
            Keep = true;
            KeeperThread = new Thread(new ThreadStart(Process));
            KeeperThread.Start();
        }

        public void Stop()
        {
            Keep = false;
        }

        private void Process()
        {
            Console.WriteLine("(Keeper)\tKeeper up.");

            double delay = 2000; // 2s
            DateTime lastTime = System.DateTime.Now;
            DateTime now;

            //byte[] bytes; // Incoming data buffer.
            NetworkStream networkStream = Client.GetStream();
            XmlSerializer serializer = new XmlSerializer(typeof(Package), new Type[] { AppPackageType });

            while (Keep)
            {
                now = System.DateTime.Now;

                if ( now >= lastTime.AddMilliseconds(delay))
                {
                    try
                    {
                        // Sending message
                        serializer.Serialize(networkStream, Protocol.GetPackage("keep"));
                    }
                    catch (ArgumentException) // networkStream closed
                    {
                        Console.WriteLine("(Keeper)\tServer timed out!");
                        OnServerDown();
                        break;
                    }
                    catch (IOException) // Timeout
                    {
                        Console.WriteLine("(Keeper)\tServer timed out!");
                        OnServerDown();
                        break;
                    }
                    catch (SocketException)
                    {
                        Console.WriteLine("(Keeper)\tConection broken!");
                        break;
                    }

                    lastTime = now;
                }

            }
            Console.WriteLine("(Keeper)\tKeeper stopped.");
            OnKeepAliveDown();
        }

        // Interface Methods
        public void RegisterObserver(IKeepAlive observer)
        {
            Observers.Add(observer);
        }

        public void RemoveObserver(IKeepAlive observer)
        {
            Observers.Remove(observer);
        }

        // Dispachers
        public void OnServerDown()
        {
            ServerDownEvent e = new ServerDownEvent(this);
            foreach (IKeepAlive observer in Observers)
            {
                observer.OnServerDown(e);
            }
        }

        public void OnKeepAliveDown()
        {
            KeepAliveDownEvent e = new KeepAliveDownEvent(this);
            foreach (IKeepAlive observer in Observers)
            {
                observer.OnKeepAliveDown(e);
            }
        }

    }
}

