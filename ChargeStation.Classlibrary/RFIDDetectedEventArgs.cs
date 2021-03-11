using System;
using System.Collections.Generic;
using System.Text;

namespace ChargeStation.Classlibrary
{
    public class RFIDDetectedEventArgs : EventArgs
    {
        public bool RFIDDetected { get; set; }
    }
}
