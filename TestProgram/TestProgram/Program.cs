using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            int zahl1;
            int zahl2;

            zahl1 = Convert.ToInt32(Console.ReadLine());
            zahl2 = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine(zahl1 + zahl2);

            Console.ReadKey();
        }
    }
}
