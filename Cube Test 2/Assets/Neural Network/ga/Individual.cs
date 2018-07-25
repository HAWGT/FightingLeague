using System;

namespace ga
{
	public abstract class Individual<P, I> : IComparable<I> where P : IProblem where I : Individual
	{
		protected double fitness;
		protected P problem;

		public Individual(P problem)
		{
			this.problem = problem;
		}

		public Individual(Individual<P,I> original)
		{
			this.problem = original.problem;
			this.fitness = original.fitness;
		}

		public abstract double ComputeFitness();
		public abstract int GetNumGenes();
		public abstract void SwapGenes(I other, int g);

		public double getFitness()
		{
			return fitness;
		}

		public abstract override I Clone();
		public abstract int CompareTo(I other);
	}
}