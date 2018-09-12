
using System;


namespace NeuralNetwork {

	[Serializable]
    public class WeightsFile {

        private double[][] weights1;
        private double[][] weights2;
		private double[] erroSaida;
		private double[] erroEscondida;
		private double[] activation1;
		private double[] activationHidden;


		public WeightsFile(double[][] w1, double[][] w2, double[] erroSaida, double[] erroEscondida, double[] activation1, double[] activationHidden)
		{
			weights1 = w1;
			weights2 = w2;
			this.erroSaida = erroSaida;
			this.erroEscondida = erroEscondida;
			this.activation1 = activation1;
			this.activationHidden = activationHidden;
		}

		public double[][] GetW1()
		{
			return weights1;
		}

		public double[][] GetW2()
		{
			return weights2;
		}

		public double[] GetErroSaida()
		{
			return erroSaida;
		}

		public double[] GetErroEscondida()
		{
			return erroEscondida;
		}

		public double[] GetActivation1()
		{
			return activation1;
		}

		public double[] GetActivationHidden()
		{
			return activationHidden;
		}
	}
}