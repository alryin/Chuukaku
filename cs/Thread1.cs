using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ConsoleApplication1
{
  class program
  {
    static void Main(string[] args)
      {
        c1 c = new c1();
        Thread t1 = new Thread(new ParameterizedThreadStart(c.ThreadCode));
        Thread t2 = new Thread(new ParameterizedThreadStart(c.ThreadCode));
        Thread t3 = new Thread(new ParameterizedThreadStart(c.ThreadCode));
        Thread t4 = new Thread(new ParameterizedThreadStart(c.ThreadCode));
        Thread t5 = new Thread(new ParameterizedThreadStart(c.ThreadCode));
        Thread t6 = new Thread(new ParameterizedThreadStart(c.ThreadCode));
        Thread t7 = new Thread(new ParameterizedThreadStart(c.ThreadCode));
        Thread t8 = new Thread(new ParameterizedThreadStart(c.ThreadCode));
        Thread t9 = new Thread(new ParameterizedThreadStart(c.ThreadCode));

        t1.Start("Thread 1");
        t2.Start("Thread 2");
        t3.Start("Thread 3");
        t4.Start("Thread 4");
        t5.Start("Thread 5");
        t6.Start("Thread 6");
        t7.Start("Thread 7");
        t8.Start("Thread 8");
        t9.Start("Thread 9");
      }
    }

    class c1 
    {
      Random r = new Random();
      
      public void ThreadCode(object caller) 
      {
        while (true)
          {
            string par = (string)caller;
            int rand = r.Next(1000, 3000);
            Console.WriteLine(par);
            Thread.Sleep(rand);
          }
      }
  }
}
