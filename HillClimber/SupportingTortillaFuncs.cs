using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HillClimber
{
    public class ActivationFunction
    {
        Func<double, double> function;
        Func<double, double> derivative;
        public ActivationFunction(Func<double, double> function, Func<double, double> derivative)
        {
            this.function = function;
            this.derivative = derivative;
        }

        public double Function(double input)
        {
            return function(input);
        }

        public double Derivative(double input)
        {
            return derivative(input);
        }

        #region funcs
        public static double BinaryStep(double input)
        {
            return input < 0 ? 0 : 1;
        }
        public static double Sigmoid(double input)
        {
            return 1 / (1 + Math.Pow(Math.E, -input));
        }
        public static double TanH(double input)
        {
            var pE = Math.Pow(Math.E, input);
            var nE = Math.Pow(Math.E, -input);
            return (pE - nE) / (pE + nE);
        }
        public static double ReLU(double input)
        {
            return input < 0 ? 0 : input;
        }

        #endregion

        #region derivs
        public static double BinaryDeriv(double input)
        {
            return 1;
        }
        public static double SigDeriv(double input)
        {
            return input * (1 - input);
        }
        public static double TanDeriv(double input)
        {
            return 1 - input * input;
        }
        public static double RectalDeriv(double input)
        {
            return ReLU(input);
        }
        #endregion
    }

    public class ErrorFunction
    {
        Func<double, double, double> function;
        Func<double, double, double> derivative;
        public ErrorFunction(Func<double, double, double> function, Func<double, double, double> derivative)
        {
            this.function = function;
            this.derivative = derivative;
        }

        public double Function(double output, double desiredOutput)
        {
            return function(output, desiredOutput);
        }
        public double Derivative(double output, double desiredOutput)
        {
            return derivative(output, desiredOutput);
        }
    }
}
