using System;

namespace ChargeStation.Classlibrary
{
    public class USBCharger : IUSBCharger
    {

        // Constants
        private const double MaxCurrent = 500.0; // mA
        private const double FullyChargedCurrent = 2.5; // mA
        private const double OverloadCurrent = 750; // mA
        private const int ChargeTimeMinutes = 20; // minutes
        private const int CurrentTickInterval = 250; // ms


        public event EventHandler<CurrentEventArgs> CurrentValueEvent;
        public double CurrentValue { get; private set; }
        public bool Connected { get; set; }

        private bool _overload;
        private bool _charging;
        private System.Timers.Timer _timer;

        protected virtual void OnCurrentChanged()
        {
            CurrentValueEvent?.Invoke(this, new CurrentEventArgs() { Current = this.CurrentValue });
        }

        public USBCharger()
        {
            CurrentValue = 0.0;
            Connected = true;
            _overload = false;

            _timer = new System.Timers.Timer();
            _timer.Enabled = false;
            _timer.Interval = CurrentTickInterval;
           

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

                OnCurrentChanged();
                _charging = true;

                _timer.Start();
            }
        }

        public void StopCharge()
        {
            _timer.Stop();

            CurrentValue = 0.0;
            OnCurrentChanged();

            _charging = false;
        }

        private void OnNewCurrent()
        {
            CurrentValueEvent?.Invoke(this, new CurrentEventArgs() { Current = this.CurrentValue });
        }
    }
}