using System;
using System.Collections.Generic;
using System.Text;

namespace ChargeStation.Classlibrary
{
    public interface IRdfReader
    {
        public event EventHandler<RFIDDetectedEventArgs> RFIDDectected;
        public void RFIDReader(string id);
    }
}
