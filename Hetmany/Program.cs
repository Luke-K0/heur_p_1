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
                n = 10; // domyslnie szachownica 8x8
            }

            double temp;
            string userTemp = args.Length > 1 ? args[1] : null;
            if (!double.TryParse(userTemp, out temp))
            {
                temp = 1000; // domyslnie aż 1000 stopniów !!!
            }

            double coolingRate;
            string userCoolingRate = args.Length > 2 ? args[2] : null;
            if (!double.TryParse(userCoolingRate, out coolingRate))
            {
                coolingRate = 0.0001; 
            }

            RysujMnieTuSolucje(SimulatedAnnealing(n, temp, coolingRate));
            Console.ReadLine();
        }

        private static int[] SimulatedAnnealing(int n, double temp0, double coolingRate)
        {
            var temp = temp0;

            int[] currentSolution = RandomSolution(n);
            int[] bestSolution = currentSolution;

            int currentFitness = Fitness(currentSolution);
            int bestFitness = Fitness(currentSolution);

            int iteracja = 1;
            int bestIteracja = iteracja;

            while (temp > 1 && bestFitness > 0)
            {
                Console.WriteLine("temp = {0}", temp);
                int[] newSolution = ModifySolution(currentSolution);
                int newFitness = Fitness(newSolution);
                
                if (newFitness < currentFitness)
                {
                    currentSolution = newSolution;
                    currentFitness = newFitness;
                    if (currentFitness < bestFitness)
                    {
                        bestFitness = currentFitness;
                        bestSolution = currentSolution;
                        bestIteracja = iteracja;
                    }
                }
                else
                {
                    var random = Rng.Next(1);
                    if (random < Math.Exp((newFitness - currentFitness) / temp))
                    {
                        currentSolution = newSolution;
                        currentFitness = newFitness;
                    }
                }

                temp = temp * (1 - coolingRate);
                iteracja++;
            }

            Console.WriteLine("Najlepsza rozwiazanie bylo w iteracji {0}/{1}", bestIteracja, --iteracja);
            return bestSolution;
        }

        private static int[] ModifySolution(int[] solution)
        {
            int n = solution.Length;
            int position1 = Rng.Next(n);
            int position2 = Rng.Next(n);

            int[] newSolution = new int[n];
            solution.CopyTo(newSolution, 0);

            newSolution[position1] = solution[position2];
            newSolution[position2] = solution[position1];

            return newSolution;
        }

        private static void RysujMnieTuSolucje(int[] solution)
        {
            Console.WriteLine();
            Console.WriteLine("Ostateczny fitness = {0}", Fitness(solution));
            Console.WriteLine();
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
