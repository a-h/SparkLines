using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Publisher
{
    public class PerlinNoise
    {
        /* coherent noise function over 1, 2 or 3 dimensions */
        /* (copyright Ken Perlin) */
        const int B = 0x100; // 256
        const int BM = 0xff;

        const int N = 0x1000;
        const int NP = 12;   /* 2^N */
        const int NM = 0xfff;

        int[] p = new int[B + B + 2];
        double[] g1 = new double[B + B + 2];

        private Random rnd = new Random();

        public PerlinNoise()
        {
            int i, j, k;

            for (i = 0; i < B; i++)
            {
                p[i] = i;

                g1[i] = (double)((rnd.Next() % (B + B)) - B) / B;
            }

            while (--i > 0)
            {
                k = p[i];
                p[i] = p[j = rnd.Next() % B];
                p[j] = k;
            }

            for (i = 0; i < B + 2; i++)
            {
                p[B + i] = p[i];
                g1[B + i] = g1[i];
            }
        }

        private static double s_curve(double t)
        {
            return t * t * (3.0d - 2.0d * t);
        }

        private static double lerp(double t, double a, double b)
        {
            return a + t * (b - a);
        }

        public double Noise(double x)
        {
            double t = x + N;
            int bx0 = ((int)t) & BM;
            int bx1 = (bx0 + 1) & BM;
            double rx0 = t - (int)t;
            double rx1 = rx0 - 1.0d;

            double sx = s_curve(rx0);

            double u = rx0 * g1[p[bx0]];
            double v = rx1 * g1[p[bx1]];

            return lerp(sx, u, v);
        }

        private static void normalize(double[] vec)
        {
            var sumOfSquares = vec.Select(v => v * v).Sum();
            var squareRootOfSumOfSquares = Math.Sqrt(sumOfSquares);

            for (int i = 0; i < vec.Length; i++)
            {
                vec[i] = vec[i] / squareRootOfSumOfSquares;
            }
        }

        private static void SizeArray(double[][] array, int childDimensions)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = new double[childDimensions];
            }
        }
    }
}
