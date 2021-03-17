using System;
using System.Collections.Generic;
using System.Text;

namespace ChargeStation.Classlibrary
{
    public interface IRdfReader
    {
        public event EventHandler<RFIDDetectedEventArgs> RFIDDectectedEvent;
        public void RfidDetected(string id);
    }
}
