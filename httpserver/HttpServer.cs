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
        private static readonly string RootCatalog = "c:/temp";
        public int Port { get; set; }
        
        

       public HttpServer(int port)
        {
            Port = port;
        }

        public void Run()
        {
            TcpListener connectionSocket = new TcpListener(DefaultPort);
            connectionSocket.Start();
           
                while (true)
                {
                    TcpClient client = null;
                        
                    try
                    {
                        client = connectionSocket.AcceptTcpClient();
                        Stream ns = client.GetStream();
                        StreamReader sr = new StreamReader(ns);
                        StreamWriter sw = new StreamWriter(ns);
                        sw.AutoFlush = true; // enable automatic flushing

                        //string message = sr.ReadLine();
                        //Console.WriteLine("Client: " + message);

                        //sw.Write("HTTP/1.0 200 OK \r\n");
                        //sw.Write("\r\n");
                        //sw.Write("Hello");
                        //sw.Write(message);
                        

                       string request = sr.ReadLine();
                        if (request != null)
                        {
                            string[] tokens = request.Split(' ');
                            if (tokens.Length == 0)
                            {
                                throw new NullReferenceException("Invalid http request");
                                    //or ArguementOutOfRangeException?
                            }
                            using (var reader = new StreamReader(RootCatalog + tokens[1]))
                            {
                                string line = reader.ReadToEnd();
                                Console.WriteLine(line);
                                sw.Write(line);
                                sw.Flush();
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
    }
}
