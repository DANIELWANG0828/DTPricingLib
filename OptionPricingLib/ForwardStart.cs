using MathNet.Numerics.Distributions;
using MathNet.Numerics.Integration;
using System;

namespace OptionPricingLib
{
    public class ForwardStartOption
    {
        public static double ForwardStart(string cpflg, double S0, double t1, double t2, double r, double b, double vol, double a)
        {

            double return_value = double.NaN;
            return_value = DoubleExponentialTransformation.Integrate((y) => { return Normal.PDF(0, 1, y) * BlackScholesMethod.BlackScholes(cpflg, S0 * Math.Exp((b - 0.5 * vol * vol) * t1 + y * vol * Math.Sqrt(t1)), Math.Max(S0 * Math.Exp((b - 0.5 * vol * vol) * t1 + y * vol * Math.Sqrt(t1)) + a, 0), t2 - t1, r, b, vol); }, -200, 200, 1e-4);
            return return_value;

        }

        public static double Delta(string cpflg, double S0, double t1, double t2, double r, double b, double vol, double a)
        {

            double return_value = double.NaN;
            double ds = 0.01;
            return_value = (ForwardStart(cpflg,S0*(1+ds),t1,t2,r,b,vol,a) - ForwardStart(cpflg, S0 * (1 - ds), t1, t2, r, b, vol, a)) / 2 / (ds*S0);
            return return_value;

        }

        public static double Vega(string cpflg, double S0, double t1, double t2, double r, double b, double vol, double a)
        {

            double return_value = double.NaN;
            return_value = (ForwardStart(cpflg, S0, t1, t2, r, b, vol+0.01, a) - ForwardStart(cpflg, S0, t1, t2, r, b, vol-0.01, a)) / 2;
            return return_value;

        }


    }
}