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
}
