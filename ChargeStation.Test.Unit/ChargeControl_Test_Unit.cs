using System.Diagnostics;
using ChargeStation.Classlibrary;
using NSubstitute;
using NUnit.Framework;

namespace ChargeStation.Test.Unit
{
    public class ChargeControl_Test_Unit
    {
        private ChargeControl uut;

        private IUSBCharger usbCharger;

        private CurrentEventArgs currentEventArgs;
        //private readonly TestUSBCharcgerSource testUsbCharger;

        [SetUp]
        public void Setup()
        {
            usbCharger = Substitute.For<IUSBCharger>();

            uut = new ChargeControl(usbCharger);

        }

        #region Event()

        [TestCase(-10)]
        [TestCase(1)]
        [TestCase(500)]
        [TestCase(700)]
        [TestCase(1000)]
        public void CurrentChanged_DifferentArguments_CurrentIsCorrect_ExpectEvent(int newCurrent)
        {
            usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs() {Current = newCurrent});
            Assert.That(uut.CurrentCurrent, Is.EqualTo(newCurrent));
            
        }

        //TODO Her skal være noget med HandleCurretDataEvent og hvordan den bearbejder de forskellige CUrrent ændringer som controller klasse

        #endregion
        [Test]
        public void StartCharge_Received_ExpectOnetime()
        {
            uut.StartCharge();
            usbCharger.Received(1).StartCharge();
            usbCharger.DidNotReceive().StopCharge();

        }
        [Test]
        public void StopCharge_Received_ExpectOnetime()
        {
            uut.StopCharge();
            usbCharger.Received(1).StopCharge();
            usbCharger.DidNotReceive().StartCharge();

        }

        [Test]
        public void IsConnectet_Received_ExpectOnetime()
        {
            uut.StopCharge();
            usbCharger.Received(1).StopCharge();
            usbCharger.DidNotReceive().StartCharge();

        }





    }




}
