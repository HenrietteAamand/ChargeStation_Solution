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
            chargeControl = new ChargeControl(usbCharger, display);
            rfdReader = Substitute.For<IRdfReader>();
            uut = new StationControl(door, chargeControl, rfdReader, display, logfile);


        }
        [Test]
        public void TotalSystemTest_FollowSekvensDiagram10Times_Correct()
        {

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

        [Test]
        public void TotalSystemTest_IntendedUseOneTime_Correct()
        {

            #region En lade session

            // sd HandleDoorEvent
            // User åbner døren:
            door.DoorStatusChangedEvent += Raise.EventWith(new DoorStatusEventArgs { IsOpen = true });
            door.DoorIsOpen.Returns(true);
            // døren er åben og
            // beskeden Tilslut telefon udskrives

            // User PlugIn Telefon
            usbCharger.Connected.Returns(true);
            // Telefon tilsluttet
            // Dør åbent

            // User lukker døren
            door.DoorStatusChangedEvent += Raise.EventWith(new DoorStatusEventArgs { IsOpen = false });
            door.DoorIsOpen.Returns(false);
            // Døren er lukket
            // beskeden Indlæs RFID udskrives 

            // User Scanner RFID
            rfdReader.RFIDDectectedEvent += Raise.EventWith(new RFIDDetectedEventArgs() { Id = "SpecialHashCode:aokjshfdciuosydcnakjsefdi" });
            door.DoorIsLocked.Returns(true);
            // Døren låses
            // Skabet er nu optaget
            // Ladning er i gang


            // User Scanner RFID
            rfdReader.RFIDDectectedEvent += Raise.EventWith(new RFIDDetectedEventArgs() { Id = "SpecialHashCode:aokjshfdciuosydcnakjsefdi" });
            door.DoorIsLocked.Returns(false);
            // Døren låses op
            // Beskeden Fjern telefon udskrives

            //User åbner døren
            door.DoorStatusChangedEvent += Raise.EventWith(new DoorStatusEventArgs { IsOpen = true });
            door.DoorIsOpen.Returns(true);
            //Døren er åbent
            // beskeden Indlæs RFID udskrives 

            //User Unplugger Telefon
            usbCharger.Connected.Returns(false);
            //Skabet er åbent og ulåst
            //Ingen mobil tilkoblet


            #endregion

            #region Test til at det er kørt 1 lade session

            usbCharger.Received(1).StartCharge();
            usbCharger.Received(1).StopCharge();
            door.Received(1).LockDoor();
            door.Received(1).UnlockDoor();
            logfile.Received(1).DoorLocked("SpecialHashCode:aokjshfdciuosydcnakjsefdi");
            logfile.Received(1).DoorUnlocked("SpecialHashCode:aokjshfdciuosydcnakjsefdi");
            display.Received(2).ChangeText(MessageCode.TilslutTelefon);
            display.Received(1).ChangeText(MessageCode.IndlaesRFID);
            display.Received(1).ChangeText(MessageCode.LadeskabOptaget);
            display.Received(1).ChangeText(MessageCode.FjernTelefon);
            display.Received(5).ChangeText(Arg.Any<MessageCode>());

            #endregion
        }

        [Test]
        public void TotalSystemTest_IntendedUseTwoTime_Correct()
        {
            #region En lade session

            // sd HandleDoorEvent
            // User åbner døren:
            door.DoorStatusChangedEvent += Raise.EventWith(new DoorStatusEventArgs { IsOpen = true });
            door.DoorIsOpen.Returns(true);
            // døren er åben og
            // beskeden Tilslut telefon udskrives

            // User PlugIn Telefon
            usbCharger.Connected.Returns(true);
            // Telefon tilsluttet
            // Dør åbent

            // User lukker døren
            door.DoorStatusChangedEvent += Raise.EventWith(new DoorStatusEventArgs { IsOpen = false });
            door.DoorIsOpen.Returns(false);
            // Døren er lukket
            // beskeden Indlæs RFID udskrives 

            // User Scanner RFID
            rfdReader.RFIDDectectedEvent += Raise.EventWith(new RFIDDetectedEventArgs() { Id = "SpecialHashCode:aokjshfdciuosydcnakjsefdi" });
            door.DoorIsLocked.Returns(true);
            // Døren låses
            // Skabet er nu optaget
            // Ladning er i gang


            // User Scanner RFID
            rfdReader.RFIDDectectedEvent += Raise.EventWith(new RFIDDetectedEventArgs() { Id = "SpecialHashCode:aokjshfdciuosydcnakjsefdi" });
            door.DoorIsLocked.Returns(false);
            // Døren låses op
            // Beskeden Fjern telefon udskrives

            //User åbner døren
            door.DoorStatusChangedEvent += Raise.EventWith(new DoorStatusEventArgs { IsOpen = true });
            door.DoorIsOpen.Returns(true);
            //Døren er åbent
            // beskeden Indlæs RFID udskrives 

            //User Unplugger Telefon
            usbCharger.Connected.Returns(false);
            //Skabet er åbent og ulåst
            //Ingen mobil tilkoblet


            #endregion

            #region Start på anden session

            // User PlugIn Telefon
            usbCharger.Connected.Returns(true);
            // Telefon tilsluttet
            // Dør åbent

            // User lukker døren
            door.DoorStatusChangedEvent += Raise.EventWith(new DoorStatusEventArgs { IsOpen = false });
            door.DoorIsOpen.Returns(false);
            // Døren er lukket
            // beskeden Indlæs RFID udskrives 

            // User Scanner RFID
            rfdReader.RFIDDectectedEvent += Raise.EventWith(new RFIDDetectedEventArgs() { Id = "SpecialHashCode:aokjshfdciuosydcnakjsefdi" });
            door.DoorIsLocked.Returns(true);
            // Døren låses
            // Skabet er nu optaget
            // Ladning er i gang


            // User Scanner RFID
            rfdReader.RFIDDectectedEvent += Raise.EventWith(new RFIDDetectedEventArgs() { Id = "SpecialHashCode:aokjshfdciuosydcnakjsefdi" });
            door.DoorIsLocked.Returns(false);
            // Døren låses op
            // Beskeden Fjern telefon udskrives

            //User åbner døren
            door.DoorStatusChangedEvent += Raise.EventWith(new DoorStatusEventArgs { IsOpen = true });
            door.DoorIsOpen.Returns(true);
            //Døren er åbent
            // beskeden Indlæs RFID udskrives 

            //User Unplugger Telefon
            usbCharger.Connected.Returns(false);
            //Skabet er åbent og ulåst
            //Ingen mobil tilkoblet

            #endregion

            #region Test til at det er kørt 2 lade session

            usbCharger.Received(2).StartCharge();
            usbCharger.Received(2).StopCharge();
            door.Received(2).LockDoor();
            door.Received(2).UnlockDoor();
            logfile.Received(2).DoorLocked("SpecialHashCode:aokjshfdciuosydcnakjsefdi");
            logfile.Received(2).DoorUnlocked("SpecialHashCode:aokjshfdciuosydcnakjsefdi");
            display.Received(3).ChangeText(MessageCode.TilslutTelefon);
            display.Received(2).ChangeText(MessageCode.IndlaesRFID);
            display.Received(2).ChangeText(MessageCode.LadeskabOptaget);
            display.Received(2).ChangeText(MessageCode.FjernTelefon);
            display.Received(9).ChangeText(Arg.Any<MessageCode>());

            #endregion
        }


        [Test]
        public void TotalSystemTest_DontPlugInThePhoneAndCanContinue_Correct()
        {

            #region En lade session

            // sd HandleDoorEvent
            // User åbner døren:
            door.DoorStatusChangedEvent += Raise.EventWith(new DoorStatusEventArgs { IsOpen = true });
            door.DoorIsOpen.Returns(true);
            // døren er åben og
            // beskeden Tilslut telefon udskrives

            // User lukker døren
            door.DoorStatusChangedEvent += Raise.EventWith(new DoorStatusEventArgs { IsOpen = false });
            door.DoorIsOpen.Returns(false);
            // Døren er lukket
            // beskeden Indlæs RFID udskrives 

            // User Scanner RFID
            rfdReader.RFIDDectectedEvent += Raise.EventWith(new RFIDDetectedEventArgs() { Id = "SpecialHashCode:aokjshfdciuosydcnakjsefdi" });
            //door.DoorIsLocked.Returns(true);
            // Døren låses
            // Skabet er nu optaget
            // Ladning er i gang




            // User åbner døren:
            door.DoorStatusChangedEvent += Raise.EventWith(new DoorStatusEventArgs { IsOpen = true });
            door.DoorIsOpen.Returns(true);
            // døren er åben og
            // beskeden Tilslut telefon udskrives


            // User PlugIn Telefon
            usbCharger.Connected.Returns(true);
            // Telefon tilsluttet
            // Dør åbent

            // User lukker døren
            door.DoorStatusChangedEvent += Raise.EventWith(new DoorStatusEventArgs { IsOpen = false });
            door.DoorIsOpen.Returns(false);
            // Døren er lukket
            // beskeden Indlæs RFID udskrives 

            // User Scanner RFID
            rfdReader.RFIDDectectedEvent += Raise.EventWith(new RFIDDetectedEventArgs() { Id = "SpecialHashCode:aokjshfdciuosydcnakjsefdi" });
            door.DoorIsLocked.Returns(true);
            // Døren låses
            // Skabet er nu optaget
            // Ladning er i gang


            // User Scanner RFID
            rfdReader.RFIDDectectedEvent += Raise.EventWith(new RFIDDetectedEventArgs() { Id = "SpecialHashCode:aokjshfdciuosydcnakjsefdi" });
            door.DoorIsLocked.Returns(false);
            // Døren låses op
            // Beskeden Fjern telefon udskrives

            //User åbner døren
            door.DoorStatusChangedEvent += Raise.EventWith(new DoorStatusEventArgs { IsOpen = true });
            door.DoorIsOpen.Returns(true);
            //Døren er åbent
            // beskeden Indlæs RFID udskrives 

            //User Unplugger Telefon
            usbCharger.Connected.Returns(false);
            //Skabet er åbent og ulåst
            //Ingen mobil tilkoblet


            #endregion

            

            usbCharger.Received(1).StartCharge();
            usbCharger.Received(1).StopCharge();
            door.Received(1).LockDoor();
            door.Received(1).UnlockDoor();
            logfile.Received(1).DoorLocked("SpecialHashCode:aokjshfdciuosydcnakjsefdi");
            logfile.Received(1).DoorUnlocked("SpecialHashCode:aokjshfdciuosydcnakjsefdi");
            display.Received(3).ChangeText(MessageCode.TilslutTelefon);
            display.Received(2).ChangeText(MessageCode.IndlaesRFID);
            display.Received(1).ChangeText(MessageCode.LadeskabOptaget);
            display.Received(1).ChangeText(MessageCode.FjernTelefon);
            display.Received(8).ChangeText(Arg.Any<MessageCode>());

        }




    }
}