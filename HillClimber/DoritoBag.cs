using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HillClimber
{
    class DoritoBag
    {
        DoritoClump[] clumps;
        ErrorFunction errorFunc;

        public DoritoBag(ActivationFunction activation, ErrorFunction errorFunc,
        params int[] doritosPerClump)
        {
            this.errorFunc = errorFunc;
            clumps = new DoritoClump[doritosPerClump.Length];
            
        }
        public void Randomize(Random random, double min, double max)
        {
            foreach (var clump in clumps)
            {
                clump.Randomize(random, min, max);
            }
        }
        public double[] Compute(double[] inputs)
        {
            foreach (var clump in clumps)
            {
                
            }
        }
        public double GetError(double[] inputs, double[] desiredOutputs)
        {
            return 0;
        }
    }
}
