using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace httpserver
{
    public class HttpServer
    {
        public static readonly int DefaultPort = 8888;
        private string _response = ""; //Empty response string, is build upon in Run()
        private static readonly string RootCatalog = "C:/temp";
        public int Port { get; set; }

        public HttpServer(int port)
        {
            Port = port;
        }
        public void Run()
        {
            
            while (true)
            {
                TcpClient client = null;
                try
                {
                    TcpListener connectionSocket = new TcpListener(DefaultPort);
                    connectionSocket.Start();
                    while (true)
                    {
                        client = connectionSocket.AcceptTcpClient();
                        Task.Run(()=>GetValue(client));
                        //GetValue(client);
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

        private void GetValue(TcpClient client)
        {
            try
            {
                Console.WriteLine("Server activated");
                Stream ns = client.GetStream();

                StreamReader sr = new StreamReader(ns);
                StreamWriter sw = new StreamWriter(ns);
                sw.AutoFlush = true; // enable automatic flushing

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
                            case "GET":
                                try
                                {
                                    using (var reader = new StreamReader(RootCatalog + tokens[1]))
                                    {
                                        sw.Write("HTTP/1.0 200 OK\r\n");
                                        _response += reader.ReadToEnd();
                                        Console.WriteLine(_response);
                                        sw.Write("\r\n"+_response);
                                        sw.Flush();
                                    }
                                }
                                catch (FileNotFoundException)
                                {
                                    sw.Write("HTTP/1.0 404 Not Found\r\n");
                                }
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

                Console.WriteLine("Some error" + ex.Message);
            }
            finally
            {
                client.Close();
            }
        }
    }
}