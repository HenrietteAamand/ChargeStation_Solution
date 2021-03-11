using System;

namespace ChargeStation.Classlibrary
{
    public class CurrentEventArgs : EventArgs
    {
        // Value in mA (milliAmpere)
        public double Current { get; set; }

    }
}
