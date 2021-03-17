using System;
using System.Collections.Generic;
using System.Text;

namespace ChargeStation.Classlibrary
{
    public interface IDoor
    {
        public event EventHandler<DoorStatusEventArgs> DoorStatusChangedEvent;
        public void LockDoor();
        public void UnlockDoor();
    }
}
