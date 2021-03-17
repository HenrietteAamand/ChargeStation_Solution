using NUnit.Framework;
using NSubstitute;
using ChargeStation.Classlibrary;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

namespace ChargeStation.Test.Unit
{
    class StationControl_Test_Unit
    {
        private StationControl uut;
        private ILogfile logfile;
        private IDoor door;
        private IChargeControl chargeControl;
        private IRdfReader rfdReader;
        private IDisplay display;
        [SetUp]
        public void Setup()
        {
            logfile = Substitute.For<ILogfile>();
            door = Substitute.For<IDoor>();
            display = Substitute.For<IDisplay>();
            chargeControl = Substitute.For<IChargeControl>();
            rfdReader = Substitute.For<IRdfReader>();
            uut = new StationControl(door, chargeControl, rfdReader, display, logfile);
        }

        // Zero: Da der ikke er nogen state i klassen, kan denne ikke testes
        // One: Her testes samtlige metoder, hvor de bliver kaldt én gang

        // Dette er en mock test. Tester på Display
        [TestCase(true, MessageCode.TilslutTelefon)]
        [TestCase(false, MessageCode.IndlaesRFID)]
        public void HandleDoorEvent(bool stateOfDoor, MessageCode code)
        {
            door.DoorStatusChangedEvent += Raise.EventWith(new DoorStatusEventArgs {IsOpen = stateOfDoor});
            display.Received(1).ChangeText(code);
        }


        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}
