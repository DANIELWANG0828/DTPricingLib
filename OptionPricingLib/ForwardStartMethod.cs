using MathNet.Numerics.Distributions;
using MathNet.Numerics.Integration;
using System;

namespace OptionPricingLib
{
    public class ForwardStartMethod
    {
        public static double ForwardStart(string cpflg, double S0, double t, double T, double r, double b, double vol, double a)
        {

            double return_value = double.NaN;
            return_value = DoubleExponentialTransformation.Integrate((y) => { return Normal.PDF(0, 1, y) 
                * BlackScholesMethod.BlackScholes(cpflg, S0 * Math.Exp((b - 0.5 * vol * vol) * t + y * vol 
                * Math.Sqrt(t)), Math.Max(S0 * Math.Exp((b - 0.5 * vol * vol) * t + y * vol * Math.Sqrt(t)) + a, 0), 
                T - t, r, b, vol); }, -200, 200, 1e-4);
            return return_value;

        }

        public static double Delta(string cpflg, double S0, double t, double T, double r, double b, double vol, double a)
        {

            double return_value = double.NaN;
            double ds = 0.01;
            return_value = (ForwardStart(cpflg,S0*(1+ds),t,T,r,b,vol,a) - ForwardStart(cpflg, S0 * (1 - ds), t, T, r, b, vol, a)) / 2 / (ds*S0);
            return return_value;

        }

        public static double Vega(string cpflg, double S0, double t, double T, double r, double b, double vol, double a)
        {

            double return_value = double.NaN;
            return_value = (ForwardStart(cpflg, S0, t, T, r, b, vol+0.01, a) - ForwardStart(cpflg, S0, t, T, r, b, vol-0.01, a)) / 2;
            return return_value;

        }


    }
    public class VanillaForwardStartMethod
    {
        public static double VanillaForwardStart(string cpflg, double S0, double t, double T, double r, double b, double vol, double alpha)
        {

            double d1 = 0;
            double d2 = 0;
            double price = double.NaN;
            d1 = (Math.Log(1 / alpha) + (b + vol * vol / 2) * (T - t)) / (vol * Math.Sqrt(T - t));
            d2 = d1 - vol * Math.Sqrt(T - t);
            if (cpflg.Equals("c"))
            {
                price = S0 * Math.Exp((b - r) * t) * (Math.Exp((b - r) * (T - t)) * Normal.CDF(0, 1, d1) - alpha * Math.Exp(-r * (T - t)) * Normal.CDF(0, 1, d2));
            }
            if (cpflg.Equals("p"))
            {
                price = S0 * Math.Exp((b - r) * t) * (-Math.Exp((b - r) * (T - t)) * Normal.CDF(0, 1, -d1) + alpha * Math.Exp(-r * (T - t)) * Normal.CDF(0, 1, -d2));
            }
            return price;

        }

        public static double Delta(string cpflg, double S0, double t, double T, double r, double b, double vol, double a)
        {

            double return_value = double.NaN;
            double ds = 0.01;
            return_value = (VanillaForwardStart(cpflg, S0 * (1 + ds), t, T, r, b, vol, a) - VanillaForwardStart(cpflg, S0 * (1 - ds), t, T, r, b, vol, a)) / 2 / (ds * S0);
            return return_value;

        }

        public static double Vega(string cpflg, double S0, double t, double T, double r, double b, double vol, double a)
        {

            double return_value = double.NaN;
            return_value = (VanillaForwardStart(cpflg, S0, t, T, r, b, vol + 0.01, a) - VanillaForwardStart(cpflg, S0, t, T, r, b, vol - 0.01, a)) / 2;
            return return_value;

        }
    }

}