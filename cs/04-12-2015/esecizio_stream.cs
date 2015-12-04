using System;
using System.IO;

//esercizio svolto da me; non ho idea di ciò che è stato spiegato in laboratorio [Ryick]

namespace Esercizi_Stream
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamWriter writer =
                new StreamWriter("file.txt")) // inserire path completa
            {
                writer.WriteLine("S'i' fosse foco [1956]");
                writer.WriteLine("");
                writer.WriteLine("S'i' fosse foco, ardere' il mondo;");
                writer.WriteLine("s'i' fosse vento, lo tempestarei;");
                writer.WriteLine("s'i' fosse acqua, i' l'annegherei;");
                writer.WriteLine("s'i' fosse Dio, mandereil'en profondo;");
                writer.WriteLine("");
                writer.WriteLine("s'i' fosse papa, serei allor giocondo,");
                writer.WriteLine("ché tutti cristïani embrigarei;");
                writer.WriteLine("s'i' fosse 'mperator, sa' che farei?");
                writer.WriteLine("a tutti mozzarei lo capo a tondo.");
                writer.WriteLine("");
                writer.WriteLine("S'i' fosse morte, andarei da mio padre;");
                writer.WriteLine("s'i' fosse vita, fuggirei da lui:");
                writer.WriteLine("similemente faria da mi' madre,");
                writer.WriteLine("");
                writer.WriteLine("S'i' fosse Cecco, com'i' sono e fui,");
                writer.WriteLine("torrei le donne giovani e leggiadre:");
                writer.WriteLine("le vecchie e laide lasserei altrui.");
                writer.WriteLine("");
                writer.WriteLine("(Cecco Angiolieri)");
            }

            string line;

            using (StreamReader reader =
                new StreamReader("file.txt")) // inserire path completa
            {
                line = reader.ReadToEnd();
            }
            Console.WriteLine(line);
        }

    }
}
