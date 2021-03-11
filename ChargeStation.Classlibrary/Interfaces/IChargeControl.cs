using System;
using System.Collections.Generic;
using System.Text;

namespace ChargeStation.Classlibrary
{
    public interface IChargeControl
    {

        public bool IsConnected();
        public void StartCharge();
        public void StopCharge();



    }
}
