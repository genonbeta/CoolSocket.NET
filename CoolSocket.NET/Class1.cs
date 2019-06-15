using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace CoolSocket.NET
{
    abstract public class Server
    {
        public const string HeaderSeperator = "\nHEADER_END\n";
        public const string HeaderLength = "length";

        protected internal List<string> ActiveConnections { get; set; }
        protected internal TcpListener TcpServer { get; set; }

        public Server() : this(0)
        {
            
        }

        public Server(Int16 port) : this (IPAddress.Parse("0.0.0.0"), port)
        {
            
        }

        public Server(IPAddress ipAddress, short port)
        {
            this.TcpServer = new TcpListener(ipAddress, port);
        }
    }
}
