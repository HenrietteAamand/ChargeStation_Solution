using ChargeStation.Classlibrary;
using NSubstitute;
using NUnit.Framework;

namespace ChargeStation.Test.Unit
{
    public class ChargeControl_Test_Unit
    {
        private IChargeControl uut;

        private UsbChargerSimulator usbCharger;

        private CurrentEventArgs currentEventArgs;
        //private readonly TestUSBCharcgerSource testUsbCharger;

        [SetUp]
        public void Setup()
        {
            usbCharger = Substitute.For<UsbChargerSimulator>();
            uut = new ChargeControl(usbCharger);

        }


    }




}
