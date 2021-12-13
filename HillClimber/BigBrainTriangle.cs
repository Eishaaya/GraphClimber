using System;
using System.Collections.Generic;
using System.Text;

namespace HillClimber
{
    class BigBrainTriangle
    {
        double[] weights;
        double bias;
        Random aléatoire;
        double mutationLevel;
        Func<double, double, double> errorCalc;
        ActivationFunction filterButNot;
        int oldSize = 0;

        public BigBrainTriangle(double[] initialWeightValues, double initialBiasValue,
            double mutationSize, Random random, Func<double, double, double> err, ActivationFunction filter = null)
        { /*initializes the weights array and bias*/
            weights = initialWeightValues;
            bias = initialBiasValue;
            aléatoire = random;
            mutationLevel = mutationSize;
            errorCalc = err;

            if (filter != null)
            {
                filterButNot = filter;
            }
            else
            {
                filterButNot = new ActivationFunction(ActivationFunction.Identity, ActivationFunction.IdentityDeriv);
            }
        }

        public BigBrainTriangle(int amountOfInputs, Random random, double mutationAmount,
            Func<double, double, double> err, ActivationFunction filter = null)
        { /*Initializes the weights array given the amount of inputs*/
            weights = new double[amountOfInputs];
            Randomize(random, -mutationAmount, mutationAmount);
            errorCalc = err;
            mutationLevel = mutationAmount;
            aléatoire = random;

            if (filter != null)
            {
                filterButNot = filter;
            }
            else
            {
                filterButNot = new ActivationFunction(ActivationFunction.Identity, ActivationFunction.IdentityDeriv);
            }
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
            sum /= inputs.Length;
            return filterButNot.Function(sum + bias);
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

        public double GetError(double[][] inputs, Func<double, double, double, double> GetY, double bOrM)
        {
            double error = 0;
            double output = Compute(inputs[0]);

            for (int i = 0; i < inputs[0].Length; i++)
            {
                var gottenY = GetY(inputs[0][i], output, bOrM);
                error += errorCalc(gottenY, inputs[1][i]);
            }            
            return error;
        }

        public bool ShiftSizeTo(int length)
        {
            var olderSize = oldSize;
            oldSize = length;            
            if (length <= olderSize) return length < olderSize;
            
            double[] newWeights = new double[length];            

            for (int i = 0; i < Math.Min(weights.Length, length); i++)
            {
                newWeights[i] = weights[i];
            }
            for (int i = weights.Length; i < newWeights.Length; i++)
            {
                newWeights[i] = aléatoire.NextDouble() * mutationLevel;
            }

            weights = newWeights;
            return true;
        }

        public double TrainDeezNuts(double[][] inputs, Func<double, double, double, double> GetY, double bOrM, double currentError)
        {           
            var randIndex = aléatoire.Next(weights.Length + 1);
            ref double poidsMuté = ref bias;
            if (randIndex != weights.Length)
            {
                poidsMuté = ref weights[randIndex];
            }

            var mutation = aléatoire.NextDouble() * (aléatoire.Next(0, 2) != 0 ? mutationLevel : -mutationLevel);
            poidsMuté += mutation;

            var newError = GetError(inputs, GetY, bOrM);

            if (newError > currentError)
            {
                poidsMuté -= mutation;
                return currentError;
            }
            return newError;
        }
    }
}
