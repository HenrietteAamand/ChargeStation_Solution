    using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using ChargeStation.Classlibrary;

namespace ChargeStation.Test.Unit
{
    class RfidReader_Simulator_Test_Unit
    {
        private RFIDDetectedEventArgs receivedEventArgs;
        private IRdfReader uut;

        [SetUp]
        public void Setup()
        {
            receivedEventArgs = null;
            uut = new RfidReader_simulator();
            uut.RfidDetected("");

            uut.RFIDDectectedEvent +=
                (o, args) =>
                {
                    receivedEventArgs = args;
                };
        }

        [TestCase("RfidTest")]
        public void RfidDetected_AddRfid_ExpextRfid(string Rfid)
        {
            uut.RfidDetected(Rfid);
            Assert.That(receivedEventArgs.Id, Is.EqualTo(Rfid));
        }
    }
}
