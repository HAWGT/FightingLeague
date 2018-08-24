using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeuralNetwork
{
	public class FF2Layer
	{

		private System.Random aleatorio;

		private int numEntradas;
		private int tamanhoCamada1;
		private int numSaidas;

		private double alfa;
		private double limiteErro;

		/*Matriz de pesos da primeira camada.*/
		private double[,] weights1; //w1[numEntradas + 1][tamanhoCamada1];
									/* Matriz de pesos da segunda camada.*/
		private double[] weights2; //w2[tamanhoCamada1 + 1][numSaidas];
								   /*Vetor com o valor de activacao dos nos da primeira camada.*/
		private double[] activation1; //a1[tamanhoCamada1 + 1];
									  /* Vetor com o valor de activacao dos nos da camada de saida.*/
		private double activationHidden; //o(único);
										 /* Vetor com os erros nos nós da camada escondida*/
		private double[] erroEscondida; //erroEscondida[tamanhoCamada1 + 1]
										/*Vetor com os erros de cada uma das saidas.*/
		private double erroSaida; // erroSaida(unico);

		private int interacoesMAX = 10000;

		public FF2Layer(int numEntradas, int tamanhoCamada1, int numSaidas, int seed)
		{
			this.numEntradas = numEntradas;
			this.tamanhoCamada1 = tamanhoCamada1;
			this.numSaidas = numSaidas;
			this.aleatorio = new System.Random(seed);
			this.erroSaida = new System.Random(seed).NextDouble();

			weights1 = new double[numEntradas + 1, tamanhoCamada1];
			weights2 = new double[tamanhoCamada1 + 1];
			activation1 = new double[tamanhoCamada1 + 1];
			erroEscondida = new double[tamanhoCamada1 + 1];

		}

		public void TreinoRede(Neuron[] conjuntoTreino, Neuron[] conjuntoTeste, double alfa, double limiteErro)
		{
			this.alfa = alfa;
			this.limiteErro = limiteErro;

			//Iniciação dos pesos

			for (int i = 0; i < numEntradas + 1; i++)
			{
				for (int j = 0; j < tamanhoCamada1; j++)
				{
					weights1[i, j] = (aleatorio.Next(2) == 0 ? aleatorio.NextDouble() : -aleatorio.NextDouble());
				}
			}

			for (int i = 0; i < tamanhoCamada1 + 1; i++)
			{
				weights2[i] = (aleatorio.Next(2) == 0 ? aleatorio.NextDouble() : -aleatorio.NextDouble());
			}

			//Treino da rede
			int iteration = 0;
			while (!CondicaoParagem(conjuntoTreino, conjuntoTeste) || iteration < interacoesMAX)
			{
				for (int i = 0; i < conjuntoTreino.Length; i++)
				{
					ForwardPropagation(conjuntoTreino[i].GetInstancia());
					BackPropagation(conjuntoTreino[i]);
				}
				iteration++;

			}
		}

		private bool CondicaoParagem(Neuron[] conjuntoTreino, Neuron[] conjuntoTeste)
		{
			for (int e = 0; e < conjuntoTreino.Length; e++)
			{
				ForwardPropagation(conjuntoTreino[e].GetInstancia());
				///re-escrever condição
				double forAbs = activationHidden - conjuntoTreino[e].GetSaida();
				if (activationHidden <= 0.5)
				{
					return false;
				}
			}
			return true;
		}

		//calcula o resultado de uma rede para uma determinada instancia
		private double ForwardPropagation(double[] instancia)
		{
			double soma = 0;

			//calculo da primeira camada
			for (int i = 0; i < tamanhoCamada1; i++)
			{
				for (int j = 0; j < numEntradas; j++)
				{
					soma += weights1[j, i] * instancia[j];
				}

				activation1[i] = SigmoidFn.Output(soma + erroSaida); // 1 / (1 + Math.Pow(Math.E, -soma))
			}
			activation1[tamanhoCamada1] = -1;

			soma = 0;

			//calculo da camada de saída

			for (int j = 0; j < tamanhoCamada1 + 1; j++)
			{
				soma += activation1[j] * weights2[j];
			}
			activationHidden = SigmoidFn.Output(soma); //1 / (1 + Math.Pow(Math.E, -soma))

			return activationHidden;
		}

		private void BackPropagation(Neuron exemplo)
		{
			double soma;

			//calculo do erro para as unidades de saida
			//erroSaida = SigmoidFn.Derivative(activationHidden) * (exemplo.GetSaida() - activationHidden);

			//calculo do erro para as unidades da camada escondida
			for (int i = 0; i < tamanhoCamada1; i++)
			{
				soma = weights2[i] * erroSaida;
				erroEscondida[i] = SigmoidFn.Derivative(soma) * erroSaida * weights2[i];//  activation1[i] * (1 - activation1[i]) * soma;
			}

			//actualização dos pesos das unidades de saida
			for (int j = 0; j < numEntradas; j++)
			{
				weights2[j] += alfa * erroSaida * activation1[j];
			}

			//actualização dos pesos das unidades da camada escondida
			for (int i = 0; i < numSaidas; i++)
			{
				for (int j = 0; j < tamanhoCamada1 + 1; j++)
				{
					weights1[j, i] += alfa * erroEscondida[i] * exemplo.GetEntrada(j);
				}
			}
		}

		public double CalculaResultadoRede(double[] instancia)
		{
			return ForwardPropagation(instancia);
		}
	}

}