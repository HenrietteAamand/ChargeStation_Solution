using ChargeStation.Classlibrary;
using NUnit.Framework;

namespace ChargeStation.Test.Unit
{
    public class UnitTest_USBCharger
    {
        private IUSBCharger uut;
        private CurrentEventArgs receivedEventArgs;

        [SetUp]
        public void Setup()
        {
            receivedEventArgs = null;

            uut = new USBCharger();
            

            // Set up an event listener to check the event occurrence and event data
            uut.CurrentValueEvent +=
                (o, args) =>
                {
                    receivedEventArgs = args;

                };

        }

        [Test]
        public void StartCharge_CurrentSetToNewValue_EventFired()
        {
            uut.StartCharge();
            Assert.That(receivedEventArgs, Is.Not.Null);

        }

        [Test]
        public void StartCharge_CurrentSetToNewValue_CorrectNewCurrentReceived()
        {
            uut.StartCharge();
            Assert.That(receivedEventArgs.Current, Is.EqualTo(500));


        }

    }
}