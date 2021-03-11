using System;
using System.Collections.Generic;
using System.Text;

namespace ChargeStation.Classlibrary
{
    

    public class Door_Simulator : IDoor
    {

        public event EventHandler<DoorStatusEventArgs> DoorStatusChangedEvent;
        
        
        private bool _doorIsOpen;

        public bool DoorIsOpen
        {
            get { return _doorIsOpen; }
            set
            {
                if (value != _doorIsOpen)
                {
                    _doorIsOpen = value;
                    OnDoorStatusChanged(new DoorStatusEventArgs
                    {
                        IsOpen = value

                    });
                }
            }
        }

        private void OnDoorStatusChanged(DoorStatusEventArgs eventArgs)
        {
            DoorStatusChangedEvent?.Invoke(this, eventArgs);
        }

        private bool _doorIsLocked;

        public bool DoorIsLocked
        {
            get { return _doorIsLocked; }
            set { _doorIsLocked = value; }
        }

        public void LockDoor()
        {
            if (!DoorIsOpen)
            {
                DoorIsLocked = true;
            }
        }

        public void UnlockDoor()
        {
            DoorIsLocked = false;
        }

        public void SimulateOpeningTry()
        {
            if (DoorIsLocked)
            {
                throw new ArgumentException("Døren var låst da du åbnede..");
            }
            else
            {
                DoorIsOpen = true;
            }
        }

        public void SimulateClosingTry()
        {
            if (DoorIsLocked)
            {
                throw new ArgumentException("Døren var låst da du lukkede..");
            }
            else
            {
                DoorIsOpen = false;
            }
        }

    }

    public class DoorStatusEventArgs : EventArgs
    {
        public bool IsOpen { get; set; }
    }
}
