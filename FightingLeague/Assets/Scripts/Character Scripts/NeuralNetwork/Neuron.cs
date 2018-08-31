using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeuralNetwork
{
	public class Neuron
	{
		private double[] x;
		private int y;

		/*contructor
		x => entrada
		y => saida aproximada
		*/
		public Neuron(double[] x, int y)
		{
			int indice = 0;
			this.x = new double[x.Length];
			for (int i = 0; i < x.Length; i++)
			{
				this.x[indice++] = x[i];
			}

			//Entrada negativa para peso TETA
			this.x[indice - 1] = -1;
		}

		public double GetEntrada(int i)
		{
			return x[i];
		}

		public double[] GetInstancia()
		{
			return x;
		}

		public int GetSaida()
		{
			return y;
		}
	}

}
