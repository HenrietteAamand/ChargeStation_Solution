using System;
using System.Collections.Generic;
using System.Text;

namespace ChargeStation.Classlibrary
{
    public class ChargeControl : IChargeControl
    {

        private IUSBCharger iUsbCharger;
        private IDisplay iDisplay = new IDisplay();

        public double CurrentCurrent { get; set; }
        private const int ZeroCurrent = 0;
        private const int TelefonOpladet = 5;
        private const int Ladestrøm = 500;
        private const int Kortslutning =501;

        public ChargeControl(IUSBCharger iUsbCharger)
        {
            this.iUsbCharger = iUsbCharger;
            
            iUsbCharger.CurrentValueEvent += HandleCurrentChangeEvent;
        }

        private void HandleCurrentChangeEvent(object sender, CurrentEventArgs e)
        {
            CurrentCurrent = e.Current;
            // Handle current data
            HandleCurretDataEvent();
        }

        private void HandleCurretDataEvent()
        {
            switch (CurrentCurrent)
            {
                case ZeroCurrent:
                    break;
                case <= TelefonOpladet:
                    iDisplay.MessageCode.Text(7);
                    break;
                case <=Ladestrøm:
                    iDisplay.MessageCode.Text(8);
                    break;
                case >=Kortslutning:
                    iDisplay.MessageCode.Text(9);
                    break;
            }
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

