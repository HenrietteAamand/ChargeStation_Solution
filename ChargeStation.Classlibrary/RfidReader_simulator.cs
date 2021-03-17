using System;
using System.Collections.Generic;
using System.Text;

namespace ChargeStation.Classlibrary
{
    public class RFIDDetectedEventArgs : EventArgs
    {
        public string Id { get; set; }
    }

    public class RfidReader_simulator : IRdfReader
    {
        public event EventHandler<RFIDDetectedEventArgs> RFIDDectectedEvent;

        private void RFIDReaderIsActivated(RFIDDetectedEventArgs eventArgs)
        {
            RFIDDectectedEvent?.Invoke(this, eventArgs);
        }


        public void RfidDetected(string id)
        {
            RFIDReaderIsActivated(new RFIDDetectedEventArgs
            {
                Id = id

            }); ;
        }
    }
}
