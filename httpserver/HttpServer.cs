using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace httpserver
{
    public class HttpServer
    {
        public static readonly int DefaultPort = 8888;
        private const string RootCatalog = "c:/temp";
        public int Port { get; set; }

        public HttpServer(int port)
        {
            Port = port;
        }


        public void Run()
        {
            var connectionSocket = new TcpListener(DefaultPort);
            connectionSocket.Start();
            while (true)
            {
                TcpClient client = null;
                try
                {
                    client = connectionSocket.AcceptTcpClient();
                    Task.Run(() => GetStaticValue(client));
                   
                    Stream ns = client.GetStream();
                    var sr = new StreamReader(ns);
                    var sw = new StreamWriter(ns) {AutoFlush = true};

                    string message = sr.ReadLine();
                    Console.WriteLine("Client: " + message);
                    
                    if (message != null)
                    {
                        string[] tokens = message.Split(' ');

                        if (tokens.Length != 3)
                        {
                            sw.Write("HTTP/1.0 400 Illegal request\r\n\r\n");
                        }
                        if (!(tokens[2].Equals("HTTP/1.0") || tokens[2].Equals("HTTP/1.1")))
                        {
                            sw.Write("HTTP/1.0 400 Illegal protocol\r\n");
                        }
                        else
                        {
                            switch (tokens[0])
                            {
                                case "GET": DoGet(tokens, sw);
                                    break;
                                case "POST":
                                case "HEAD":
                                    sw.Write("HTTP/1.0 200 Not Implemented\r\n");
                                    break;
                                default:
                                    sw.Write("HTTP/1.0 400 Illegal request\r\n");
                                    break;
                            }
                        }
                    }
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    if (client != null)
                    {
                        client.Close();
                    }
                }
            }
        }
        private static void DoGet(string[] tokens, StreamWriter sw)
        {
            try
            {
                using (var reader = new StreamReader(RootCatalog + tokens[1]))
                {
                    sw.Write("HTTP/1.0 200 OK\r\n");
                    string line = reader.ReadToEnd();
                    Console.WriteLine(line);
                    sw.Write(line);
                    sw.Flush();
                }
            }
            catch (FileNotFoundException)
            {
                sw.Write("HTTP/1.0 404 Not Found\r\n");
            }
        }
        private void GetStaticValue(TcpClient connectionSocket)
        {
            try
            {
                Console.WriteLine("Server activated");
                var ns = connectionSocket.GetStream();
                var sr = new StreamReader(ns);
                var sw = new StreamWriter(ns);
                sw.Write("HTTP/1.0 200 OK\r\n");
                sw.Write("\r\n Hello World!");
                sw.Flush();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                connectionSocket.Close();
            }
        }
    }
}
