using System.IO;

namespace ChargeStation.Classlibrary
{
    public class FileWriter : IFileWriter
    {
        private string filename = "Logfile.txt";

        /// <summary>
        /// Denne metode gemmer line i en ny fil hvis den ikke er oprette, ellers skirver den blot til i bunden af filen
        /// Filen gemmes her: <<ChargeStation_Solution\ChargeStation_Application\bin\Debug\netcoreapp3.1>>
        /// </summary>
        /// <param name="line">Den linje der ønskes at skrives til loggen</param>
        public async void WriteLineToFile(string line)
        {
            if (!File.Exists(filename))
            {
                using StreamWriter sw = new StreamWriter(File.Create(filename));
                {
                    sw.WriteLine(line);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filename))
                {
                    sw.WriteLine(line);
                }
            }
        }
    }
}