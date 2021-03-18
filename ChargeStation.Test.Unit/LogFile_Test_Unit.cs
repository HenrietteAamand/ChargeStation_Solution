using ChargeStation.Classlibrary;
using NSubstitute;
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

        // OBS! Skal jeg lave flere testcases?
        [Test]
        public void DoorUnlocked_SetTimeAndID_checkFileWriterLine()
        {
            faketimeProvider.GetTime().Returns("1234");
            uut.DoorUnlocked("RFID_ID");
            fakeFilewriter.Received(1).WriteLineToFile("Door was unlocked at 1234 with Rfid id RFID_ID");
        }

        [Test]
        public void DoorLocked_SetTimeAndID_checkFileWriterLine()
        {
            faketimeProvider.GetTime().Returns("1234");
            uut.DoorUnlocked("RFID_ID");
            fakeFilewriter.Received(1).WriteLineToFile("Door was unlocked at 1234 with Rfid id RFID_ID");
        }
    }
}
