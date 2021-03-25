using System;
using ChargeStation.Classlibrary;

namespace ChargeStation_Application
{
    class Program
    {
        static void Main(string[] args)
        {
            Door_Simulator door = new Door_Simulator();
            UsbChargerSimulator uSBSimulator = new UsbChargerSimulator();
            IChargeControl chargeControl = new ChargeControl(uSBSimulator, new Display_Simulator());
            IDisplay display = new Display_Simulator();
            ILogfile logfile = new Logfile(new FileWriter(), new TimeProvider());
            IRdfReader rfidReader = new RfidReader_simulator();


            StationControl stationControl = new StationControl(door,chargeControl,rfidReader,display,logfile);
            // Assemble your system here from all the classes

            bool finish = false;
            do
            {
                string input;
                System.Console.WriteLine("Indtast E(Exit), O(OpenDoor), C(CloseDoor), R(Læs RFID), P(Plug), U(UnPlug) , K(OverLoad) , L(NotOverLoad): ");
                input = Console.ReadLine().ToUpper();
                if (string.IsNullOrEmpty(input)) continue;

                switch (input[0])
                {
                    case 'E':
                        finish = true;
                        break;

                    case 'O':
                        door.SimulateOpeningTry();
                        break;

                    case 'C':
                        door.SimulateClosingTry();
                        break;

                    case 'R':
                        System.Console.WriteLine("Indtast RFID id: ");
                        string idString = System.Console.ReadLine();

                        rfidReader.RfidDetected(idString);
                        break;
                    case 'P':
                        uSBSimulator.SimulateConnected(true);
                        break;
                    case 'U':
                        uSBSimulator.SimulateConnected(false);
                        break;
                    case 'K':
                        uSBSimulator.SimulateOverload(true);
                        break;
                    case 'L':
                        uSBSimulator.SimulateOverload(false);
                        break;

                    default:
                        break;
                }

            } while (!finish);
        }
    }
}
