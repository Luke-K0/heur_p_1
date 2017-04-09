using System;

namespace Hetmany
{
    public static class SimulatedAnnealing
    {
        const double Temp0 = 10;
        const double CoolingRate = 0.0003;

        public static Solution Execute(int n)
        {
            var temp = Temp0;

            Solution currentSolution = new Solution(n);
            Solution bestSolution = currentSolution;
            
            int iteracja = 1;
            int bestIteracja = iteracja;

            Console.WriteLine("Annealing...");
            while (temp > 0.1 && bestSolution.Fitness > 0)
            {
                //Console.Write("temp = {0}", temp);
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
                    double random = Solution.Rng.NextDouble();
                    double acceptanceProbability = Math.Exp((currentSolution.Fitness - newSolution.Fitness) / temp);
                    //Console.Write("  delta fitness = {1},    acceptance probability = {0}", acceptanceProbability, currentSolution.Fitness - newSolution.Fitness);
                    if (random < acceptanceProbability)
                    {
                        currentSolution = newSolution;
                    }
                }

                temp = temp * (1 - CoolingRate);
                iteracja++;
                //Console.WriteLine();
            }

            Console.WriteLine("Najlepsza rozwiazanie bylo w iteracji {0}/{1}", bestIteracja, --iteracja);
            return bestSolution;
        }

        private static Solution ModifySolution(Solution solution)
        {
            int n = solution.Length;
            int position1 = Solution.Rng.Next(n);
            int position2 = Solution.Rng.Next(n);

            Solution newSolution = new Solution(solution)
                                   {
                                       [position1] = solution[position2],
                                       [position2] = solution[position1]
                                   };

            newSolution.ReCalculateFitness();

            return newSolution;
        }
    }
}