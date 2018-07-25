using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ga
{
	public class Population<I, P> where I : Individual where P: IProblem<I>
	{
		private List<I> individuals;
		private I best;

		public Population(int size)
		{
			individuals = new List<I>(size);
		}

		public Population(int size, P problem)
		{
			individuals = new List<I>(size);
			for(int i=0; i < size; i++)
			{
				individuals.Add(problem.GetNewIndividual());
			}
		}

		public void AddIndividual(I individual)
		{
			individuals.Add(individual);
		}

		public I GetIndividual(int index)
		{
			return individuals[index];
		}

		public I Evaluate()
		{
			best = GetIndividual(0);
			for(I individual : individuals)
			{
				individual.ComputeFitness();
				if(individual.CompareTo(best) > 0)
				{
					best = individual;
				}
			}

			return best;
		}

		public int GetSize()
		{
			return individuals.Count;
		}
	}

}
