using System;
using System.Collections.Generic;
using System.Text;

namespace ChargeStation.Classlibrary
{
    // Henriette
    public class StationControl
    {
        private readonly IDoor _door;
        private readonly IChargeControl _chargeControl;
        private readonly IRdfReader _rfdReader;
        private readonly IDisplay _display;
        private readonly ILogfile _logfile;
        private string rfidID;
        private bool doorIsLocked;

        

        //Dette er controllerklassen. 
        public StationControl(IDoor door, IChargeControl chargeControl, IRdfReader rfdReader, IDisplay display, ILogfile logfile)
        {
            _door = door;
            _door.DoorStatusChangedEvent += HandleDoorEvent;
            _chargeControl = chargeControl;
            _rfdReader = rfdReader;
            _rfdReader.RFIDDectected += HandleRfidEvent;
            _display = display;
            _logfile = logfile;
            doorIsLocked = false;
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
            if (doorIsLocked == false)  //Hvis døren er låst, så skal vi en ting
            {
                switch (_chargeControl.IsConnected())
                {
                    //under test: Assert.Multible og test alt dens funktionalitet
                    case true:
                        rfidID = rfidReader.Id;
                        _chargeControl.StartCharge();
                        _door.LockDoor();
                        doorIsLocked = true;
                        _logfile.DoorLocked(rfidID);
                        _display.ChangeText(MessageCode.LadeskabOptaget);
                        break;

                    case false:
                        _display.ChangeText(MessageCode.Tilslutningsfejl);
                        break;
                }
            }
            else
            {
                switch (CheckId(rfidReader.Id))
                {
                    case true: //Hvis det detekterede rfid ID er det samme som før
                        _chargeControl.StopCharge();                    // Stop opladningen
                        _door.UnlockDoor();                             // Lås døren op
                        _display.ChangeText(MessageCode.FjernTelefon);  // Udskriv "Fjern telefon" på display
                        rfidID = "";
                        doorIsLocked = false;
                        _logfile.DoorUnlocked(rfidReader.Id);           // Gem i lockfilen, at døren er blevet låst op

                        break;

                    case false: // Hvis det detekterede rfid ID er forskelligt fra før
                        _display.ChangeText(MessageCode.RFIDFejl);
                        break;
                }
            }
        }

        public bool CheckId(string id)
        {
            if (rfidID == id)
            {
                return true;
            }

            return false;
        }

    }
}
