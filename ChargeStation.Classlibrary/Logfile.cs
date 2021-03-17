using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ChargeStation.Classlibrary
{
    public class Logfile : ILogfile
    {
        private readonly IFileWriter _fileWriter;
        private readonly ITimeProvider _timeprovider;
        private string filename = "Logfile";
        private FileStream streamer;
        private StreamWriter writer;

        public Logfile(IFileWriter fileWriter, ITimeProvider timeprovider)
        {
            _fileWriter = fileWriter;
            _timeprovider = timeprovider;
            if (!File.Exists(filename))
            {
                streamer = new FileStream("Log", FileMode.Create, FileAccess.Write);
                streamer.Close();
            }
        }

        public void DoorUnlocked(string rfid_Id)
        {
            string logLine = "Door was unlocked at " + DateTime.Now + " with Rfid id " + rfid_Id;
            File.AppendAllText(filename, logLine);
        }

        public void DoorLocked(string rfid_Id)
        {
            string logLine = "Door was locked at " + DateTime.Now + " with Rfid id " + rfid_Id;
            File.AppendAllText(filename, logLine);
        }
    }
}
