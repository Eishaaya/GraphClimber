using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HillClimber
{
    class HillFaller : Climber
    {
        //protected override void Mutate()
        double prevSlope = double.MaxValue;

        Func<double, double> errorDerivative;
        protected override void Mutate()
        {            
            ref var num = ref nums[random.Next(0, nums.Length)];
            
            var newSlope = errorDerivative(trueError);
            if (prevSlope == newSlope) 
                ;

            var adder = random.NextDouble();
            if (newSlope == prevSlope)
            {
                newSlope *= random.Next(0, 2);
            }
            //adding a random between 0 & 1, then multiplying it half the time with -1
            num += adder * (newSlope < prevSlope ? 1d : -1d);
            prevSlope = errorDerivative(trueError);
        }

        public HillFaller(Func<double, double, double> error, Func<double, double> derive)
           : base(error)
        {
            errorDerivative = derive;
        }

        protected override void Update(Vector2[] points)
        {
            var oldNums = (double[])nums.Clone();
            Mutate();

            var oldError = trueError;
            if (Error(oldNums, points))
            {
                nums = oldNums;
            }                          
        }

    }
}
