using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ga
{
	public class GeneticAlgorithm<I, P> where I: Individual where P: IProblem<I>
	{

		private Random random;
		private int popSize;
		private int numSaidas;
		private int maxGenerations;

		private Tournament<I,P> selectionTournament;
		private Recombination recombination;
		private Mutation mutation;
		private int generationIteration;
		private Population<I,P> population;
		private bool stopped;
		private I bestInRun;


		public GeneticAlgorithm(int popSize, int maxGenerations, Tournament tournamet, Recombination recombination, Mutation mutation, Random rand)
		{
			random = new Random();
			this.popSize = popSize;
			this.maxGenerations = maxGenerations;
			this.selectionTournament = tournamet;
			this.recombination = recombination;
			this.mutation = mutation;			
		}

		public I Run(IProblem problem)
		{
			population = new Population<>(popSize, problem);
			bestInRun = population.Evaluate();
		}
	}
	
}