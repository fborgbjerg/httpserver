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
        static void Main(string[] args)
        {
           
            HttpServer server = new HttpServer(8888);
            server.Run();
            
            
            Console.WriteLine("Hello http server");
        }
    }
}
