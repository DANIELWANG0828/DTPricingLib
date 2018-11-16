using MathNet.Numerics.Distributions;
using System;

namespace OptionPricingLib
{
    public class DiscreteAdjustedBarrierMethod
    {
        //Util Functions
        private static double Exp(double X) { return Math.Exp(X); }
        private static double Sqr(double x) { return Math.Sqrt(x); }


        public static double DiscreteAdjustedBarrier(double S, double H, double v, double dt)
        {
            double barrier_adj = double.NaN;
            if (H > S)
            {
                barrier_adj = H * Exp(0.5826 * v * Sqr(dt));
            }
            else if (H < S)
            {
                barrier_adj = H * Exp(-0.5826 * v * Sqr(dt));
            }
            else
            {
                barrier_adj = double.NaN;
            }
            return barrier_adj;
        }
    }
}
