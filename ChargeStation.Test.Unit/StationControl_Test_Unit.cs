using System.ComponentModel;
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
        public void HandleDoorEvent_changeDoorStatus_mockenumIsCorrect(bool stateOfDoor, MessageCode code)
        {
            door.DoorStatusChangedEvent += Raise.EventWith(new DoorStatusEventArgs {IsOpen = stateOfDoor});
            display.Received(1).ChangeText(code);
        }

        [Test]
        public void HandleRfidEvent_raisRfidEventWhenDoorIsNOTLockedAndCargerIsConnected_testOnMultipleMocs()
        {
            //Arrange
            chargeControl.IsConnected().Returns(false);

            // Act
            rfdReader.RFIDDectectedEvent += Raise.EventWith(new RFIDDetectedEventArgs() { Id = "testID"});
            display.Received(1).ChangeText(MessageCode.Tilslutningsfejl);
        }

        [Test]
        public void HandleRfidEvent_raisRfidEventWhenDoorIsNOTLocked_testOnMultipleMocs()
        {
            //Arrange
            chargeControl.IsConnected().Returns(true);

            // Act
            rfdReader.RFIDDectectedEvent += Raise.EventWith(new RFIDDetectedEventArgs() { Id = "testID" });
            Assert.Multiple(() =>
            {
                chargeControl.Received(1).StartCharge();
                door.Received(1).LockDoor();
                logfile.Received(1).DoorLocked("testID");
                display.Received(1).ChangeText(MessageCode.LadeskabOptaget);
            });
        }

        [Test]
        public void HandleRfidEvent_raisRfidEventWhenDoorIsLockedAndRFIDIsTheSame_testOnMultipleMocs()
        {

            //Arrange
            chargeControl.IsConnected().Returns(true);
            rfdReader.RFIDDectectedEvent += Raise.EventWith(new RFIDDetectedEventArgs() {Id = "testID"}); //Sørger for at testID fra før er det samme

            // Act
            rfdReader.RFIDDectectedEvent += Raise.EventWith(new RFIDDetectedEventArgs() { Id = "testID" });
            Assert.Multiple(() =>
            {
                chargeControl.Received(1).StopCharge();
                door.Received(1).UnlockDoor();
                logfile.Received(1).DoorUnlocked("testID");
                display.Received(1).ChangeText(MessageCode.FjernTelefon);
            });
        }

        [Test]
        public void HandleRfidEvent_raisRfidEventWhenDoorIsLockedAndRFIDIsNOTTheSame_testOnMultipleMocs()
        {
            //Arrange
            chargeControl.IsConnected().Returns(true);
            rfdReader.RFIDDectectedEvent += Raise.EventWith(new RFIDDetectedEventArgs() { Id = "testIDwrong" }); //Sørger for at testID fra før er det samme

            // Act
            rfdReader.RFIDDectectedEvent += Raise.EventWith(new RFIDDetectedEventArgs() { Id = "testID" });
            display.Received(1).ChangeText(MessageCode.RFIDFejl);
            }

    }
}
