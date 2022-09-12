/*********************************************************************
* Simple sample C# selection, showing stdin, stdout, static, cetera
Ben Elpidius, September 9, 2022
Harry Plantinga, 9/2011
*
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fib
{
  class Program
  {
    static void Main(string[] args)
    {
      while (true)
      {
        // Calculates fibonnaci number
        Console.WriteLine("Fantastic Fibonnaci X Finder!");

        Console.Write("\nEnter X: ");
        long x = long.Parse(Console.ReadLine());
        long fib = Fib(x);
        Console.WriteLine("Fib({0}) = {1}.", x, fib);

        // Calculates lg(lg(n)), with base 2
        Console.WriteLine("Fantastic Log(Log N) base 2 Finder!");

        Console.WriteLine("Enter N: ");
        long n = long.Parse(Console.ReadLine()); // Reads user input for n
        long log = CalculateLog(CalculateLog(n)); // Calls the function to calculate log twice
        Console.WriteLine("Floor of Lg(lg({0})) = {1}.", n, log); // Outputs in format string

        // Ask the user to continue or quit
        Console.WriteLine("Press anything other than 'Q' to continue. Press 'Q' to quit.");
        if (Console.ReadLine().ToUpper() == "Q")
        {
          break;
        }
      }
    }

    // Fibonnaci Finder
    static long Fib(long x)
    {
      if (x <= 2)
        return 1;
      else
        return Fib(x - 1) + Fib(x - 2);
    }

    // Log base 2 Finder
    static long CalculateLog(long n)
    {
      long log = n / 2;
      int exponent = 0;

      // Converts n into power of 2 by increment the exponent by 1 everytime n is divisible by 2.
      // Returns when n is less than 1.
      for (long i = log; i >= 1; i = i / 2)
      {
        ++exponent;
      }

      return exponent;
    }
  }
}