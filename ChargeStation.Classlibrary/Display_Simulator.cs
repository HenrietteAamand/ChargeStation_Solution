using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;

namespace ChargeStation.Classlibrary
{
    public class Display_Simulator : IDisplay
    {
        public void ChangeText(string indlæsRfid)
        {
            Console.WriteLine(indlæsRfid);
        }

        public void ChangeText(MessageCode text)
        {
            switch (text)
            {
                case MessageCode.FjernTelefon:
                    Console.WriteLine("Fjern telefon");
                    break;
                case MessageCode.IndlaesRFID:
                    Console.WriteLine("Indlæs RFID");
                    break;
                case MessageCode.LadeskabOptaget:
                    Console.WriteLine("Ladeskab optaget");
                    break;
                case MessageCode.RFIDFejl:
                    Console.WriteLine("RFID fejl");
                    break;
                case MessageCode.TilslutTelefon:
                    Console.WriteLine("Tilslut telefon");
                    break;
                case MessageCode.Tilslutningsfejl:
                    Console.WriteLine("Tilslutningsfejl");
                    break;
                case MessageCode.TelefonFuldtOpladet:
                    System.Console.WriteLine("Telefonen er fuldt opladet");
                    break;
                case MessageCode.LadningIgang:
                    System.Console.WriteLine("Opladning er igang");
                    break;
                case MessageCode.Kortslutning:
                    System.Console.WriteLine("Systemet er kortsluttet");
                    break;
                default:
                    Console.WriteLine("Unknown Error");
                    break;

            }
        }
    }
}
