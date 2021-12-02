using System;
using System.Collections.Generic;
using System.Text;

namespace HillClimber
{
    class BigBrainTriangle : Grapher
    {
        public double M { get => nums[0]; }
        double b { get => nums[1]; }

        //double[] goal { get; } = new double[] { 0, 0 };

        double[] nums = new double[2];
        double[] weights;
        double bias;
        Random aléatoire;
        double mutationLevel;
        double currentError = int.MaxValue;
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

        public float GetY(float x)
        {
            return (float)(M * x + b);
        }

        public void Update(List<Button> points, float scale)
        {
            ShiftSizeTo(points.Count);
            var inputs = ConvertToDoubles(points, scale);

            currentError = TrainDeezNuts(inputs);

            nums = Compute(inputs);
        }

        static double[][] ConvertToDoubles(List<Button> points, float scale)
        {
            var result = new double[2][] { new double[points.Count], new double[points.Count] };

            for (int i = 0; i < points.Count; i++)
            {
                result[1][i] = points[i].Location.X / scale;
                result[0][i] = points[i].Location.Y / scale;
            }

            return result;
        }

        public void ShiftSizeTo(int length)
        {
            if (length <= weights.Length) return;

            double[] newWeights = new double[length];
            currentError = int.MaxValue;

            for (int i = 0; i < Math.Min(weights.Length, length); i++)
            {
                newWeights[i] = weights[i];
            }
            for (int i = weights.Length; i < newWeights.Length; i++)
            {
                newWeights[i] = aléatoire.NextDouble() * mutationLevel;
            }

            weights = newWeights;
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

        public double GetError(double[][] inputs)
        {
            double error = 0;
            nums = Compute(inputs);

            for (int i = 0; i < inputs[0].Length; i++)
            {
                var outPutY = GetY((float)inputs[0][i]);
                var realOutput = inputs[1][i];
                error += errorCalc(realOutput, outPutY);
            }

            error /= inputs[0].Length;
            return error;
        }

        public double TrainDeezNuts(double[][] inputs)
        {
            var randIndex = aléatoire.Next(weights.Length + 1);
            ref double poidsMuté = ref bias;
            if (randIndex != weights.Length)
            {
                poidsMuté = ref weights[randIndex];
            }

            var mutation = aléatoire.NextDouble() * (aléatoire.Next(0, 2) != 0 ? mutationLevel : -mutationLevel);
            poidsMuté += mutation;

            var newError = GetError(inputs);

            if (newError > currentError)
            {
                poidsMuté -= mutation;
                return currentError;
            }
            return newError;
        }
    }
}
