using System;
using System.Collections.Generic;
using System.Text;

namespace ChargeStation.Classlibrary
{
    public interface IDisplay
    {
        public void ChangeText(MessageCode text);
    }
}
