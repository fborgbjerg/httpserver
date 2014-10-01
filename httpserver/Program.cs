using System;


namespace httpserver
{
    class Program
    {
        public static readonly int DefaultPort = 8888;

        static void Main(string[] args)
        {

            Console.WriteLine("Hello http server");
           
            var server = new HttpServer(DefaultPort);
            server.Run();
        }
    }
}
