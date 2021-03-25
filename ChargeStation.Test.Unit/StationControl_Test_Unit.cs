using System;
using System.ComponentModel;
using NUnit.Framework;
using NSubstitute;
using ChargeStation.Classlibrary;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using NSubstitute.Core.Arguments;

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

        // Zero: Det testes, at der ikke er kaldt nogle metoder i klassen. Dette er delvist en whiteboxtest,
            // da jeg ved, at alle metoderne bruger Display.Changetext(). Derfor kan jeg tjekke, at der er modtaget 0 kald til denne metode
        [Test]
        public void uut_setUpuut_noCallsRecievInDisplay()
        {
            display.DidNotReceive().ChangeText(Arg.Any<MessageCode>());
        }


        // One: Her testes samtlige metoder, hvor de bliver kaldt én gang
        // Dette er en mock test. Tester på Display, og at når døren åbnes/lukkes så udskrives det rigtige på skærmen
        [TestCase(true, MessageCode.TilslutTelefon)]
        [TestCase(false, MessageCode.IndlaesRFID)]
        public void HandleDoorEvent_changeDoorStatus_mockenumIsCorrect(bool stateOfDoor, MessageCode code)
        {
            //Arrange 
            //Act
            door.DoorStatusChangedEvent += Raise.EventWith(new DoorStatusEventArgs { IsOpen = stateOfDoor });
            
            //Assert
            display.Received(1).ChangeText(code);
        }

        // Tester at vi får vist en tilslutningsfejl, hvis ikke telefonen er connectet
        [Test]
        public void HandleRfidEvent_raisRfidEventWhenDoorIsNOTLockedAndCargerIsConnected_testOnMultipleMocs()
        {
            //Arrange
            chargeControl.IsConnected().Returns(false);

            // Act
            rfdReader.RFIDDectectedEvent += Raise.EventWith(new RFIDDetectedEventArgs() { Id = "testID" });
            
            //Assert
            display.Received(1).ChangeText(MessageCode.Tilslutningsfejl);
        }

        //Tester at alle de korrekte ting sker, når der holdes et RFID tag foran døren når en ladning påbegyndes
        [Test]
        public void HandleRfidEvent_raisRfidEventWhenDoorIsNOTLocked_testOnMultipleMocs()
        {
            //Arrange
            chargeControl.IsConnected().Returns(true);

            // Act
            rfdReader.RFIDDectectedEvent += Raise.EventWith(new RFIDDetectedEventArgs() { Id = "testID" });

            //Assert
            Assert.Multiple(() =>
            {
                chargeControl.Received(1).StartCharge();
                door.Received(1).LockDoor();
                logfile.Received(1).DoorLocked("testID");
                display.Received(1).ChangeText(MessageCode.LadeskabOptaget);
            });
        }

        //Tester at alle de korrekte ting sker, når der holdes et RFID tag foran døren når en ladning afsluttes
        [Test]
        public void HandleRfidEvent_raisRfidEventWhenDoorIsLockedAndRFIDIsTheSame_testOnMultipleMocs()
        {
            //Arrange
            chargeControl.IsConnected().Returns(true);
            door.DoorIsLocked.Returns(false);
            rfdReader.RFIDDectectedEvent += Raise.EventWith(new RFIDDetectedEventArgs() { Id = "testID" }); //Sørger for at testID fra før er det samme
            door.DoorIsLocked.Returns(true);

            // Act
            rfdReader.RFIDDectectedEvent += Raise.EventWith(new RFIDDetectedEventArgs() { Id = "testID" });

            //Assert
            Assert.Multiple(() =>
            {
                chargeControl.Received(1).StopCharge();
                door.Received(1).UnlockDoor();
                logfile.Received(1).DoorUnlocked("testID");
                display.Received(1).ChangeText(MessageCode.FjernTelefon);
            });
        }

        //Tester at alle de korekte ting sker, når der holdes et RFID tag foran døren
        //når en ladning forsøges at afsluttes, men der briges det forkerte RFID tag
        [Test]
        public void HandleRfidEvent_raisRfidEventWhenDoorIsLockedAndRFIDIsNOTTheSame_testOnMultipleMocs()
        {
            //Arrange
            chargeControl.IsConnected().Returns(true);
            door.DoorIsLocked.Returns(false);
            rfdReader.RFIDDectectedEvent += Raise.EventWith(new RFIDDetectedEventArgs() { Id = "testIDwrong" }); //Sørger for at testID fra før er det samme
            door.DoorIsLocked.Returns(true);

            // Act
            rfdReader.RFIDDectectedEvent += Raise.EventWith(new RFIDDetectedEventArgs() { Id = "testID" });
            
            //Assert
            display.Received(1).ChangeText(MessageCode.RFIDFejl);
        }

    }
}
