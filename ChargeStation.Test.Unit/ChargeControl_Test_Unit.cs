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
        private IDisplay iDisplay;
        //private readonly TestUSBCharcgerSource testUsbCharger;

        [SetUp]
        public void Setup()
        {
            usbCharger = Substitute.For<IUSBCharger>();
            iDisplay = Substitute.For<IDisplay>();
            uut = new ChargeControl(usbCharger, iDisplay);

        }

        #region Event()

        [TestCase(-10)]
        [TestCase(1)]
        [TestCase(500)]
        [TestCase(700)]
        [TestCase(1000)]
        public void CurrentChanged_DifferentArguments_CurrentIsCorrect_ExpectEvent(int newCurrent)
        {
            usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs() { Current = newCurrent });
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
        public void IsConnectetUpOnStartCharge_Received_ExpectTrue()
        {
            usbCharger.Connected.Returns(true);


            uut.StartCharge();
            usbCharger.Received(1).StartCharge();

            Assert.That(uut.IsConnected, Is.EqualTo(true));

        }
        [Test]
        public void IsConnectetUpOnStartCharge_Received_ExpectFalse()
        {
            usbCharger.Connected.Returns(false);


            uut.StartCharge();
            usbCharger.Received(1).StartCharge();

            Assert.That(uut.IsConnected, Is.EqualTo(false));

        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void OpladningFærdig_RaiseCurrentEvent_DisplayReceiveCorrectMessage(int ladestrøm)
        {

            usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs() { Current = ladestrøm }); //Sørger for at testID fra før er det samme
            iDisplay.Received(1).ChangeText(MessageCode.TelefonFuldtOpladet);

        }

        [TestCase(500)]
        public void OpladningIgang_RaiseCurrentEvent_DisplayReceiveCorrectMessage(int ladestrøm)
        {
            usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs() { Current = ladestrøm }); //Sørger for at testID fra før er det samme
            iDisplay.Received(1).ChangeText(MessageCode.LadningIgang);

        }
        [TestCase(0)]
        public void ZerroCurrent_RaiseCurrentEvent_DisplayReceiveCorrectMessage(int ladestrøm)
        {
            usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs() { Current = ladestrøm }); //Sørger for at testID fra før er det samme
            iDisplay.Received(0).DidNotReceiveWithAnyArgs();
        }
        [TestCase(501)]
        [TestCase(510)]
        [TestCase(600)]
        [TestCase(700)]
        [TestCase(1000)]
        public void OpladningKortsluttet_RaiseCurrentEvent_DisplayReceiveCorrectMessage(int ladestrøm)
        {

            usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs() { Current = ladestrøm }); //Sørger for at testID fra før er det samme
            iDisplay.Received(1).ChangeText(MessageCode.Kortslutning);

        }

        [Test]
        public void CurrentEventNull_RaiseCurrentEvent_SwitchCaseDefault()
        {
            uut.CurrentCurrent = null;

            usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs() {Current = null}); //Sørger for at testID fra før er det samme
            iDisplay.Received(0).DidNotReceiveWithAnyArgs();
        }






    }




}
