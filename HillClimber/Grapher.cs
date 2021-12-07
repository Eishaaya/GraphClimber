using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HillClimber
{
    interface Grapher
    {
        void Update(List<Button> points, float scaler);
        void Clear();
        float GetY(float x);
        double M { get; }
    }
}
