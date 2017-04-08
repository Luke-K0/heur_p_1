using System;
using System.Collections.Generic;
using System.Linq;

namespace Hetmany
{
    class Program
    {

        private static readonly Random Rng = new Random();

        static void Main(string[] args)
        {
            int n;
            string userN = args.Length > 0 ? args[0] : null;
            if (!int.TryParse(userN, out n))
            {
                n = 8; // domyslnie szachownica 8x8
            }

            int fitness = int.MaxValue;
            int[] solution = RandomSolution(n);
            int i = 1;
            while (fitness > 0 || i > 1000000)
            {
                Console.WriteLine("Iteracja {0}", i);
                solution = RandomSolution(n);
                fitness = Fitness(solution);
                i++;
            }

            RysujMnieTuSolucje(solution);
            Console.ReadLine();
        }

        private static void RysujMnieTuSolucje(int[] solution)
        {
            for (int i = 0; i < solution.Length; i++)
            {
                for (int j = 0; j < solution.Length; j++)
                {
                    if (solution[i] == j)
                    {
                        Console.Write("[H]");
                    }
                    else
                    {
                        Console.Write("[_]");
                    }
                }
                Console.WriteLine();
            }
        }

        private static int[] RandomSolution(int n)
        {
            var solution = GenerateSequence(n).ToArray();
            int x = n;

            while (x > 1)
            {
                x--;
                int k = Rng.Next(x + 1);
                int value = solution[k];
                solution[k] = solution[x];
                solution[x] = value;
            }

            return solution;
        }

        private static IEnumerable<int> GenerateSequence(int length)
        {
            for (int i = 0; i < length; i++)
            {
                yield return i;
            }
        }

        private static int Fitness(int[] solution)
        {
            int errors = 0;
            for (int i = 0; i < solution.Length - 1; i++)
            {
                for (int j = i + 1; j < solution.Length; j++)
                {
                    if (CheckQueenPair(i, solution[i], j, solution[j]))
                    {
                        errors++;
                    }
                }
            }

            return errors;
        }

        private static bool CheckQueenPair(int i, int qi, int j, int qj)
        {
            return (i - qi == j - qj) || (i + qi == j + qj);
        }
    }
}
