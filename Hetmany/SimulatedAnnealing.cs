using System;

namespace Hetmany
{
    public static class SimulatedAnnealing
    {
        public static Solution Execute(int n, double temp0, double coolingRate)
        {
            var temp = temp0;

            Solution currentSolution = new Solution(n);
            Solution bestSolution = currentSolution;
            
            int iteracja = 1;
            int bestIteracja = iteracja;

            while (temp > 0.1 && bestSolution.Fitness > 0)
            {
                Console.Write("temp = {0}", temp);
                Solution newSolution = ModifySolution(currentSolution);
                
                if (newSolution.Fitness <= currentSolution.Fitness)
                {
                    currentSolution = newSolution;
                    if (currentSolution.Fitness < bestSolution.Fitness)
                    {
                        bestSolution = currentSolution;
                        bestIteracja = iteracja;
                    }
                }
                else
                {
                    var random = Solution.Rng.Next(1);
                    double acceptanceProbability = Math.Exp((currentSolution.Fitness - newSolution.Fitness) / temp);
                    Console.Write("  delta fitness = {1},    acceptance probability = {0}", acceptanceProbability, currentSolution.Fitness - newSolution.Fitness);
                    if (random < acceptanceProbability)
                    {
                        currentSolution = newSolution;
                    }
                }

                temp = temp * (1 - coolingRate);
                iteracja++;
                Console.WriteLine();
            }

            Console.WriteLine("Najlepsza rozwiazanie bylo w iteracji {0}/{1}", bestIteracja, --iteracja);
            return bestSolution;
        }

        private static Solution ModifySolution(Solution solution)
        {
            int n = solution.Length;
            int position1 = Solution.Rng.Next(n);
            int position2 = Solution.Rng.Next(n);
            
            Solution newSolution = new Solution(solution);

            newSolution[position1] = solution[position2];
            newSolution[position2] = solution[position1];

            newSolution.ReCalculateFitness();

            return newSolution;
        }
    }
}