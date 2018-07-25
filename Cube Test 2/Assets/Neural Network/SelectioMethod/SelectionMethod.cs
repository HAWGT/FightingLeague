using ga;

public abstract class SelectionMethod<I,P> where I: Individual where P: IProblem<I>
{

	protected int popSize;

	public SelectionMethod(int popSize)
	{
		this.popSize = popSize;
	}

	public abstract Population<I, P> Run(Population<I, P> original);
}