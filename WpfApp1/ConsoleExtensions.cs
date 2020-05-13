using System;

namespace Demo
{
    public static class ConsoleExtensions
    {
        public static void WriteLine(this object o)
        {
            Console.WriteLine(o);
        }
    }
}