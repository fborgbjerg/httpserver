using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace httpserver
{
    class Program
    {
        public static readonly int DefaultPort = 8888;

        static void Main(string[] args)
        {

            Console.WriteLine("Hello http server");
           
            HttpServer server = new HttpServer(DefaultPort);
            server.Run();


            
            
            
        }
    }
}
