using System;
using System.Collections.Generic;
using System.Text;

namespace ChargeStation.Classlibrary
{
    
    public interface IUSBCharger
    {
        // Event triggered on new current value
        event EventHandler<CurrentEventArgs> CurrentValueEvent;

        // Direct access to the current current value
        public double CurrentValue { get; }

        // Require connection status of the phone
        public bool Connected { get; }

        // Start charging
        public void StartCharge();
        // Stop charging
        public void StopCharge();

    }
}
