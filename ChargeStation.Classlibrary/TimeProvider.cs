using System;
using System.Collections.Generic;
using System.Text;

namespace ChargeStation.Classlibrary
{
    public class TimeProvider : ITimeProvider
    {
        public string GetTime()
        {
            return Convert.ToString(DateTime.Now);
        }
    }
}
