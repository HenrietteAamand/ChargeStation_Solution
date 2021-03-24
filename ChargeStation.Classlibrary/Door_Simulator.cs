using System;
using System.Collections.Generic;
using System.Text;

namespace ChargeStation.Classlibrary
{
    public class DoorStatusEventArgs : EventArgs
    {
        public bool IsOpen { get; set; }
    }

    public class Door_Simulator : IDoor
    {

        #region Event

        public event EventHandler<DoorStatusEventArgs> DoorStatusChangedEvent;

        //TODO Jeg ved ikke hvordan OnDoorStatusChanged skal testes...
        private void OnDoorStatusChanged(DoorStatusEventArgs eventArgs)
        {
            DoorStatusChangedEvent?.Invoke(this, eventArgs);
        }
        

        #endregion

        #region Props

        private bool _doorIsOpen;

        public bool DoorIsOpen
        {
            get { return _doorIsOpen; }
            set { _doorIsOpen = value; }
        }

        private bool _doorIsLocked;

        public bool DoorIsLocked
        {
            get { return _doorIsLocked; }
            set { _doorIsLocked = value; }
        }
        

        #endregion

        #region Ctor

        public Door_Simulator()
        {
            DoorIsLocked = false;
            DoorIsOpen = false;
        }
        #endregion



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

        private void DoorChangeStatus(bool newStatus)
        {
            //Hvis statusen er ændret vil OnDoorStatusChanged blive kaldt
            if (DoorIsOpen != newStatus)
            {
                DoorIsOpen = newStatus;

                OnDoorStatusChanged(new DoorStatusEventArgs
                {
                    IsOpen = newStatus

                });
            }
        }

        public void SimulateOpeningTry()
        {
            if (!DoorIsLocked)
            {
                DoorChangeStatus(true);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Døren var låst da du prøvede at åbne..");
                //throw new ArgumentException("Døren var låst da du åbnede..");
            }
        }


        public void SimulateClosingTry()
        {
            if (!DoorIsLocked)
            {
                DoorChangeStatus(false);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Døren var låst da du prøvede at lukke..");
                //throw new ArgumentException("Døren var låst da du lukkede..");
            }
        }
        

    }


}
