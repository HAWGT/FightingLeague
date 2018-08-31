using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeuralNetwork
{
	public class SigmoidFn
	{
		public static double Output(double x)
		{
			return 1 / (1 + Math.Exp(-x));
		}

		public static double Derivative(double x)
		{
			return x * (1 - x);
		}

	}

}