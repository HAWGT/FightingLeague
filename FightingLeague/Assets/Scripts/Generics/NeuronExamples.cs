using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeuralNetwork
{
	public class NeuronExamples : MonoBehaviour
	{
		private List<Neuron> examples;
		double currentHPDif;
		double bar;
		double enBar;

		private void Start()
		{
			examples.Add(new Neuron(new double[] { 10000, 10000, 0, 0, 7, 0, 0, 0, 0, 0, 0}, 1));
			examples.Add(new Neuron(new double[] { 5000, 3000, 0, 80, 4, 0, 0, 0, 0, 1, 1 }, 2));
			examples.Add(new Neuron(new double[] { 5000, 7000, 40, 45, 2, 0, 0, 0, 0, 0 }, 6));
			examples.Add(new Neuron(new double[] { 7000, 4000, 60, 30, 5, 0, 0, 0, 1, 1 }, 9));
			examples.Add(new Neuron(new double[] { 5000, 7000, 20, 60, 2.5, 0, 0, 0, 1, 1 }, 13));
			examples.Add(new Neuron(new double[] { 2000, 1500, 0, 100, 0.5, 0, 0, 0, 1, 0 }, 12));
		}

		public Neuron SearchExample(int differenceHP, int selfBar, float distanceX, int differenceHeight, bool enemyBusy, bool enemyIsAttacking, int time)
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
					}

					if(selfBar > 60)
					{
						//vanish/teleport + super
					}

				}else if (differenceHP > 0) { 
					//ser defensivo
					if (selfBar > 50)
					{
						//super
					}
					else
					{
						//meter distancia e andar para trás
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