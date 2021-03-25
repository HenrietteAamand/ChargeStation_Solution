using ChargeStation.Classlibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace SystemTestCover
{
    [TestClass]
    public class UnitTest1
    {

        private StationControl uut;
        private ILogfile logfile;
        private IDoor door;
        private IChargeControl chargeControl;
        private IRdfReader rfdReader;
        private IDisplay display;
        private IUSBCharger usbCharger;

        [TestMethod]
        public void TestMethod1()
        {
            for (int i = 0; i < 10; i++)
            {
                logfile = Substitute.For<ILogfile>();
                door = Substitute.For<IDoor>();
                display = Substitute.For<IDisplay>();
                usbCharger = Substitute.For<IUSBCharger>();
                chargeControl = new ChargeControl(usbCharger, display);
                rfdReader = Substitute.For<IRdfReader>();
                uut = new StationControl(door, chargeControl, rfdReader, display, logfile);


                // sd HandleDoorEvent
                // Vi �bner d�ren:
                door.DoorStatusChangedEvent += Raise.EventWith(new DoorStatusEventArgs { IsOpen = true });


                // Vi tester at d�ren er �ben og beskeden Tilslut telefon udskrives:
                display.Received(1).ChangeText(MessageCode.TilslutTelefon);

                // Vi tilslutter telefonen og lukker d�ren:
                door.DoorStatusChangedEvent += Raise.EventWith(new DoorStatusEventArgs { IsOpen = false });

                #region Vi forventer at display viser besked om indl�s RFID

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
                // Skabet er nu optaget og vi lader med l�st d�r
                #endregion

                #region Forkert RFID tag pr�ver at f� adgang til skabet
                rfdReader.RFIDDectectedEvent += Raise.EventWith(new RFIDDetectedEventArgs() { Id = "SpecialHashCode:aokjshfdciuosydcnakjsefd0l" });
                usbCharger.Received(0).StopCharge();
                door.Received(0).UnlockDoor();
                logfile.Received(0).DoorUnlocked("SpecialHashCode:aokjshfdciuosydcnakjsefdi");
                display.Received(0).ChangeText(MessageCode.FjernTelefon);
                //Skabet �bnede ikke!
                #endregion

                #region Vi vil hente vores telefon nu med korrekt RFID Tag.
                // Vi vil hente vores telefon nu med korrekt RFID Tag.
                rfdReader.RFIDDectectedEvent += Raise.EventWith(new RFIDDetectedEventArgs() { Id = "SpecialHashCode:aokjshfdciuosydcnakjsefdi" });
                usbCharger.Received(1).StopCharge();
                door.Received(1).UnlockDoor();
                logfile.Received(1).DoorUnlocked("SpecialHashCode:aokjshfdciuosydcnakjsefdi");
                display.Received(1).ChangeText(MessageCode.FjernTelefon);

                #endregion

                #region Vi �bner d�ren og tager telefonen

                door.DoorStatusChangedEvent += Raise.EventWith(new DoorStatusEventArgs { IsOpen = true });

                usbCharger.Connected.Returns(false);

                #endregion

            }

        }
    }
}
