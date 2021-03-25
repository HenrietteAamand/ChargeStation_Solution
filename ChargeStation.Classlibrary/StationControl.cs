using System;
using System.Collections.Generic;
using System.Text;

namespace ChargeStation.Classlibrary
{
    public class StationControl
    {
        private readonly IDoor _door;
        private readonly IChargeControl _chargeControl;
        private readonly IRdfReader _rfdReader;
        private readonly IDisplay _display;
        private readonly ILogfile _logfile;
        private string rfidID;

        public StationControl(IDoor door, IChargeControl chargeControl, IRdfReader rfdReader, IDisplay display, ILogfile logfile)
        {
            _door = door;
            _door.DoorStatusChangedEvent += HandleDoorEvent;
            _chargeControl = chargeControl;
            _rfdReader = rfdReader;
            _rfdReader.RFIDDectectedEvent += HandleRfidEvent;
            _display = display;
            _logfile = logfile;
        }

        private void HandleDoorEvent(object sender, DoorStatusEventArgs doorStatus)
        {
            switch (doorStatus.IsOpen)
            {
                case true:
                    _display.ChangeText(MessageCode.TilslutTelefon);
                    break;
                default:
                    _display.ChangeText(MessageCode.IndlaesRFID);
                    break;

            }
        }

        private void HandleRfidEvent(object sender, RFIDDetectedEventArgs rfidReader)
        {
            if (_door.DoorIsLocked == false && _door.DoorIsOpen == false)  //Hvis døren er ulåst, og der registreres et RfidEvent, så skal vi en ting
            {
                switch (_chargeControl.IsConnected())
                {
                    case true:
                        rfidID = rfidReader.Id;
                        _chargeControl.StartCharge();
                        _door.LockDoor();
                        _logfile.DoorLocked(rfidID);
                        _display.ChangeText(MessageCode.LadeskabOptaget);
                        break;

                    case false:
                        _display.ChangeText(MessageCode.Tilslutningsfejl);
                        break;
                }
            }
            else if (_door.DoorIsLocked)
            {
                switch (CheckId(rfidReader.Id))
                {
                    case true: //Hvis det detekterede rfid ID er det samme som før
                        _chargeControl.StopCharge();                    // Stop opladningen
                        _door.UnlockDoor();                             // Lås døren op
                        _display.ChangeText(MessageCode.FjernTelefon);  // Udskriv "Fjern telefon" på display
                        rfidID = "";
                        _logfile.DoorUnlocked(rfidReader.Id);           // Gem i lockfilen, at døren er blevet låst op

                        break;

                    case false: // Hvis det detekterede rfid ID er forskelligt fra før
                        _display.ChangeText(MessageCode.RFIDFejl);
                        break;
                }
            }
        }

        private bool CheckId(string id)
        {
            if (rfidID == id)
            {
                return true;
            }

            return false;
        }

    }
}
