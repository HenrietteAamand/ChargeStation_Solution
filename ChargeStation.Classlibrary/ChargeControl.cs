using System;
using System.Collections.Generic;
using System.Text;

namespace ChargeStation.Classlibrary
{
    public class ChargeControl : IChargeControl
    {
        private IUSBCharger iUsbCharger;

        public double CurrentCurrent { get; set; }
        public ChargeControl(IUSBCharger iUsbCharger)
        {
            this.iUsbCharger = iUsbCharger;
            iUsbCharger.CurrentValueEvent += HandleCurrentChangeEvent;
        }

        private void HandleCurrentChangeEvent(object sender, CurrentEventArgs e)
        {
            CurrentCurrent = e.Current;
            //REGULATE
        }

        public bool IsConnected()
        {
            return iUsbCharger.Connected;
        }

        public void StartCharge()
        {
            iUsbCharger.StartCharge();
        }

        public void StopCharge()
        {
            iUsbCharger.StopCharge();
        }
    }
}
