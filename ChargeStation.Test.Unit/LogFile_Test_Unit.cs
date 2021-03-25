using ChargeStation.Classlibrary;
using NSubstitute;
using NSubstitute.Core.Arguments;
using NUnit.Framework;

namespace ChargeStation.Test.Unit
{
    class LogFile_Test_Unit
    {
        private IFileWriter fakeFilewriter;
        private ITimeProvider faketimeProvider;
        private Logfile uut;
        [SetUp]
        public void Setup()
        {
            fakeFilewriter = Substitute.For<IFileWriter>();
            faketimeProvider = Substitute.For<ITimeProvider>();
            uut = new Logfile(fakeFilewriter, faketimeProvider);
        }

        // Tester ZOMBIES i det omfang det giver muligt
        // Z - tester begge metoder hvor der bruges tomme tekststrenge, og om der har været 0 kald efter bare oprettelse
        [Test]
        public void DoorUnlocked_SetEmptyTimeAnaID_checkFileWriterLine()
        {
            faketimeProvider.GetTime().Returns("");
            uut.DoorUnlocked("");
            fakeFilewriter.Received(1).WriteLineToFile("Door was unlocked at  with Rfid id ");
        }

        [Test]
        public void DoorLocked_SetEmptyTimeAndID_checkFileWriterLine()
        {
            faketimeProvider.GetTime().Returns("");
            uut.DoorLocked("");
            fakeFilewriter.Received(1).WriteLineToFile("Door was locked at  with Rfid id ");
        }

        [Test]
        public void LogSetUp_DoNothingmpty_checkNoethodCallsWereMade()
        {
            //Arrange - ingen handlinger

            //Act - setup

            //Assert - Denne skal være kaldt 0 gange for at teste at hverken DoorLocked og DoorUnlocked bliver  kaldt
            fakeFilewriter.DidNotReceive().WriteLineToFile(Arg.Any<string>());
        }



        // O - Tester begge metoder hvor der indsætte plausible værdier
        [TestCase("1234","RFID_ID", "Door was unlocked at 1234 with Rfid id RFID_ID")]
        [TestCase("1234ForAndenGang", "RFID_ID2", "Door was unlocked at 1234ForAndenGang with Rfid id RFID_ID2")]
        [TestCase("1234igen", "RFID_ID_igen", "Door was unlocked at 1234igen with Rfid id RFID_ID_igen")]
        public void DoorUnlocked_SetTimeAndID_checkFileWriterLine(string time, string ID, string result)
        {
            faketimeProvider.GetTime().Returns(time);
            uut.DoorUnlocked(ID);
            fakeFilewriter.Received(1).WriteLineToFile(result);
        }

        [TestCase("12345", "RFID_ID5", "Door was locked at 12345 with Rfid id RFID_ID5")]
        [TestCase("12345ForAndenGang", "RFID_ID25", "Door was locked at 12345ForAndenGang with Rfid id RFID_ID25")]
        [TestCase("12345igen", "RFID_ID_igen5", "Door was locked at 12345igen with Rfid id RFID_ID_igen5")]
        public void DoorLocked_SetTimeAndID_checkFileWriterLine(string time, string ID, string result)
        {
            faketimeProvider.GetTime().Returns(time);
            uut.DoorLocked(ID);
            fakeFilewriter.Received(1).WriteLineToFile(result);
        }


        // M - Tester, at man kan kalde metoden to gange efter hinanden med samme ID
        [Test]
        public void DoorUnlocked_SetTimeAndIDAndAct2times_checkFileWriterLine()
        {
            faketimeProvider.GetTime().Returns("1234");
            uut.DoorUnlocked("RFID_ID");
            uut.DoorUnlocked("RFID_ID");
            fakeFilewriter.Received(2).WriteLineToFile("Door was unlocked at 1234 with Rfid id RFID_ID");
        }

        [Test]
        public void DoorLocked_SetTimeAndIDAndAct2times_checkFileWriterLine()
        {
            faketimeProvider.GetTime().Returns("1234");
            uut.DoorLocked("RFID_ID");
            uut.DoorLocked("RFID_ID");
            fakeFilewriter.Received(2).WriteLineToFile("Door was locked at 1234 with Rfid id RFID_ID");
        }

        // B - tester boundaries. Umiddelbart er der ikke nogen, da vi taler om en string, men der forsøges med:
            // - Tegn
            // - Æ, Ø og Å
            // - Tal og bogstaver og tegn
            // - Mellemrum

        [TestCase("!@[?", "£{¨-", "Door was unlocked at !@[? with Rfid id £{¨-")]
        [TestCase("ÆØÅ", "ØÅÆ", "Door was unlocked at ÆØÅ with Rfid id ØÅÆ")]
        [TestCase("123ÆØPG!)=", "321ÆØPG!(=", "Door was unlocked at 123ÆØPG!)= with Rfid id 321ÆØPG!(=")]
        [TestCase("Dette er min tid med mellemrum", "Dette er mit ID med mellemrum", "Door was unlocked at Dette er min tid med mellemrum with Rfid id Dette er mit ID med mellemrum")]
        public void DoorUnlocked_SetBoundaryValues_checkFileWriterLine(string time, string ID, string result)
        {
            faketimeProvider.GetTime().Returns(time);
            uut.DoorUnlocked(ID);
            fakeFilewriter.Received(1).WriteLineToFile(result);
        }

        [TestCase("!@[?", "£{¨-", "Door was locked at !@[? with Rfid id £{¨-")]
        [TestCase("ÆØÅ", "ØÅÆ", "Door was locked at ÆØÅ with Rfid id ØÅÆ")]
        [TestCase("123ÆØPG!)=", "321ÆØPG!(=", "Door was locked at 123ÆØPG!)= with Rfid id 321ÆØPG!(=")]
        [TestCase("Dette er min tid med mellemrum", "Dette er mit ID med mellemrum", "Door was locked at Dette er min tid med mellemrum with Rfid id Dette er mit ID med mellemrum")]
        public void DoorLocked_SetBoundaryValues_checkFileWriterLine(string time, string ID, string result)
        {
            faketimeProvider.GetTime().Returns(time);
            uut.DoorLocked(ID);
            fakeFilewriter.Received(1).WriteLineToFile(result);
        }

        // I - der er kun 2 metoder i klassen, og de er testet i ovenstående

        // E - Under boundaries er det forsøgt at teste obskurde værdier, og i zeroe er der testet med ingen værdi. Dermed er der ikke mere til E
    }
}
