using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HillClimber
{
    class DoritoClump
    {
        public Dorito[] Doritos { get; }
        public double[] Outputs { get; }

        public DoritoClump(ActivationFunction activation, int doritoCount, DoritoClump previousClump)
        {
            Doritos = new Dorito[doritoCount];
            for (int i = 0; i < Doritos.Length; i++)
            {
                Doritos[i] = new Dorito(activation, previousClump.Doritos);
            }
            Outputs = new double[doritoCount];
        }
        public void Randomize(Random random, double min, double max)
        {
            foreach(var dorito in Doritos)
            {
                dorito.Randomize(random, min, max);
            }
        }
        public double[] Compute()
        {            
            for (int i = 0; i < Doritos.Length; i++)
            {
                Outputs[i] = Doritos[i].Compute();
            }
            return Outputs;
        }
    }
}
