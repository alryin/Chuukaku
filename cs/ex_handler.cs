using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Esercizio
{
  delegate void Op(double d1, double d2);
  
 class MathOpClass
  {
    private Op _del;

    public MathOpClass() 
    {
       _del = new Op(Add);
      _del += new Op(Sub);
      _del += new Op(Multiply);
    }
    
    private static void Add(double d1, double d2) { Console.WriteLine((d1 + d2) + " = " + d1 + " + " + d2); }

    private static void Sub(double d1, double d2) { Console.WriteLine((d1 - d2) + " = " + d1 + " - " + d2); }

    private static void Multiply(double d1, double d2) { Console.WriteLine((d1 * d2) + " = " + d1 + " * " + d2); }

    public void CallOp(double d1, double d2){ _del(d1,d2); }
  }

  class Program
  {
    public static void Main(String[] arg) 
    {
      MathOpClass mc = new MathOpClass();
      mc.CallOp(13, 14);
    }
  }
}
