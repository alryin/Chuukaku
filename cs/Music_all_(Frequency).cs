using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ConsoleApplication1
{
  class Program
  {
    static void Main(string[] args)
    {
      int j = 0;
      for (int i = 37; i <= 32767; i += 200)
        {
          j += 200;
          Console.Beep(i, j);
        }
    }
  }
}
