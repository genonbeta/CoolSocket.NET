using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CoolSocket
{
    public abstract class Server
    {
        public const string HeaderSeperator = "\nHEADER_END\n";
        public const string HeaderLength = "length";

        protected Thread ServerThread = null;
        private bool RestartRequested = false;

        protected internal List<ActiveConnection> ActiveConnections { get; set; }
        protected internal SelfTcpListener TcpListener { get; set; }

        public Server() : this(0)
        {
            
        }

        public Server(UInt16 port) : this(IPAddress.Parse("0.0.0.0"), port)
        {

        }

        public Server(IPAddress ipAddress, [Range(IPEndPoint.MinPort, System.Net.IPEndPoint.MaxPort)] int port)
        {
            this.TcpListener = new SelfTcpListener(ipAddress, port);
        }

        protected abstract void OnConnected(ActiveConnection connection);

        public int GetLocalPort()
        {
            return ((IPEndPoint)this.TcpListener.LocalEndpoint).Port;
        }

        public bool IsRunning()
        {
            return this.TcpListener.Active;
        }

        public bool Restart()
        {
            if (!IsRunning())
                return false;

            RestartRequested = true;
            Stop();

            return true;
        }
        /// <summary>
        /// Starts the server if not already running. Restarts it if it is.
        /// </summary>
        public void Start()
        {
            if (!Restart())
            {
                ServerThread = new Thread(this.ServerMethod);
                ServerThread.Start();
            }
        }

        public void Stop()
        {
            this.TcpListener.Stop();
        }
        
        private void ServerMethod(object obj)
        {
            Console.WriteLine("Server started");

            TcpListener.Start();
            RestartRequested = false;

            while (IsRunning())
                RespondToRequest(TcpListener.AcceptTcpClient());

            if (RestartRequested)
                ServerMethod(obj);
        }

        protected void RespondToRequest(TcpClient client)
        {

        }
    }

    public class SelfTcpListener : TcpListener
    {
        public SelfTcpListener(IPEndPoint localEP) : base(localEP)
        {
        }

        public SelfTcpListener(IPAddress localaddr, int port) : base(localaddr, port)
        {
        }

        public new bool Active
        {
            get { return base.Active; }
        }
    }

    public class ActiveConnection
    {

    }

    public class Client
    {
        public static Client Connect()
        {
            return new Client();
        }
    }

    namespace Test
    {
        [TestClass]
        public class Main
        {
            [TestMethod]
            public void RunServer()
            {
                Assert.AreEqual(true, true);
            }
        }
    }
}
