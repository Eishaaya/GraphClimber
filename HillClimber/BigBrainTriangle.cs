using System;
using System.Collections.Generic;
using System.Text;

namespace Perceptron
{
    class BigBrainTriangle
    {
        double[] weights;
        double bias;
        Random aléatoire;
        double mutationLevel;
        Func<double, double, double> errorCalc;

        public BigBrainTriangle(double[] initialWeightValues, double initialBiasValue,
            double mutationSize, Random random, Func<double, double, double> err)
        { /*initializes the weights array and bias*/
            weights = initialWeightValues;
            bias = initialBiasValue;
            aléatoire = random;
            mutationLevel = mutationSize;
            errorCalc = err;
        }

        public BigBrainTriangle(int amountOfInputs, Random random, double mutationAmount,
            Func<double, double, double> err)
        { /*Initializes the weights array given the amount of inputs*/
            weights = new double[amountOfInputs];
            Randomize(random, -mutationAmount, mutationAmount);
            errorCalc = err;
            mutationLevel = mutationAmount;
            aléatoire = random;
        }

        public void Randomize(Random random, double min, double max)
        { /*Randomly generates values for every weight including the bias*/
            double range = max - min;
            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = random.NextDouble() * range;
            }
            bias = random.NextDouble() * range;            
        }

        public double Compute(double[] inputs)
        { /*computes the output with given input*/
            var sum = 0d;
            for (int i = 0; i < inputs.Length; i++)
            {
                sum += inputs[i] * weights[i];
            }
            return sum + bias;
            //return 4;
        }

        public double[] Compute(double[][] inputs)
        { /*computes the output for each row of inputs*/
            double[] results = new double[inputs.Length];
            for (int i = 0; i < inputs.Length; i++)
            {
                results[i] = Compute(inputs[i]);
            }

            return results;
        }

        public double GetError(double[][] inputs, double[] desiredOutputs)
        {
            double error = 0;
            double[] output = Compute(inputs);

            for (int i = 0; i < output.Length; i++)
            {
                error += errorCalc(output[i], desiredOutputs[i]);
            }

            error /= output.Length;
            return error;
        }

        public double TrainDeezNuts(double[][] inputs, double[] desiredOutputs, double currentError)
        {           
            var randIndex = aléatoire.Next(weights.Length + 1);
            ref double poidsMuté = ref bias;
            if (randIndex != weights.Length)
            {
                poidsMuté = ref weights[randIndex];
            }

            var mutation = aléatoire.NextDouble() * (aléatoire.Next(0, 2) != 0 ? mutationLevel : -mutationLevel);
            poidsMuté += mutation;

            var newError = GetError(inputs, desiredOutputs);

            if (newError > currentError)
            {
                poidsMuté -= mutation;
                return currentError;
            }
            return newError;
        }
    }
}
