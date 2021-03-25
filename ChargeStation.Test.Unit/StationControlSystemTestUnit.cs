using ChargeStation.Classlibrary;
using NSubstitute;
using NUnit.Framework;

namespace ChargeStation.Test.Unit
{
    public class StationControlSystemTestUnit
    {
        private StationControl uut;
        private ILogfile logfile;
        private IDoor door;
        private IChargeControl chargeControl;
        private IRdfReader rfdReader;
        private IDisplay display;
        private IUSBCharger usbCharger;
 

        [SetUp]
        public void Setup()
        {
            logfile = Substitute.For<ILogfile>();
            door = Substitute.For<IDoor>();
            display = Substitute.For<IDisplay>();
            usbCharger = Substitute.For<IUSBCharger>();
            chargeControl = new ChargeControl(usbCharger,display);
            rfdReader = Substitute.For<IRdfReader>();
            uut = new StationControl(door, chargeControl, rfdReader, display, logfile);


        }
        [Test]
        public void TotalSystemTest_FollowSekvensDiagram10Times_Correct()
        {
            
                logfile = Substitute.For<ILogfile>();
                door = Substitute.For<IDoor>();
                display = Substitute.For<IDisplay>();
                usbCharger = Substitute.For<IUSBCharger>();
                chargeControl = new ChargeControl(usbCharger, display);
                rfdReader = Substitute.For<IRdfReader>();
                uut = new StationControl(door, chargeControl, rfdReader, display, logfile);


                // sd HandleDoorEvent
                // Vi åbner døren:
                door.DoorStatusChangedEvent += Raise.EventWith(new DoorStatusEventArgs { IsOpen = true });
                

                // Vi tester at døren er åben og beskeden Tilslut telefon udskrives:
                display.Received(1).ChangeText(MessageCode.TilslutTelefon);

                // Vi tilslutter telefonen og lukker døren:
                door.DoorStatusChangedEvent += Raise.EventWith(new DoorStatusEventArgs { IsOpen = false });

                #region Vi forventer at display viser besked om indlæs RFID

                display.Received(1).ChangeText(MessageCode.IndlaesRFID);

                rfdReader.RFIDDectectedEvent += Raise.EventWith(new RFIDDetectedEventArgs() { Id = "SpecialHashCode:aokjshfdciuosydcnakjsefdi" });
                usbCharger.Received(0).StartCharge();
                door.Received(0).LockDoor();
                logfile.Received(0).DoorLocked("SpecialHashCode:aokjshfdciuosydcnakjsefdi");
                display.Received(0).ChangeText(MessageCode.LadeskabOptaget);
                display.Received(1).ChangeText(MessageCode.Tilslutningsfejl);

                #endregion

                #region Raiser event med RFID detected
                // Raiser event med RFID detected
                usbCharger.Connected.Returns(true); //Faker den returnerer true
                rfdReader.RFIDDectectedEvent += Raise.EventWith(new RFIDDetectedEventArgs() { Id = "SpecialHashCode:aokjshfdciuosydcnakjsefdi" });
                usbCharger.Received(1).StartCharge();
                door.Received(1).LockDoor();
                door.DoorIsLocked.Returns(true);
                logfile.Received(1).DoorLocked("SpecialHashCode:aokjshfdciuosydcnakjsefdi");
                display.Received(1).ChangeText(MessageCode.LadeskabOptaget);
                // Skabet er nu optaget og vi lader med låst dør
                #endregion
                
                #region Forkert RFID tag prøver at få adgang til skabet
                rfdReader.RFIDDectectedEvent += Raise.EventWith(new RFIDDetectedEventArgs() { Id = "SpecialHashCode:aokjshfdciuosydcnakjsefd0l" });
                usbCharger.Received(0).StopCharge();
                door.Received(0).UnlockDoor();
                logfile.Received(0).DoorUnlocked("SpecialHashCode:aokjshfdciuosydcnakjsefdi");
                display.Received(0).ChangeText(MessageCode.FjernTelefon);
                //Skabet åbnede ikke!
                #endregion

                #region Vi vil hente vores telefon nu med korrekt RFID Tag.
                // Vi vil hente vores telefon nu med korrekt RFID Tag.
                rfdReader.RFIDDectectedEvent += Raise.EventWith(new RFIDDetectedEventArgs() { Id = "SpecialHashCode:aokjshfdciuosydcnakjsefdi" });
                usbCharger.Received(1).StopCharge();
                door.Received(1).UnlockDoor();
                logfile.Received(1).DoorUnlocked("SpecialHashCode:aokjshfdciuosydcnakjsefdi");
                display.Received(1).ChangeText(MessageCode.FjernTelefon);

                #endregion

                #region Vi åbner døren og tager telefonen

                door.DoorStatusChangedEvent += Raise.EventWith(new DoorStatusEventArgs { IsOpen = true });

                usbCharger.Connected.Returns(false); 

                #endregion

            



        }

    }
}