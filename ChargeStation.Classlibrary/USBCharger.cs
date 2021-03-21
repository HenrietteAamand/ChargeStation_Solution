using System;

namespace ChargeStation.Classlibrary
{
    public class USBCharger : IUSBCharger
    {

        public event EventHandler<CurrentEventArgs> CurrentValueEvent;

        public double CurrentValue { get; private set; }

        public bool Connected { get; set; }
        private bool _overload { get; set; }
        private bool _charging;
        private const double OverloadCurrent = 750;


        protected virtual void OnCurrentChanged()
        {
            CurrentValueEvent?.Invoke(this, new CurrentEventArgs() { Current = this.CurrentValue });
        }

        public USBCharger()
        {
            
            Connected = true;
           
        }
        public USBCharger(bool connected, bool overload)
        {

            Connected = connected;
            _overload = overload;

        }
        public void StartCharge()
        {
           
            if (!_charging)
            {
                if (Connected && !_overload)
                {
                    CurrentValue = 500;
                }
                else if (Connected && _overload)
                {
                    CurrentValue = OverloadCurrent;
                }
                else if (!Connected)
                {
                    CurrentValue = 0.0;
                }
            }

            OnCurrentChanged();
        }

        public void StopCharge()
        {
           
            OnCurrentChanged();
            _charging = false;
            CurrentValue = 0.0;

        }


    }
}