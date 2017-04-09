using System;
using System.Collections.Generic;
using System.Linq;

namespace Hetmany
{
    public static class Genetyczny
    {
        private const int PopulationCount = 4;
        private const double MutationRate = 0.01;
        private const int MaxGenerations = 1000;

        public static Solution Execute(int n)
        {
            var currentGeneration = GeneratePopulation(n, PopulationCount).ToList();
            currentGeneration.Sort((s1, s2) => s1.Fitness - s2.Fitness);
            int numerPokolenia = 0;

            while (numerPokolenia < MaxGenerations)
            {
                if (currentGeneration.First().Fitness == 0)
                {
                    return currentGeneration.First();
                }

                var newGeneration = new List<Solution>();
                List<Tuple<Solution, Solution>> reproduktorzy = GenerujReproduktorow(currentGeneration);
                foreach (var para in reproduktorzy)
                {
                    Tuple<Solution, Solution> potomkowie = CrossOver(para);
                    potomkowie.Item1.Mutuj(MutationRate);
                    potomkowie.Item2.Mutuj(MutationRate);

                    newGeneration.Add(potomkowie.Item1);
                    newGeneration.Add(potomkowie.Item2);
                }

                newGeneration.Sort((s1, s2) => s1.Fitness - s2.Fitness);
                currentGeneration = newGeneration;
                numerPokolenia++;
            }

            return currentGeneration.First();
        }

        private static Tuple<Solution, Solution> CrossOver(Tuple<Solution, Solution> para)
        {
            var child1 = new Solution(para.Item1.Length);
            var child2 = new Solution(para.Item2.Length);

            for (int i = 0; i < child1.Length; i++)
            {
                if (i < child1.Length / 2)
                {
                    child1[i] = para.Item1[i];
                    child2[i] = para.Item2[i];
                }
                else
                {
                    child1[i] = para.Item2[i];
                    child2[i] = para.Item1[i];
                }
            }

            child1.ReCalculateFitness();
            child2.ReCalculateFitness();

            return new Tuple<Solution, Solution>(child1, child2);
        }

        private static List<Tuple<Solution, Solution>> GenerujReproduktorow(List<Solution> population)
        {
            int targetCount = population.Count / 2;
            var result = new List<Tuple<Solution, Solution>>();
            while (result.Count < targetCount)
            {
                result.Add(new Tuple<Solution, Solution>(LosujReproduktora(population), LosujReproduktora(population)));
            }

            return result;
        }

        private static Solution LosujReproduktora(List<Solution> population)
        {
            double maxFitness = population.Last().Fitness;
            double sumOfAllFitness = population.Sum(s => s.Fitness);

            var progi = new double[population.Count];
            double offset = 0;

            for (int i = 0; i < population.Count; i++)
            {
                double procent = (population[i].Fitness / sumOfAllFitness);
                progi[i] = offset;
                offset += procent;
            }

            int index = progi.Length - 1;
            double random = Solution.Rng.NextDouble();
            for (int i = 0; i < progi.Length - 1; i++)
            {
                if (progi[i] <= random && progi[i+1] >= random)
                {
                    index = i;
                    break;
                }
            }

            Console.WriteLine("Wylosowalem reproduktora nr {0}, fitness {1}", index, population[index].Fitness);
            return population[index];
        }

        private static IEnumerable<Solution> GeneratePopulation(int n, int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new Solution(n);
            }
        }
    }
}