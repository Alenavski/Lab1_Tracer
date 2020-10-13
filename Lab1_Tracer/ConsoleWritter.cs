using System;

namespace Lab1_Tracer
{
    public class ConsoleWritter : IWritter
    {
        public void Write(string String)
        {
            Console.WriteLine(String);
        }
    }
}