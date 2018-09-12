using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeuralNetwork
{
	public class NeuronExamples
	{
		private List<Neuron> examples;
		double currentHPDif;
		double bar;
		double enBar;


		public NeuronExamples()
		{
			examples = new List<Neuron>();

			examples.Add(new Neuron(new double[] { 10000, 10000, 0, 0, 7, 0, 0, 0, 0, 0, 0 }, 1));
			examples.Add(new Neuron(new double[] { 5000, 3000, 0, 80, 4, 0, 0, 0, 0, 1, 1 }, 2));
			examples.Add(new Neuron(new double[] { 5000, 7000, 40, 45, 2, 0, 0, 0, 0, 0 }, 6));
			examples.Add(new Neuron(new double[] { 7000, 4000, 60, 30, 5, 0, 0, 0, 1, 1 }, 9));
			examples.Add(new Neuron(new double[] { 5000, 7000, 20, 60, 2.5, 0, 0, 0, 1, 1 }, 13));
			examples.Add(new Neuron(new double[] { 2000, 1500, 0, 100, 0.5, 0, 0, 0, 1, 0 }, 12));
		}


		public Neuron SearchExample(double differenceHP, double selfBar, double distanceX, double differenceHeight, int enemyBusy)
		{
			Neuron toSend = examples[0];
			UpdateExample(toSend);
			foreach ( Neuron neuron in examples)
			{
				if(differenceHP <= 0)
				{
					//ser agressivo
					if(distanceX> 1.3)
					{
						//aproximar
						return examples[0];
					}

					if(selfBar > 60)
					{
						//vanish/teleport + super
						return examples[5];
					}

				}else if (differenceHP > 0) { 
					//ser defensivo
					if (selfBar > 50)
					{
						//super
						return examples[3];
					}
					else
					{
						//meter distancia e andar para trás
						return examples[1];
					}
				}
			}
			return toSend;
		}

		private void UpdateExample(Neuron toSend)
		{
			currentHPDif = toSend.GetDiffHP();
			bar = toSend.GetBarPlayer();
			enBar = toSend.GetBarEnemy();
		}


	}

}