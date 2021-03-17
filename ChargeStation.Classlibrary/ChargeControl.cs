using System;
using System.Collections.Generic;
using System.Text;

namespace ChargeStation.Classlibrary
{
    public class ChargeControl : IChargeControl
    {
<<<<<<< HEAD
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
=======
        public bool IsConnected()
        {
            throw new NotImplementedException();
>>>>>>> 92b605c1e89c8665f4d543f0ea6716faf588f2a3
        }

        public void StartCharge()
        {
<<<<<<< HEAD
            iUsbCharger.StartCharge();
=======
            throw new NotImplementedException();
>>>>>>> 92b605c1e89c8665f4d543f0ea6716faf588f2a3
        }

        public void StopCharge()
        {
<<<<<<< HEAD
            iUsbCharger.StopCharge();
=======
            throw new NotImplementedException();
>>>>>>> 92b605c1e89c8665f4d543f0ea6716faf588f2a3
        }
    }
}
