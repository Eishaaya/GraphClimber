using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HillClimber
{
    class ClimbTrainer : ITrainer
    {
        public ClimbTrainer(Func<double, double> deriv)
        {
            Deriv = deriv;
        }

        public Func<double, double> Deriv { get; }

        public virtual ITrainer Clone()
        {
            return new ClimbTrainer(Deriv);
        }
        public virtual double Mutate(Random random, double mutationLevel, double error)
        {
            return random.NextDouble() * (random.Next(0, 2) != 0 ? mutationLevel : -mutationLevel);
        }
    }
    class FallTrainer : ClimbTrainer
    {
        public FallTrainer(Func<double, double> deriv)
            : base(deriv)
        {
            prevSlope = double.PositiveInfinity;
        }

        double prevSlope;
        public override ITrainer Clone()
        {
            return new FallTrainer(Deriv);
        }
        public override double Mutate(Random random, double mutationLevel, double error)
        {
            var newSlope = Deriv(error);

            var adder = random.NextDouble();
            if (newSlope == prevSlope)
            {
                newSlope *= random.Next(0, 2);
            }
            //adding a random between 0 & 1, then multiplying it half the time with -1

            
            var returner = adder * (newSlope < prevSlope ? 1d : -1d);
            prevSlope = Deriv(error);
            return returner;
        }
    }
}
