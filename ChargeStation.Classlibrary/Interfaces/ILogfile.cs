using System;
using System.Collections.Generic;
using System.Text;

namespace ChargeStation.Classlibrary
{
    public interface ILogfile
    {
        public void DoorUnlocked(string eId);
        void DoorLocked(string rfidId);
    }
}
