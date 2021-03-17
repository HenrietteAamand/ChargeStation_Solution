using System;
using ChargeStation.Classlibrary;

namespace ChargeStation_Application
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            IFileWriter log = new FileWriter();
            log.WriteLineToFile("Test line from Program");
        }
    }
}
