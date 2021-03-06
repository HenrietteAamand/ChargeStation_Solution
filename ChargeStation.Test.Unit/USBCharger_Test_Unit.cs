using ChargeStation.Classlibrary;
using NSubstitute;
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

        [Test]
        public void StartCharge_CurrentSetToOverload_CorrectNewCurrentReceived()
        {
            #region SetUp

            uut = new USBCharger(true, true);
            uut.CurrentValueEvent +=
                (o, args) =>
                {
                    receivedEventArgs = args;

                };

            #endregion


            uut.StartCharge();
            Assert.That(receivedEventArgs.Current, Is.EqualTo(750));
            
        }
        [Test]
        public void StartCharge_CurrentSetToOverloadIsNotConnected_CorrectNewCurrentReceived()
        {
            #region SetUp

            uut = new USBCharger(false,false);
            uut.CurrentValueEvent +=
                (o, args) =>
                {
                    receivedEventArgs = args;

                };
            

                #endregion
            uut.StartCharge();

            Assert.That(receivedEventArgs.Current, Is.EqualTo(0.0));
            
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

        [Test]
        public void StartCharge_EventCalled_CorrectNewCurrentCalled()
        {
            double lastValue = 1000;
            uut.CurrentValueEvent += (o, args) => lastValue = (double)args.Current;

            uut.StartCharge();
            
            System.Threading.Thread.Sleep(300);

            Assert.That(lastValue, Is.EqualTo(500.0));


        }




        #endregion


    }
}