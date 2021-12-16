using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HillClimber
{
    class Climber : IGrapher
    {
        protected Random random = Extensions.random;
        public double M { get => nums[0]; }
        protected double b { get => nums[1]; }

        protected double[] nums = new double[2];

        protected bool cleared = false;

        protected Func<double, double, double> errorFunc;

        protected double trueError = double.MaxValue;

        public Climber(Func<double, double, double> error)
        {
            nums[0] = 0;
            nums[1] = 0;

            errorFunc = error;
        }

        public float GetY(float x)
        {
            return (float)(M * x + b);
        }

        public void Clear()
        {
            nums[0] = 0;
            nums[1] = 0;
            cleared = true;
        }

        public void Update(List<Button> spots, float scale)
        {
            if (cleared)
            {
                cleared = false;
                return;
            }

            var points = new Vector2[spots.Count];
            for (int i = 0; i < spots.Count; i++)
            {
                points[i] = spots[i].Location / scale;
            }
            Update(points);
        }
        protected virtual void Update(Vector2[] points)
        {
            var oldNums = (double[])nums.Clone();
            Mutate();

            if (Error(oldNums, points))
            {
                nums = oldNums;
            }
        }
        protected virtual void Mutate()
        {
            ref var num = ref nums[random.Next(0, nums.Length)];

            //adding a random between 0 & 1, then multiplying it half the time with -1
            num += random.NextDouble() * (double)(random.Next(0, 2) == 0? 1 : -1);
        }
        protected bool Error(double[] oldNums, Vector2[] points)
        {
            double error = 0;
            double oldError = 0;
            foreach (var point in points)
            {
                error += errorFunc(point.Y, (point.X * M + b));
                oldError += errorFunc(point.Y, (float)(point.X * oldNums[0] + oldNums[1]));
            }
            if (oldError < error)
            {
                trueError = oldError;
                return true;
            }
            trueError = error;
            return false;
        }
    }
}
