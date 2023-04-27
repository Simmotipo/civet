using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Web;

namespace civet
{
    class UdpMan
    {

        static List<UdpServer> servers = new List<UdpServer>();
        static List<string> ports = new List<string>();
        public static string Go(string cmd)
        {
            string port = cmd.Split(' ')[0]; //doubles up as address for SEND command
            string keyword = cmd.Split(' ')[1].ToLower();

            switch (keyword)
            {
                case "start":
                    if (ports.Contains(port)) { UdpErrors.ServerAlreadyExists("HTTP", port); return ""; }
                    UdpServer s = new UdpServer();
                    s.port = Convert.ToInt32(port);
                    //if (cmd.Split(keyword).Length > 1) { s.goPoint = cmd.Split(keyword)[1].Substring(1); }
                    bool started = s.start();
                    Console.WriteLine(s.goPoint);
                    if (!started) UdpErrors.Generic(null, port);
                    else
                    {
                        servers.Add(s);
                        ports.Add(port);
                    }
                    break;
                case "stop":
                    if (!ports.Contains(port)) { UdpErrors.ServerDoesntExist("HTTP", port); return ""; }
                    for (int i = 0; i < ports.Count; i++)
                    {
                        if (ports[i] == port)
                        {
                            bool disposed = servers[i].dispose();
                            if (disposed)
                            {
                                servers[i].dispose();
                                servers.RemoveAt(i);
                                ports.RemoveAt(i);
                            }
                            else UdpErrors.ServerNotDisposed("HTTP", port);
                        }
                    }
                    break;
                case "send":
                    string msg = cmd.Split(keyword)[1];

                    UdpServer temp = new UdpServer();
                    temp.send(port, msg);

                    break;
                case "bufferstring":
                    if (!ports.Contains(port)) { UdpErrors.ServerDoesntExist("HTTP", port); return ""; }
                    for (int i = 0; i < ports.Count; i++)
                    {
                        if (ports[i] == port)
                        {
                            return servers[i].getLatestBufferAsString();
                        }
                    }
                    break;
                default:
                    UdpErrors.UnknownServerCommand(keyword);
                    break;
            }

            return "";
        }

    }

    

    class UdpServer
    {
        class UDPSocket
        {
            private Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            private const int bufSize = 8 * 1024;
            private State state = new State();
            private EndPoint epFrom = new IPEndPoint(IPAddress.Any, 0);
            private AsyncCallback recv = null;
            public string goPoint = "";

            public class State
            {
                public byte[] buffer = new byte[bufSize];
            }

            public void Server(string address, int port)
            {
                _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
                _socket.Bind(new IPEndPoint(IPAddress.Parse(address), port));
                Receive();
            }

            public void Client(string address, int port, bool oneTime = false)
            {
                _socket.Connect(IPAddress.Parse(address), port);
                if (!oneTime) Receive();
            }

            public void Send(string text)
            {
                byte[] data = Encoding.ASCII.GetBytes(text);
                _socket.BeginSend(data, 0, data.Length, SocketFlags.None, (ar) =>
                {
                    State so = (State)ar.AsyncState;
                    int bytes = _socket.EndSend(ar);
                    if (Program.debugMode) Console.WriteLine("SEND: {0}, {1}", bytes, text);
                }, state);
            }

            byte[] currentBuffer;

            private void Receive()
            {
                _socket.BeginReceiveFrom(state.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv = (ar) =>
                {
                    int bufferOffset = 0;
                    State so = (State)ar.AsyncState;
                    int bytes = _socket.EndReceiveFrom(ar, ref epFrom);
                    _socket.BeginReceiveFrom(so.buffer, bufferOffset, bufSize, SocketFlags.None, ref epFrom, recv, so);
                    if (Program.debugMode) Console.WriteLine("RECV: {0}: {1}, {2}", epFrom.ToString(), bytes, Encoding.ASCII.GetString(so.buffer, bufferOffset, bytes));
                    currentBuffer = new byte[bytes];
                    for (int i = bufferOffset; i < bufferOffset + bytes; i++)
                    {
                        currentBuffer[i] = so.buffer[bufferOffset + i];
                    }
                }, state);

                //if (goPoint != "" ) GoPointInterpreter();
            }

            public byte[] getCurrentBuffer()
            {
                return currentBuffer;
            }


            //void GoPointInterpreter()
            //{
            //    Console.WriteLine("goto " + goPoint);
            //    Program subProgram = new Program();
            //    Console.WriteLine(subProgram.Interpret("goto " + goPoint));
            //    /*
            //    while (index < lines.Length)
            //    {
            //        Interpret(lines[index]);
            //        index++;
            //    }*/
            //}
        }

        public int port = 27000;
        public string goPoint = "";
        public static bool runServer = true;
        static Thread serverThread;
        UDPSocket serverSocket;


        public bool dispose()
        {
            try
            {
                runServer = false;
                return true;
            }
            catch (Exception e) { UdpErrors.Generic(e,Convert.ToString(port)); return false; }
        }

        public bool start()
        {
            try
            {
                runServer = true;
                serverThread = new Thread(init);
                serverThread.Start();
                return true;
            }
            catch { return false; }
        }

        public void init()
        {
            try
            {
                serverSocket = new UDPSocket();
                serverSocket.Server("0.0.0.0", port);
            }
            catch (Exception e) { UdpErrors.Generic(e, Convert.ToString(port)); }
        }

        public void send(string target, string message)
        {
            try
            {
                UDPSocket c = new UDPSocket();

                c.Client(target.Split(':')[0], Convert.ToInt32(target.Split(':')[1]), true);
                c.Send(message);
            }
            catch (Exception e)
            {
                UdpErrors.ErrorSendingTraffic(Convert.ToString(e));
            }
        }

        public byte[] getLatestBytes()
        {
            return serverSocket.getCurrentBuffer();
        }

        public string getLatestBufferAsString()
        {
            byte[] buffer = serverSocket.getCurrentBuffer();
            if (buffer == null) return "NUL";
            else return ASCIIEncoding.ASCII.GetString(buffer);
        }

    }

    class HttpResponsePackage
    {
        public byte[] data;
        public string reponseType;
    }
}
