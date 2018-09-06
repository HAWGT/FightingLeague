using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NeuralNetwork {
    [System.Serializable]
    public class WeightsFile : MonoBehaviour {

        public double[,] weights1;
        public double[] weights2;
        public WeightsFile(double [,] w1, double[] w2)
        {
            this.weights1 = w1;
            this.weights2 = w2;
        }
    }
}