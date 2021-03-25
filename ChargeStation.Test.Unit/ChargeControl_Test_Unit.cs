using System.Diagnostics;
using System.Threading;
using ChargeStation.Classlibrary;
using NSubstitute;
using NSubstitute.Core;
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
        public void CurrentEventChanged_DifferentArguments_CurrentIsCorrect_ExpectEvent(int newCurrent)
        {
            usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs() { Current = newCurrent });
            Assert.That(uut.CurrentCurrent, Is.EqualTo(newCurrent));
        }


       
        [TestCase(-10,1)]
        [TestCase(1,2)]
        [TestCase(500,2)]
        [TestCase(700,2)]
        [TestCase(1000,2)]
        public void CurrentEventChanged_DifferentArgumentsTwoTimes_CurrentIsCorrect_ExpectEvent(int newCurrent, int CallsReceived)
        {
            usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs() { Current = 400 });
            Thread.Sleep(500);
            usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs() { Current = newCurrent });

            iDisplay.Received(CallsReceived).ReceivedCalls();
          
            Assert.That(uut.CurrentCurrent, Is.EqualTo(newCurrent));


        }
        #endregion
        [Test]
        public void StartCharge_Received_ExpectOnetime()
        {
            uut.StartCharge();
            usbCharger.Received(1).StartCharge();
            usbCharger.DidNotReceive().StopCharge();
        }
        [Test]
        public void StartCharge_ReceivedZero_ExpectOnetime()
        {
            usbCharger.Received(0).StartCharge();
            usbCharger.DidNotReceive().StopCharge();
        }
        [Test]
        public void StartCharge_Received100000Times_ExpectOnetime()
        {
            int theBigNumberRule = 10000;
            for (int i = 0; i < theBigNumberRule; i++)
            {
                uut.StartCharge();
            }
            usbCharger.Received(theBigNumberRule).StartCharge();
            usbCharger.DidNotReceive().StopCharge();
        }

        [Test]
        public void StopCharge_ReceivedZeroTimes_ExpectOnetime()
        {
            usbCharger.Received(0).StopCharge();
            usbCharger.DidNotReceive().StartCharge();
        }
        [Test]
        public void StopCharge_Received_ExpectOnetime()
        {
            uut.StopCharge();
            usbCharger.Received(1).StopCharge();
            usbCharger.DidNotReceive().StartCharge();
        }
        [Test]
        public void StopCharge_ReceivedTwice_ExpectOnetime()
        {
            uut.StopCharge();
            uut.StopCharge();
            usbCharger.Received(2).StopCharge();
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
        [TestCase(3)]
        [TestCase(5)]
        public void OpladningFærdig_RaiseCurrentEvent_DisplayReceiveCorrectMessage(int ladestrøm)
        {
            usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs() { Current = ladestrøm }); 
            iDisplay.Received(1).ChangeText(MessageCode.TelefonFuldtOpladet);
        }

        [TestCase(-100)]
        [TestCase(-1)]
        [TestCase(6)]
        [TestCase(100)]
        public void OpladningFærdigUdenForBoundaries_RaiseCurrentEvent_DisplayDoesNotReceiveTelefonFuldtOpladet(int ladestrøm)
        {
            usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs() { Current = ladestrøm });
            iDisplay.Received(0).ChangeText(MessageCode.TelefonFuldtOpladet);

        }

        [TestCase(6)]
        [TestCase(150)]
        [TestCase(500)]
        public void OpladningIgang_RaiseCurrentEvent_DisplayReceiveCorrectMessage(int ladestrøm)
        {
            usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs() { Current = ladestrøm }); 
            iDisplay.Received(1).ChangeText(MessageCode.LadningIgang);
        }

        [TestCase(4)]
        [TestCase(5)]
        [TestCase(501)]
        [TestCase(750)]
        public void OpladningIgangUdenForBoundaries_RaiseCurrentEvent_DisplayReceiveCorrectMessage(int ladestrøm)
        {
            usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs() { Current = ladestrøm });
            iDisplay.Received(0).ChangeText(MessageCode.LadningIgang);
        }


        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-5)]
        [TestCase(-100)]
        public void ZerroCurrent_RaiseCurrentEvent_DisplayReceiveCorrectMessage(int ladestrøm)
        {
            usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs() { Current = ladestrøm }); 
            iDisplay.Received(0).DidNotReceiveWithAnyArgs();
        }

        [TestCase(501)]
        [TestCase(510)]
        [TestCase(600)]
        [TestCase(700)]
        [TestCase(1000)]
        public void OpladningKortsluttet_RaiseCurrentEvent_DisplayReceiveCorrectMessage(int ladestrøm)
        {

            usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs() { Current = ladestrøm }); 
            iDisplay.Received(1).ChangeText(MessageCode.Kortslutning);
        
        }

        [TestCase(-5)]
        [TestCase(5)]
        [TestCase(499)]
        public void OpladningKortsluttetVærdiOutOfBoundaries_RaiseCurrentEvent_DisplayDoesNotRecieveKorslutning(int ladestrøm)
        {

            usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs() { Current = ladestrøm });
            iDisplay.Received(0).ChangeText(MessageCode.Kortslutning);

        }

        [Test]
        public void CurrentEventNull_RaiseCurrentEvent_SwitchCaseDefault()
        {
            uut.CurrentCurrent = null;

            usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs() {Current = null});
            iDisplay.DidNotReceive().ChangeText(Arg.Any<MessageCode>());

        }






    }




}
