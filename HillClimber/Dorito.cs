using System;

namespace HillClimber
{
    class Dorito
    {
        double bias;
        DoritoDust[] dendrites;
        public double Output { get; set; }
        public double Input { get; private set; }
        public ActivationFunction Activation { get; set; }

        public Dorito(ActivationFunction activation, Dorito[] previousDoritos)
        {

        }
        public void Randomize(Random random, double min, double max)
        {

        }
        public double Compute()
        {
            return 0;
        }

    }
}