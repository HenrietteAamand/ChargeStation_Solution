using System;
using System.Collections.Generic;
using System.Text;

namespace ChargeStation.Classlibrary
{
    public class RFIDDetectedEventArgs : EventArgs
    {
        public bool RFIDDetected { get; set; }
        public string Id { get; set; }
    }

    public class RfidReader_simulator : IRdfReader
    {
        private bool ReaderIsActive;
        private readonly StationControl StationControl;

        public event EventHandler<RFIDDetectedEventArgs> RFIDDectected;

        public RfidReader_simulator()
        {
            ReaderIsActive = false;
        }

        private void RFIDReaderIsActivated(RFIDDetectedEventArgs eventArgs)
        {
            RFIDDectected?.Invoke(this, eventArgs);
        }

        private void simmulateRFIDReaderActive(string readRFID)
        {
            if (ReaderIsActive == false)
            {
                ReaderIsActive = true;
                RFIDReaderStatusChange(true);
                RFIDReader(readRFID);
            }
            
        }

        public void simmulateRFIDReaderInactive()
        {
            if (ReaderIsActive == true)
            {
                ReaderIsActive = false;
                RFIDReaderStatusChange(false);
            }
            
        }

        private void RFIDReaderStatusChange(bool newStatus)
        {
            if (ReaderIsActive != newStatus)
            {
                ReaderIsActive = newStatus;

                RFIDReaderIsActivated(new RFIDDetectedEventArgs
                {
                    RFIDDetected = newStatus

                }) ;
            }

        }

        public void RFIDReader(string id)
        {
            StationControl.CheckId(id);
        }



    }
}
