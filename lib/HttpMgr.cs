using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Web;

namespace civet
{
    class HttpMan
    {

        static readonly List<HttpServer> servers = new List<HttpServer>();
        static readonly List<string> ports = new List<string>();
        public static string Go(string cmd)
        {
            string port = cmd.Split(' ')[0];
            string keyword = cmd.Split(' ')[1].ToLower();

            switch (keyword)
            {
                case "start":
                    if (ports.Contains(port)) { HttpErrors.ServerAlreadyExists("HTTP", port); return ""; }
                    HttpServer s = new HttpServer();
                    s.port = port;
                    bool started = s.start();
                    if (!started) HttpErrors.Generic(null, port);
                    else
                    {
                        servers.Add(s);
                        ports.Add(port);
                    }
                    break;
                case "stop":
                    if (!ports.Contains(port)) { HttpErrors.ServerDoesntExist("HTTP", port); return ""; }
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
                            else HttpErrors.ServerNotDisposed("HTTP", port);
                        }
                    }
                    break;
                default:
                    FileErrors.UnknownFileManagerInstruction(keyword);
                    break;
            }

            return "";
        }

    }

    class HttpServer
    {
        public List<string> paths = new List<string>();
        public List<string> goPoints = new List<string>();
        public string port = "";
        public static bool runServer = true;
        static Thread serverThread;

        public static HttpListener listener;
        public static string url = $"";
        public static int pageViews = 0;
        public static int requestCount = 0;
        public static string response = "";

        public bool dispose()
        {
            try
            {
                listener.Stop();
                runServer = false;
                return true;
            }
            catch (Exception e) { HttpErrors.Generic(e, port); return false; }
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
                url = $"http://+:{port}/";
                // Create a Http server and start listening for incoming connections
                listener = new HttpListener();
                listener.Prefixes.Add(url);
                listener.Start();

                // Handle requests
                Task listenTask = HandleIncomingConnections();
                listenTask.GetAwaiter().GetResult();

                // Close the listener
                listener.Close();
            }
            catch (Exception e) { HttpErrors.Generic(e, port); }
        }

        public static async Task HandleIncomingConnections()
        {

            while (runServer)
            {
                try
                {
                    // Will wait here until we hear from a connection
                    HttpListenerContext ctx = await listener.GetContextAsync();

                    // Peel out the requests and response objects
                    HttpListenerRequest req = ctx.Request;
                    HttpListenerResponse resp = ctx.Response;

                    // Print out some info about the request
                    //if (config.logInfo) Console.WriteLine("Request #: {0}", ++requestCount);
                    //if (config.logInfo) Console.WriteLine(req.Url.ToString());
                    //if (config.logInfo) Console.WriteLine(req.HttpMethod);
                    //if (config.logInfo) Console.WriteLine(req.UserHostName);
                    //if (config.logInfo) Console.WriteLine(req.UserAgent);
                    //if (config.logInfo) Console.WriteLine();

                    string data = "";
                    using (System.IO.Stream body = req.InputStream) // here we have data
                    {
                        using (var reader = new System.IO.StreamReader(body, req.ContentEncoding))
                        {
                            data = reader.ReadToEnd();
                        }
                    }



                    // Write the response info
                    HttpResponsePackage r = MainThreadHandleContentReceived(req.Url.ToString(), req.HttpMethod, req.UserHostName, req.UserAgent, data);
                    byte[] responseData = r.data;
                    resp.ContentType = r.reponseType;
                    resp.ContentEncoding = Encoding.UTF8;
                    resp.ContentLength64 = responseData.LongLength;

                    // Write out to the response stream (asynchronously), then close it
                    await resp.OutputStream.WriteAsync(responseData, 0, responseData.Length);
                    resp.Close();
                }
                catch (Exception e)
                {

                }
            }
        }

        public bool AddPoint(string path, string point)
        {
            if (paths.Contains(path)) HttpErrors.ServerPathAlreadyRegistered(port, path);
            else { paths.Add(path); goPoints.Add(point); }
            return true;
        }

        public bool RemovePoint(string path)
        {
            if (paths.Contains(path)) HttpErrors.ServerPathNotRegistered(port, path);
            else
            {
                for (int i = 0; i < paths.Count; i++) if (paths[i].ToLower() == path.ToLower()) { paths.RemoveAt(i); goPoints.RemoveAt(i); return true; }
            }
            return true;
        }

        public static HttpResponsePackage MainThreadHandleContentReceived(string path, string HttpMethod, string UserHostName, string UserAgent, string data)
        {
            HttpResponsePackage r = new HttpResponsePackage();

            r.reponseType = "text/html";
            r.data = Encoding.ASCII.GetBytes("<h1>This is a test feature</h1><p>If you're seeing this, that means it probably worked!</p>");

            return r;
        }
    }

    class HttpResponsePackage
    {
        public byte[] data;
        public string reponseType;
    }
}
