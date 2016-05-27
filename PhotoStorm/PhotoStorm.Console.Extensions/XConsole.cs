using System;

namespace PhotoStorm.Console.Extensions
{
    public static class XConsole
    {
        public static void WriteLine(string value, ConsoleColor color = ConsoleColor.Gray, params object[] args)
        {
            System.Console.ForegroundColor = color;
            System.Console.WriteLine($"{DateTime.UtcNow} | {value}", args);
        }
    }
}
