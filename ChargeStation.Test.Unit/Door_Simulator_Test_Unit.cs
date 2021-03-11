using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using ChargeStation.Classlibrary;


namespace ChargeStation.Test.Unit
{
    class Door_Simulator_Test_Unit
    {
        private DoorStatusEventArgs _receivedEventArgs;
        private Door_Simulator uut;

        [SetUp]
        public void Setup()
        {
            _receivedEventArgs = null;
            uut = new Door_Simulator();
            uut.DoorStatusChangedEvent +=
                (o, args) =>
                {
                    _receivedEventArgs = args;
                };
        }

        //Disse metoder er testet efter ZOMBIE-princip 

        //Z = Zero
        //O = One
        //M = Multipel
        //B = Boundries
        //I = Interfaces
        //E = Exceptional Behavior

        #region Test LockDoor - ZOMxIx

        [Test]
        public void LuckDoor_Zero_StartesUnLuckedAndClosed_Exppect_False()
        {
            //Arrange 
            //UUT Starter lukket og ulåst

            //Act
            //Der er ikke nogen act da det er en zero test

            //Assert
            Assert.That(uut.DoorIsLocked, Is.False);
        }


        [Test]
        public void LuckDoor_One_StartesUnLuckedAndClosed_Exppect_True()
        {
            //Arrange 

            //UUT Starter lukket og ulåst
            // var uut = new Door_Simulator();

            //Act
            uut.LockDoor();

            //Assert
            Assert.That(uut.DoorIsLocked, Is.True);

        }


        [Test]
        public void LuckDoor_StartesUnLuckedAndClosed_Exppect_True()
        {
            //UUT Starter lukket og ulåst
            //Arrange 


            //Act
            uut.LockDoor();
            uut.LockDoor();

            //Assert
            Assert.That(uut.DoorIsLocked, Is.True);
        }

        #endregion

        #region Test Unluck - ZOMxIx

        [Test]
        public void UnLuckDoor_Zero_StartesLuckedAndClosed_Exppect_True()
        {
            //Arrange 

            //UUT Starter lukket og ulåst
            uut.LockDoor();

            //Act

            //Assert
            Assert.That(uut.DoorIsLocked, Is.True);
        }

        [Test]
        public void UnLuckDoor_One_StartesLuckedAndClosed_Exppect_False()
        {
            //UUT Starter lukket og ulåst
            //Arrange 
            uut.LockDoor();


            //Act
            uut.UnlockDoor();

            //Assert
            Assert.That(uut.DoorIsLocked, Is.False);
        }

        [Test]
        public void UnLuckDoor_Two_StartesLuckedAndClosed_Exppect_False()
        {
            //UUT Starter lukket og ulåst
            //Arrange 
            uut.LockDoor();


            //Act
            uut.UnlockDoor();
            uut.UnlockDoor();

            //Assert
            Assert.That(uut.DoorIsLocked, Is.False);
        }

        #endregion


        #region SimulateOpeningTry - ZOMxxE

        [Test]
        public void SimulateOpeningTry_Zero_StartesUnLuckedAndClosed_ExpectDoorIsClosedAndEventNotFired()
        {
            //UUT Starter  lukket og ulåst
            //Arrange


            //Act
            //No Act da det er en Zero Test

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(uut.DoorIsOpen, Is.False);
                Assert.That(_receivedEventArgs, Is.Null);
            });
        }

        [Test]
        public void SimulateOpeningTry_One_StartesUnLuckedAndClosed_ExpectDoorIsOpenAndEventFired()
        {
            //UUT Starter lukket og ulåst
            //Arrange


            //Act
            uut.SimulateOpeningTry();

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(uut.DoorIsOpen, Is.True);
                Assert.That(_receivedEventArgs, Is.Not.Null);
                Assert.That(_receivedEventArgs.IsOpen, Is.True);
            });
        }

        [Test]
        public void SimulateOpeningTry_Two_StartesUnLuckedAndClosed_ExpectDoorIsOpenAndEventFired()
        {
            //UUT Starter lukket og ulåst
            //Arrange

            //Act
            uut.SimulateOpeningTry();
            uut.SimulateOpeningTry();

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(uut.DoorIsOpen, Is.True);
                Assert.That(_receivedEventArgs, Is.Not.Null);
                Assert.That(_receivedEventArgs.IsOpen, Is.True);
            });
        }

        [Test]
        public void SimulateOpeningTry_ExceptionalBehavior_StartesLuckedAndClosed_ExpectException()
        {
            //UUT Starter lukket og låst
            //Arrange
            uut.LockDoor();

            //Assert
            Assert.Throws<ArgumentException>(() => uut.SimulateOpeningTry());
        }
        #endregion

        #region SimulateCloseingTry - ZOMxxE

        [Test]
        public void SimulateCloseingTry_Zero_StartesUnLuckedAndClosed_ExpectDoorIsClosedAndEventNotFired()
        {
            //UUT Starter lukket og ulåst
            //Arrange


            //Act
            //No Act da det er en Zero Test

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(_receivedEventArgs, Is.Null);
                Assert.That(uut.DoorIsOpen, Is.False);
             });
        }
        [Test]
        public void SimulateCloseingTry_Zero_StartesUnLuckedAndOpen_ExpectDoorIsOpenAndEventNotFired()
        {
            //UUT Starter åbent og ulåst
            //Arrange
            uut.SimulateOpeningTry();


            //Act
            //No Act da det er en Zero Test

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(uut.DoorIsOpen, Is.True);
                Assert.That(_receivedEventArgs, Is.Not.Null);
                Assert.That(_receivedEventArgs.IsOpen, Is.True);
            });
        }

        [Test]
        public void SimulateCloseingTry_One_StartesUnLuckedAndOpen_ExpectDoorIsClosedAndEventFired()
        {
            //UUT Starter åbent og ulåst
            //Arrange
            uut.SimulateOpeningTry();


            //Act
            uut.SimulateClosingTry();

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(uut.DoorIsOpen, Is.False);
                Assert.That(_receivedEventArgs, Is.Not.Null);
                Assert.That(_receivedEventArgs.IsOpen, Is.False);
            });
        }

        [Test]
        public void SimulateCloseingTry_Two_StartesUnLuckedAndOpen_ExpectDoorIsClosedAndEventFired()
        {
            //UUT Starter åbent og ulåst
            //Arrange
            uut.SimulateOpeningTry();


            //Act
            uut.SimulateClosingTry();
            uut.SimulateClosingTry();

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(uut.DoorIsOpen, Is.False);
                Assert.That(_receivedEventArgs.IsOpen, Is.False);
                Assert.That(_receivedEventArgs, Is.Not.Null);
            });
        }


        //Dette er en absur test, da dette ikke burde være muligt
        [Test]
        public void SimulateCloseingTry_Zero_StartesLuckedAndOpen_ExpectNewException()
        {
            //UUT Starter åben og låst,
            //Arrange
            uut.LockDoor();
            uut.DoorIsOpen = true;

            //Act
            //No Act da det er en Zero Test

            //Assert

            Assert.Throws<ArgumentException>(() => uut.SimulateClosingTry());

        }

        #endregion

    }
}
