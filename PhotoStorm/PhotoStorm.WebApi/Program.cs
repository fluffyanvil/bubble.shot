using System;
using System.Net.Http;
using Microsoft.Owin.Hosting;


namespace PhotoStorm.WebApi
{
    class Program
    {

        static void Main(string[] args)
        {
            var baseAddress = "http://localhost:9000/";
            WebApp.Start<Startup>(url: baseAddress);
            // Start OWIN host 
            while (true)
            {
                
            }


        }
    }
}
