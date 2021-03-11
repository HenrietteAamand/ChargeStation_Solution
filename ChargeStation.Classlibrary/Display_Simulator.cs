using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;

namespace ChargeStation.Classlibrary
{
    public class Display_Simulator : IDisplay
    {
        public void ChangeText(string indlæsRfid)
        {
            Console.WriteLine(indlæsRfid);
        }
    }
}
