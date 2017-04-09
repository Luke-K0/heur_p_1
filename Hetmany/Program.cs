using System;
using System.Diagnostics;

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
                n = 42; // domyslnie szachownica 8x8
            }
            
            Stopwatch watch = new Stopwatch();

            watch.Start();
            Solution simAnnealSolution = SimulatedAnnealing.Execute(n);
            watch.Stop();
            simAnnealSolution.Rysuj();
            Console.WriteLine("Czas: {0}s", watch.Elapsed.TotalSeconds);
            Console.ReadLine();

            watch.Restart();
            Solution genetycznySolution = Genetyczny.Execute(n);
            watch.Stop();
            genetycznySolution.Rysuj();
            Console.WriteLine("Czas: {0}s", watch.Elapsed.TotalSeconds);
            Console.ReadLine();
        }
    }
}
