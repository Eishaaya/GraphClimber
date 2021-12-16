using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HillClimber
{
    interface ITrainer
    {
        //Func<double, double, double> Func { get; set; }
        Func<double, double> Deriv { get; }
        ITrainer Clone();
        double Mutate(Random random, double mutationLevel, double error);
    }
}
