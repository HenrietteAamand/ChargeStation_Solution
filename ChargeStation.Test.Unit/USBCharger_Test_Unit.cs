using ChargeStation.Classlibrary;
using NUnit.Framework;

namespace ChargeStation.Test.Unit
{
    public class USBCharger_Test_Unit
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

        #region StartCharger
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


        #endregion
        #region StopCharger

        [Test]
        public void StopCharge_CurrentSetToNewValue_EventFired()
        {
            uut.StopCharge();
            Assert.That(receivedEventArgs, Is.Not.Null);

        }

        [Test]
        public void StopCharge_CurrentSetToNewValue_CorrectNewCurrentReceived()
        {
            uut.StopCharge();
            Assert.That(receivedEventArgs.Current, Is.EqualTo(0.0));

        }


        #endregion
        #region OverLoadCurrent
        [Test]
        public void StartCharge_OverLoadCurrentIstrue_EventFired()
        {
           

            Assert.That(receivedEventArgs, Is.Not.Null);

        }

        [Test]
        public void StartCharge_CurrentSetToNewValue_CorrectNewCurrentReceived()
        {
            uut.StartCharge();
            Assert.That(receivedEventArgs.Current, Is.EqualTo(500));

        }


        #endregion

    }
}