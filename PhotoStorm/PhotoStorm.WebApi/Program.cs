using System;
using System.Configuration;
using Microsoft.Owin.Hosting;
using PhotoStorm.Console.Extensions;


namespace PhotoStorm.WebApi
{
    class Program
    {
        private static string _baseAddress = ConfigurationManager.AppSettings["UrlAddress"];

        static void Main(string[] args)
        {
            if (string.IsNullOrEmpty(_baseAddress))
                _baseAddress = "http://+:9000/";
            try
            {
                WebApp.Start<Startup>(_baseAddress);
                XConsole.WriteLine("Application started at {0}", ConsoleColor.Magenta, _baseAddress);
                while (true)
                {

                }
            }
            catch (Exception ex)
            {
                XConsole.WriteLine(ex.ToString(), ConsoleColor.Red);
            }
            
            // Start OWIN host 
            System.Console.ReadLine();


        }
    }
}
