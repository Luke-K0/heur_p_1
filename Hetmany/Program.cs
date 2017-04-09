using System;

namespace Hetmany
{
    class Program
    {
        static void Main(string[] args)
        {
            int n;
            string userN = args.Length > 0 ? args[0] : null;
            if (!int.TryParse(userN, out n))
            {
                n = 12; // domyslnie szachownica 8x8
            }

            double temp;
            string userTemp = args.Length > 1 ? args[1] : null;
            if (!double.TryParse(userTemp, out temp))
            {
                temp = 10; // domyslnie aż 1000 stopniów !!!
            }

            double coolingRate;
            string userCoolingRate = args.Length > 2 ? args[2] : null;
            if (!double.TryParse(userCoolingRate, out coolingRate))
            {
                coolingRate = 0.0002; 
            }

            Solution simAnnealSolution = SimulatedAnnealing.Execute(n, temp, coolingRate);
            simAnnealSolution.Rysuj();
            Console.ReadLine();
        }
    }
}
