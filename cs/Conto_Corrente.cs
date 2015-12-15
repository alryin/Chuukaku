using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.SystemException;

namespace ConsoleApplication2
{
    class ContoCorrente
    {
        public ContoCorrente() { conto = 10000; }

        private double conto;

        public void Preleva(double n){
            string s = "Sul conto hai " + conto + " euro";

            if (n <= conto)
            {
                conto = conto - n;
                s = s + "\nHai prelevato: " + n + " euro";
                s = s + "\nTi rimangono sul conto: " + conto + " euro\n";
                Console.WriteLine(s);
            }
        }

        public void Deposita(double n) 
        {
            string s = "Sul conto hai " + conto + " euro";
            conto = conto + n;
            s = s + "\nHai depositato: " + n + " euro";
            s = s + "\nTi rimangono sul conto: " + conto + " euro\n";
            Console.WriteLine(s);
        }
    }


    class Chiedi
    {
        public static void Main(String[] args) 
        {
            ContoCorrente c1 = new ContoCorrente();
            Boolean b = true;

            while(b)
            {
                Console.WriteLine("Premere 1 se si vuole prelevare");
                Console.WriteLine("Premere 2 se si vuole depositare");
                Console.WriteLine("Premere 3 per uscire");

                int n = Int32.Parse(Console.ReadLine());

                switch (n) 
                {
                    case 1: Console.Write("Inserisci il valore da prelevare: ");
                              c1.Preleva(Int32.Parse(Console.ReadLine()));
                              break;
                    case 2: Console.Write("Inserisci il valore da depositare: ");
                            c1.Deposita(Int32.Parse(Console.ReadLine()));
                            break;
                    case 3: b = false;
                            break;
                    default: Console.WriteLine("Errore");
                            break;
                            
                }
           

            }

        }
    }
}
