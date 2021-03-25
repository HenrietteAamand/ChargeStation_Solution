using System;
using System.Collections.Generic;
using System.Text;

namespace ChargeStation.Classlibrary
{
    public class ChargeControl : IChargeControl
    {

        private IUSBCharger iUsbCharger;
        private IDisplay iDisplay;

        public double? CurrentCurrent { get; set; }
        private const int ZeroCurrent = 0;
        private const int TelefonOpladet = 5;
        private const int Ladestrøm = 500;
        private const int Kortslutning =501;
        

        public ChargeControl(IUSBCharger iUsbCharger, IDisplay display)
        {
            this.iUsbCharger = iUsbCharger;
            iDisplay = display;
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
                case <=ZeroCurrent:
                    break;
                case <= TelefonOpladet:
                    iDisplay.ChangeText(MessageCode.TelefonFuldtOpladet);
                    break;
                case <=Ladestrøm:
                    iDisplay.ChangeText(MessageCode.LadningIgang);
                    break;
                case >=Kortslutning:
                    iDisplay.ChangeText(MessageCode.Kortslutning);
                    break;
                default:
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

