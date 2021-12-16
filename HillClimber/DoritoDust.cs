using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HillClimber
{
    class DoritoDust
    {
        public Dorito Previous { get; }
        public Dorito Next { get; }
        public double Weight { get; set; }

        public DoritoDust(Dorito previous, Dorito next, double weight)
        {
            Previous = previous;
            Next = next;
            Weight = weight;
        }
        public double Compute()
        {
            return Previous.Output * Weight;
        }
    }
}
