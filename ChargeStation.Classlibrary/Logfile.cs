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

        public Logfile(IFileWriter fileWriter, ITimeProvider timeprovider)
        {
            _fileWriter = fileWriter;
            _timeprovider = timeprovider;
        }

        public void DoorUnlocked(string rfid_Id)
        {
            string time = _timeprovider.GetTime();

            string logLine = "Door was unlocked at " + time + " with Rfid id " + rfid_Id;

            _fileWriter.WriteLineToFile(logLine);
        }

        public void DoorLocked(string rfid_Id)
        {
            string time = _timeprovider.GetTime();
            string logLine = "Door was locked at " + time + " with Rfid id " + rfid_Id;
            _fileWriter.WriteLineToFile(logLine);
        }
    }
}
