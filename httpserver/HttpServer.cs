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

                        string message = sr.ReadLine();
                        Console.WriteLine("Client: " + message);

                        sw.Write("HTTP/1.0 200 OK \r\n");
                        sw.Write("\r\n");
                        //sw.Write("Hello");
                        sw.Write(message);
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
