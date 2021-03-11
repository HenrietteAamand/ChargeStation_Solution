using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ChargeStation.Classlibrary
{
    public class Logfile : ILogfile
    {
        private string filename = "Logfile";
        private FileStream streamer;
        private StreamWriter writer;

        public Logfile()
        {
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
