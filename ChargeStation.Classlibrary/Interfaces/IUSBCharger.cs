using System;
using System.Collections.Generic;
using System.Text;

namespace ChargeStation.Classlibrary
{
    
    public interface IUSBCharger
    {
        // Event triggered on new current value
        event EventHandler<CurrentEventArgs> ValueEvent;

        // Direct access to the current current value
        public double CorrentValue { get; }

        // Require connection status of the phone
        public bool Connected { get; }

        // Start charging
        public void StartCharge();
        // Stop charging
        public void StopCharge();

    }
}
