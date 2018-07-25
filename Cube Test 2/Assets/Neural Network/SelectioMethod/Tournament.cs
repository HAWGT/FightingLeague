using ga;

public class Tournament<I, P> : SelectionMethod<I,P> where I:Individual where P: IProblem<I>{

	private int size;

	public Tournament(int popSize)
	{
		this(popSize, 2);
	}

	public Tournament(int popSize, int size) :base(popSize)
	{
		this.size = size;
	}

	public override Population<I,P> Run(Population<I,P> original)
	{
		Population<I, P> result = new Population<>(original.GetSize());

		for (int i = 0; i < popSize; i++)
		{
			result.AddIndividual(Tournament(original));
		}
		return result;
	}

	public I Tournament(Population<I,P> population)
	{
		I best = population.GetIndividual(GeneticAlgorithm.random.nextInt(popSize));

		for(int i=1; i < size; i++)
		{
			I aux = population.GetIndividual(GeneticAlgorithm.random.nextInt(popSize));
			if (aux.CompareTo(best) > 0)
				best = aux;
		}

		return (I)best.Clone();
	}
}
