using System;

namespace SharpShell
{
    internal class Logging
    {
        internal static void Log(string v)
        {
            Console.WriteLine(v);
        }

        internal static void Error(string v, Exception exception)
        {
            Console.WriteLine(string.Format("{0} {0}", v, exception.Message));
        }
    }
}