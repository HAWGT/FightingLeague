namespace ga
{
	public interface IProblem<E> where E: Individual
	{
		E GetNewIndividual();
	}
}