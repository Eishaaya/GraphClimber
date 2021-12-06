using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HillClimber
{
    class DoritoGrapher : Grapher
    {
        Random aléatoire = Extensions.random;
        double[] blank { get; } = new double[] { 0, 0 };
        BigBrainTriangle slope;
        BigBrainTriangle offset;
        Func<double, double, double> errorCalc;

        double currentError = int.MaxValue;

        public double M { get; private set; }
        double b;


        public void Update(List<Button> points, float scale)
        {       
            var inputs = ConvertToDoubles(points, scale);

            M = slope.Compute(inputs[0]);
            b = offset.Compute(inputs[0]);

            double slopeError;
            double offError;

            if (slope.ShiftSizeTo(points.Count) && offset.ShiftSizeTo(points.Count))
            {
                slopeError = slope.GetError(inputs, GetYForM, b);
                offError = offset.GetError(inputs, GetYForB, M);
                currentError = slopeError * offError / 2;
            }

            slopeError = slope.TrainDeezNuts(inputs, GetYForM, b, currentError);
            offError = offset.TrainDeezNuts(inputs, GetYForB, M, currentError);
            currentError = slopeError * offError / 2;
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

        public float GetY(float x)
        {
            return (float)(M * x + b);
        }

        public static double GetYForM(double x, double m, double b)
        {
            return m * x + b;
        }
        public static double GetYForB(double x, double b, double m)
        {
            return m * x + b;
        }
    }
}
