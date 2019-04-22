using Accord.Statistics.Distributions.Multivariate;
using MathNet.Numerics.Distributions;
using System;

namespace OptionPricingLib
{
    public class UtilFuncs
    {

        //Util Functions
        public static double Log(double X) { return Math.Log(X); }
        public static double CND(double X) { return Normal.CDF(0, 1, X); }
        public static double Exp(double X) { return Math.Exp(X); }
        public static double Sqr(double x) { return Math.Sqrt(x); }
        public static double M(double x, double y, double rho)
        { return MultivariateNormalDistribution.Bivariate(0, 0, 1, 1, rho).DistributionFunction(new double[] { x, y }); }
    }
}
