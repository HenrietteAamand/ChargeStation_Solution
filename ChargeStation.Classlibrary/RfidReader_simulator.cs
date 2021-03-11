using System;
using System.Collections.Generic;
using System.Text;

namespace ChargeStation.Classlibrary
{
    public class RfidReader_simulator : IRdfReader
    {
        private bool ReadRFID;

        public event EventHandler<RFIDDetectedEventArgs> RFIDDectected;

        public RfidReader_simulator()
        {
            ReadRFID = false;
        }

        public void simmulateRFIDReader(bool readRFID)
        {
            //Event ind her
            ReadRFID = readRFID;
        }

        public void RFIDReader(int id)
        {
            if (ReadRFID == true)
            {
                //StationControl.RfidDetected(id);
            }
        }



    }
}
