using System;

namespace ChargeStation.Classlibrary
{
    public class USBCharger : IUSBCharger
    {
        public event EventHandler<CurrentEventArgs> ValueEvent;
        public double CorrentValue { get; }
        public bool Connected { get; }

        public void StartCharge()
        {
            throw new System.NotImplementedException();
        }

        public void StopCharge()
        {
            throw new System.NotImplementedException();
        }
    }
}