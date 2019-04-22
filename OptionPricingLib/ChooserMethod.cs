using Accord.Statistics.Distributions.Multivariate;
using MathNet.Numerics.Distributions;
using System;

namespace OptionPricingLib
{
    public class ChooserMethod
    {
        private static double Log(double X) { return Math.Log(X); }
        private static double CND(double X) { return Normal.CDF(0, 1, X); }
        private static double Exp(double X) { return Math.Exp(X); }
        private static double Sqr(double x) { return Math.Sqrt(x); }

        public static double SimpleChooserOption(string cpflg, double S0, double X, double t1, double T2, double r, double b, double vol)
        {

            double d = (Log(S0 / X) + (b + vol * vol / 2) * T2) / (vol * Sqr(T2));
            double y = (Log(S0 / X) + (b * T2 + vol * vol / 2 * t1)) / (vol * Sqr(t1));
            double price = S0 * Exp((b - r) * T2) * CND(d) - X * Exp(-r * T2) * CND(d - vol * Sqr(T2)) - 
                S0 * Exp((b - r) * T2) * CND(-y) + X * Exp(-r * T2) * CND(y - vol * Sqr(t1));
            return price;
        }
        public static double FDA_Delta(string cpflg, double S0, double X, double t1, double T2, double r, double b, double vol, double ds)
        {
            double delta = double.NaN;
            double bsr = SimpleChooserOption(cpflg, S0 + ds, X, t1, T2, r, b, vol);
            double bs = SimpleChooserOption(cpflg, S0, X, t1, T2, r, b, vol);
            double bsl = SimpleChooserOption(cpflg, S0 - ds, X, t1, T2, r, b, vol);
            delta = (bsr - bsl) / (2 * ds);
            return delta;
        }


        public static double FDA_GammaP(string cpflg, double S0, double X, double t1, double T2, double r, double b, double vol, double ds)
        {
            double gammap = double.NaN;
            double bsr = SimpleChooserOption(cpflg, S0 + ds, X, t1, T2, r, b, vol);
            double bs = SimpleChooserOption(cpflg, S0, X, t1, T2, r, b, vol);
            double bsl = SimpleChooserOption(cpflg, S0 - ds, X, t1, T2, r, b, vol);
            gammap = S0 / 100 * (bsr - 2 * bs + bsl) / (ds * ds);
            return gammap;
        }

        public static double FDA_Vega(string cpflg, double S0, double X, double t1, double T2, double r, double b, double vol, double ds)
        {
            double vega = double.NaN;
            double bsr = SimpleChooserOption(cpflg, S0, X, t1, T2, r, b, vol + 0.01);
            double bsl = SimpleChooserOption(cpflg, S0, X, t1, T2, r, b, vol - 0.01);
            vega = (bsr - bsl) / 2.0;
            return vega;
        }

        public static double FDA_Theta(string cpflg, double S0, double X, double t1, double T2, double r, double b, double vol, double ds)
        {
            double theta = double.NaN;
            double deltaT;
            if (t1 <= 1 / 252)
            {
                deltaT = 1 - 0.000005;
            }
            else
            {
                deltaT = 1 / 252;
            }
            double bsr = SimpleChooserOption(cpflg, S0, X, t1-deltaT, T2, r, b, vol);
            double bsl = SimpleChooserOption(cpflg, S0, X, t1, T2, r, b, vol);
            theta = bsr - bsl;
            return theta;
        }
    }
    public class ComplexChooserMethod
    {
        private static double Log(double X) { return Math.Log(X); }
        private static double CND(double X) { return Normal.CDF(0, 1, X); }
        private static double Exp(double X) { return Math.Exp(X); }
        private static double Sqr(double x) { return Math.Sqrt(x); }

        public static double SimpleChooserOption(string cpflg, double S0, double X, double t1, double T2, double r, double b, double vol)
        {

            double d = (Log(S0 / X) + (b + vol * vol / 2) * T2) / (vol * Sqr(T2));
            double y = (Log(S0 / X) + (b * T2 + vol * vol / 2 * t1)) / (vol * Sqr(t1));
            double price = S0 * Exp((b - r) * T2) * CND(d) - X * Exp(-r * T2) * CND(d - vol * Sqr(T2)) -
                S0 * Exp((b - r) * T2) * CND(-y) + X * Exp(-r * T2) * CND(y - vol * Sqr(t1));
            return price;
        }
        public static double FDA_Delta(string cpflg, double S0, double X, double t1, double T2, double r, double b, double vol, double ds)
        {
            double delta = double.NaN;
            double bsr = SimpleChooserOption(cpflg, S0 + ds, X, t1, T2, r, b, vol);
            double bs = SimpleChooserOption(cpflg, S0, X, t1, T2, r, b, vol);
            double bsl = SimpleChooserOption(cpflg, S0 - ds, X, t1, T2, r, b, vol);
            delta = (bsr - bsl) / (2 * ds);
            return delta;
        }


        public static double FDA_GammaP(string cpflg, double S0, double X, double t1, double T2, double r, double b, double vol, double ds)
        {
            double gammap = double.NaN;
            double bsr = SimpleChooserOption(cpflg, S0 + ds, X, t1, T2, r, b, vol);
            double bs = SimpleChooserOption(cpflg, S0, X, t1, T2, r, b, vol);
            double bsl = SimpleChooserOption(cpflg, S0 - ds, X, t1, T2, r, b, vol);
            gammap = S0 / 100 * (bsr - 2 * bs + bsl) / (ds * ds);
            return gammap;
        }

        public static double FDA_Vega(string cpflg, double S0, double X, double t1, double T2, double r, double b, double vol, double ds)
        {
            double vega = double.NaN;
            double bsr = SimpleChooserOption(cpflg, S0, X, t1, T2, r, b, vol + 0.01);
            double bsl = SimpleChooserOption(cpflg, S0, X, t1, T2, r, b, vol - 0.01);
            vega = (bsr - bsl) / 2.0;
            return vega;
        }

        public static double FDA_Theta(string cpflg, double S0, double X, double t1, double T2, double r, double b, double vol, double ds)
        {
            double theta = double.NaN;
            double deltaT;
            if (t1 <= 1 / 252)
            {
                deltaT = 1 - 0.000005;
            }
            else
            {
                deltaT = 1 / 252;
            }
            double bsr = SimpleChooserOption(cpflg, S0, X, t1 - deltaT, T2, r, b, vol);
            double bsl = SimpleChooserOption(cpflg, S0, X, t1, T2, r, b, vol);
            theta = bsr - bsl;
            return theta;
        }
    }
}