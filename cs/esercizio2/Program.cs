using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esercizio2
{
    class Program
    {
        static void Main(string[] args)
        {
            var e = new Echo();
            Echo e1 = new Echo();
            e.call("welcome, stranger!");
            e.peek();
            e.obsoleto();
            Echo.funzionedebug();
            System.Threading.Thread.Sleep(4242);

        }
  
    }

    public class Echo {
        private string s;
        int i;
    public void call(String s) {
        this.s = s; }

    public void peek() {
            Console.WriteLine(this.s); }

        [Obsolete("don't use it")]
        public void obsoleto()
        {
            Console.WriteLine("obsoleto");
        }

        [Conditional("DEBUG")]
        public static void funzionedebug()
        {
            Console.WriteLine("solo in debug");
        }
    }

    

}
