using System;
using System.Collections.Generic;
using System.Linq;

namespace Hetmany
{
    public class Solution
    {
        public static readonly Random Rng = new Random();

        private readonly int[] solution;

        public Solution(int n)
        {
            solution = RandomSolution(n);
            ReCalculateFitness();
        }

        public Solution(Solution other)
        {
            solution = new int[other.Length];
            other.solution.CopyTo(solution, 0);
            ReCalculateFitness();
        }

        public int this[int index]
        {
            get
            {
                return solution[index];
            }
            set
            {
                solution[index] = value;
            }
        }

        public int Length { get { return solution.Length; } }

        public int Fitness { get; private set; }

        public void ReCalculateFitness()
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

            Fitness = errors;
        }

        private static int[] RandomSolution(int n)
        {
            var randomSolution = GenerateSequence(n).ToArray();
            int x = n;

            while (x > 1)
            {
                x--;
                int k = Rng.Next(x + 1);
                int value = randomSolution[k];
                randomSolution[k] = randomSolution[x];
                randomSolution[x] = value;
            }

            return randomSolution;
        }

        public void Rysuj()
        {
            Console.WriteLine();
            Console.WriteLine("Ostateczny fitness = {0}", Fitness);
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

        private static IEnumerable<int> GenerateSequence(int length)
        {
            for (int i = 0; i < length; i++)
            {
                yield return i;
            }
        }

        private static bool CheckQueenPair(int i, int qi, int j, int qj)
        {
            return (i - qi == j - qj) || (i + qi == j + qj);
        }
    }
}