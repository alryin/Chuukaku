using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{

    enum livello { veryLow = 1, low, medium, high, highest }

    class Program
    {

        string nome;
        string cognome;
        int matricola;
        livello level;

        public Program(string nome, string cognome, int matricola, livello level)
        {
            this.nome = nome;
            this.cognome = cognome;
            this.matricola = matricola;
            this.level = level;
        }

        public void setNome(string nome){ this.nome = nome; }
        public void setCognome(string cognome) { this.cognome = cognome; }
        public void setMatricola(int matricola) { this.matricola = matricola; }

        public string getNome() { return nome; }
        public string getCognome() { return cognome; }
        public int getMatricola() { return matricola; }

        public override string ToString()
        {
            string generali = "nome: " + nome + "\ncognome: " + cognome + "\nMatricola: " + matricola +
                "\nLivello Di Sicurezza: " + level ;
            return generali;
        }

        static void Main(string[] args)
        {

            Program p1 = new Program("E", "Mess", 0004, livello.veryLow);
            Console.WriteLine(p1.ToString());
        }
    }
}
