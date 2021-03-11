using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using ChargeStation.Classlibrary;


namespace ChargeStation.Test.Unit
{
    class Door_Simulator_Test_Unit
    {
        //Disse metoder er testet efter ZOMBIE-princip 

        #region Test LockDoor - ZOMxxx

        [Test]
        public void LuckDoor_Zero_StartesUnLuckedAndClosed_Exppect_False()
        {
            //Arrange 

            //Start lukket og ulåst
            var uut = new Door_Simulator();

            //Act
            //Der er ikke nogen act da det er en zero test

            //Assert
            Assert.That(uut.DoorIsLocked, Is.False);
        }


        [Test]
        public void LuckDoor_One_StartesUnLuckedAndClosed_Exppect_True()
        {
            //Arrange 

            //Start lukket og ulåst
            var uut = new Door_Simulator();

            //Act
            uut.LockDoor();

            //Assert
            Assert.That(uut.DoorIsLocked, Is.True);
        }


        [Test]
        public void LuckDoor_StartesUnLuckedAndClosed_Exppect_True()
        {
            //Arrange 

            //Start lukket og ulåst
            var uut = new Door_Simulator();

            //Act
            uut.LockDoor();
            uut.LockDoor();

            //Assert
            Assert.That(uut.DoorIsLocked, Is.True);
        }

        #endregion

        #region Test Unluck - ZOMxxx

        [Test]
        public void UnLuckDoor_Zero_StartesLuckedAndClosed_Exppect_True()
        {
            //Arrange 

            //Start lukket og ulåst
            var uut = new Door_Simulator();
            uut.LockDoor();

            //Act
            
            //Assert
            Assert.That(uut.DoorIsLocked, Is.True);
        }

        [Test]
        public void UnLuckDoor_One_StartesLuckedAndClosed_Exppect_False()
        {
            //Arrange 

            //Start lukket og ulåst
            var uut = new Door_Simulator();
            uut.LockDoor();

            //Act
            uut.UnlockDoor();

            //Assert
            Assert.That(uut.DoorIsLocked, Is.False);
        }

        [Test]
        public void UnLuckDoor_Two_StartesLuckedAndClosed_Exppect_False()
        {
            //Arrange 

            //Start lukket og ulåst
            var uut = new Door_Simulator();
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
        public void SimulateOpeningTry_Zero_StartesUnLuckedAndClosed()
        {
            //Arrange

            //Døre starter lukket og ulåst
            var uut = new Door_Simulator();

            //Act
            //No Act da det er en Zero Test

            //Assert
            Assert.That(uut.DoorIsOpen, Is.False);
        }

        [Test]
        public void SimulateOpeningTry_One_StartesUnLuckedAndClosed()
        {
            //Arrange

            //Start lukket og ulåst
            var uut = new Door_Simulator();


            //Act
            uut.SimulateOpeningTry();

            //Assert
            Assert.That(uut.DoorIsOpen, Is.True);
        }

        [Test]
        public void SimulateOpeningTry_Two_StartesUnLuckedAndClosed()
        {
            //Arrange

            //Start lukket og ulåst
            var uut = new Door_Simulator();


            //Act
            uut.SimulateOpeningTry();
            uut.SimulateOpeningTry();

            //Assert
            Assert.That(uut.DoorIsOpen, Is.True);
        }

        [Test]
        public void SimulateOpeningTry_ExceptionalBehavior_StartesLuckedAndClosed()
        {
            //Arrange

            //Start lukket og låst
            var uut = new Door_Simulator();
            uut.LockDoor();

            Assert.Throws<ArgumentException>(() => uut.SimulateOpeningTry());
            //Assert
        }
        #endregion

        private DoorStatusEventArgs _receivedEventArgs;
        private IDoor uut;
        [SetUp]
        public void Setup()
        {
            uut = new Door_Simulator();
            uut.DoorStatusChangedEvent +=
                (o, args) =>
                {
                    _receivedEventArgs = args;
                };
        }


    }
}
