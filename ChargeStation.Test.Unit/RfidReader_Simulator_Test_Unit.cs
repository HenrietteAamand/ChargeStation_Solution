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
            //Setup

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
            //Setup

            //Act
            uut.RfidDetected(Rfid);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(receivedEventArgs.Id, Is.EqualTo(Rfid));
                Assert.That(receivedEventArgs, Is.Not.Null);
            });
        }

        //Many
        [TestCase("RfidTest")]
        [TestCase("Rfid")]
        [TestCase("TestId")]
        [TestCase("1234567890")]
        [TestCase("1")]
        public void RfidDetected_Many_CallMethod_ExpextRfidIsRfidAndEventIsFired(string Rfid)
        {
            //Arrange
            //Setup

            //Act
            uut.RfidDetected(Rfid);
            uut.RfidDetected(Rfid);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(receivedEventArgs.Id, Is.EqualTo(Rfid));
                Assert.That(receivedEventArgs, Is.Not.Null);
            });
        }
    }
}
