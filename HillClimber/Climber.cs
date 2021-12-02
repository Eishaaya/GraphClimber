using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HillClimber
{
    class Climber : Grapher
    {
        Random random = Extensions.random;
        public double M { get => nums[0]; }
        double b { get => nums[1]; }

        double[] nums = new double[2];

        public Climber()
        {
            nums[0] = 0;
            nums[1] = 0;
        }

        public float GetY(float x)
        {
            return (float)(M * x + b);
        }

        public void Update(List<Button> spots, float scale)
        {
            var points = new Vector2[spots.Count];
            for (int i = 0; i < spots.Count; i++)
            {
                points[i] = spots[i].Location / scale;
            }
            Update(points);
        }
        void Update(Vector2[] points)
        {
            var oldNums = (double[])nums.Clone();
            Mutate();

            if (Error(oldNums, points))
            {
                nums = oldNums;
            }
        }
        void Mutate()
        {
            ref var num = ref nums[random.Next(0, nums.Length)];

            //adding a random between 0 & 1, then multiplying it sometimes with -1
            num += random.NextDouble() * (double)(random.Next(0, 2) == 0? 1 : -1);
        }
        bool Error(double[] oldNums, Vector2[] points)
        {
            float error = 0;
            float oldError = 0;
            foreach (var point in points)
            {
                error += MathHelper.Distance(point.Y, (float)(point.X * M + b));
                oldError += MathHelper.Distance(point.Y, (float)(point.X * oldNums[0] + oldNums[1]));
            }
            return oldError < error;
        }
    }
}
