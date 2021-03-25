    using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using ChargeStation.Classlibrary;

namespace ChargeStation.Test.Unit
{
    class RfidReader_Simulator_Test_Unit
    {
        private RFIDDetectedEventArgs receivedEventArgs;
        private IRdfReader uut;

        [SetUp]
        public void Setup()
        {
            receivedEventArgs = null;
            uut = new RfidReader_simulator();
            uut.RfidDetected("");

            uut.RFIDDectectedEvent +=
                (o, args) =>
                {
                    receivedEventArgs = args;
                };
        }

        //Zero
        [Test]
        public void RfidDetected_Zero_NoMethodCall_ExpextEventIsNotFired()
        {
            //Arrange

            //Act
            // No act for Zero test

            //Assert
            Assert.That(receivedEventArgs, Is.Null);
        }

        //One
        [TestCase("RfidTest")]
        public void RfidDetected_One_CallMethod_ExpextRfidIsRfidAndEventIsFired(string Rfid)
        {
            //Arrange

            //Act
            uut.RfidDetected(Rfid);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(receivedEventArgs.Id, Is.EqualTo(Rfid));
                Assert.That(receivedEventArgs, Is.Not.Null);
            });
        }

        //Many - tester med store/små bogstaver, tal og tom teststreng
        [TestCase("RfidTest")]
        [TestCase("")]
        [TestCase("test id")]
        [TestCase("1234567890")]
        [TestCase("ID")]
        public void RfidDetected_Many_CallMethod_ExpextRfidIsRfidAndEventIsFired(string Rfid)
        {
            //Arrange

            //Act
            uut.RfidDetected(Rfid);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(receivedEventArgs.Id, Is.EqualTo(Rfid));
                Assert.That(receivedEventArgs, Is.Not.Null);
            });
        }

        //Many - kalder metoden flere gange med forskellig parameter, tester at ID gemmes ved det sidste kald
        [Test]
        public void RfidDetected_Many_CallMethodTwoTimes_ExpextRfidIsRfidAndEventIsFired()
        {
            //Arrange

            //Act
            uut.RfidDetected("RFID");
            uut.RfidDetected("ID");
            uut.RfidDetected("testRfid");

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(receivedEventArgs.Id, Is.EqualTo("testRfid"));
                Assert.That(receivedEventArgs, Is.Not.Null);
            });
        }
    }
}
