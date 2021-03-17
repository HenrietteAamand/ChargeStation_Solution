using System;
using System.Collections.Generic;
using System.Text;

namespace ChargeStation.Classlibrary
{
    public interface ILogfile
    {
        public void DoorUnlocked(string eId);
        public void DoorLocked(string rfidId);
    }
}
