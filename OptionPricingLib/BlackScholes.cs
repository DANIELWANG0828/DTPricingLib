using MathNet.Numerics.Distributions;
using System;

namespace OptionPricingLib
{
    public class BlackScholesMethod
    {
        public static double BlackScholes(string cpflg, double S, double X, double T, double r, double b, double v)
        {
            double d1 = 0;
            double d2 = 0;
            double price = double.NaN;
            if (T == 0)
            {
                if (cpflg.Equals("c"))
                {
                    price = Math.Max(S - X, 0);
                }
                else
                {
                    price = Math.Max(X - S, 0);
                }
                return price;
            }

            d1 = (Math.Log(S / X) + (b + v * v / 2) * T) / (v * Math.Sqrt(T));
            d2 = d1 - v * Math.Sqrt(T);
            if (cpflg.Equals("c"))
            {
                price = S * Math.Exp((b - r) * T) * Normal.CDF(0, 1, d1) - X * Math.Exp(-r * T) * Normal.CDF(0, 1, d2);
            }
            if (cpflg.Equals("p"))
            {
                price = X * Math.Exp(-r * T) * Normal.CDF(0, 1, -d2) - S * Math.Exp((b - r) * T) * Normal.CDF(0, 1, -d1);
            }
            return price;
        }

        public static double Delta(string cpflg, double S, double X, double T, double r, double b, double v)
        {
            double d1 = 0;
            d1 = (Math.Log(S / X) + (b + v * v / 2) * T) / (v * Math.Sqrt(T));
            double delta = double.NaN;
            if (cpflg.Equals("c"))
            {
                delta = Math.Exp((b - r) * T) * Normal.CDF(0, 1, d1);
            }
            if (cpflg.Equals("p"))
            {
                delta = -Math.Exp((b - r) * T) * Normal.CDF(0, 1, -d1);
            }
            return delta;
        }

        public static double DdeltaDvol(double S, double X, double T, double r, double b, double v)
        {
            double d1 = 0;
            double d2 = 0;
            d1 = (Math.Log(S / X) + (b + v * v / 2) * T) / (v * Math.Sqrt(T));
            d2 = d1 - v * Math.Sqrt(T);
            double ddeltadvol = 0;
            ddeltadvol = -Math.Exp((b - r) * T) * d2 / v * Normal.PDF(0, 1, d1);
            return ddeltadvol;
        }

        public static double DvannaDvol(double S, double X, double T, double r, double b, double v)
        {
            double d1 = 0;
            double d2 = 0;
            d1 = (Math.Log(S / X) + (b + v * v / 2) * T) / (v * Math.Sqrt(T));
            d2 = d1 - v * Math.Sqrt(T);
            double dvannadvol = -Math.Exp((b - r) * T) * d2 / v * Normal.PDF(0, 1, d1) / v * (d1 * d2 - d1 / d2 - 1);
            return dvannadvol;
        }

        public static double DdeltaDtime(string cpflg, double S, double X, double T, double r, double b, double v)
        {
            double d1 = 0;
            double d2 = 0;
            d1 = (Math.Log(S / X) + (b + v * v / 2) * T) / (v * Math.Sqrt(T));
            d2 = d1 - v * Math.Sqrt(T);
            double ddeltadtime = double.NaN;
            if (cpflg.Equals("c"))
            {
                ddeltadtime = -Math.Exp((b - r) * T) * (Normal.PDF(0, 1, d1) * (b / (v * Math.Sqrt(T)) - d2 / (2 * T)) + (b - r) * Normal.CDF(0, 1, d1));
            }
            if (cpflg.Equals("p"))
            {
                ddeltadtime = -Math.Exp((b - r) * T) * (Normal.PDF(0, 1, d1) * (b / (v * Math.Sqrt(T)) - d2 / (2 * T)) - (b - r) * Normal.CDF(0, 1, -d1));
            }
            return ddeltadtime;
        }

        public static double Elasticity(string cpflg, double S, double X, double T, double r, double b, double v)
        {
            double d1 = 0;
            d1 = (Math.Log(S / X) + (b + v * v / 2) * T) / (v * Math.Sqrt(T));
            double elasticity = double.NaN;
            if (cpflg.Equals("c"))
            {
                double c = 0;
                c = BlackScholes("c", S, X, T, r, b, v);
                elasticity = Math.Exp((b - r) * T) * Normal.CDF(0, 1, d1) * S / c;
            }
            if (cpflg.Equals("p"))
            {
                double p = 0;
                p = BlackScholes("p", S, X, T, r, b, v);
                elasticity = Math.Exp((b - r) * T) * (Normal.CDF(0, 1, d1) - 1) * S / p;
            }
            return elasticity;
        }

        public static double Gamma(double S, double X, double T, double r, double b, double v)
        {
            double d1 = 0;
            d1 = (Math.Log(S / X) + (b + v * v / 2) * T) / (v * Math.Sqrt(T));
            double gamma = 0;
            gamma = Normal.PDF(0, 1, d1) * Math.Exp((b - r) * T) / (S * v * Math.Sqrt(T));
            return gamma;
        }

        public static double GammaP(double S, double X, double T, double r, double b, double v)
        {
            double d1 = 0;
            d1 = (Math.Log(S / X) + (b + v * v / 2) * T) / (v * Math.Sqrt(T));
            double gammap = 0;
            gammap = Normal.PDF(0, 1, d1) * Math.Exp((b - r) * T) / (100 * v * Math.Sqrt(T));
            return gammap;
        }

        public static double Vega(double S, double X, double T, double r, double b, double v)
        {
            double d1 = 0;
            d1 = (Math.Log(S / X) + (b + v * v / 2) * T) / (v * Math.Sqrt(T));
            double vega = 0;
            vega = S * Math.Exp((b - r) * T) * Normal.PDF(0, 1, d1) * Math.Sqrt(T);
            return vega;
        }

        public static double VegaP(double S, double X, double T, double r, double b, double v)
        {
            double d1 = 0;
            d1 = (Math.Log(S / X) + (b + v * v / 2) * T) / (v * Math.Sqrt(T));
            double vegap = 0;
            vegap = v * Math.Exp((b - r) * T) * Normal.PDF(0, 1, d1) * Math.Sqrt(T) / 10;
            return vegap;
        }

        public static double Theta(string cpflg, double S, double X, double T, double r, double b, double v)
        {
            double d1 = 0;
            double d2 = 0;
            d1 = (Math.Log(S / X) + (b + v * v / 2) * T) / (v * Math.Sqrt(T));
            d2 = d1 - v * Math.Sqrt(T);
            double theta = double.NaN;
            if (cpflg.Equals("c"))
            {
                theta = -S * Math.Exp((b - r) * T) * Normal.PDF(0, 1, d1) * v / (2 * Math.Sqrt(T))
                    - (b - r) * S * Math.Exp((b - r) * T) * Normal.CDF(0, 1, d1)
                    - r * X * Math.Exp(-r * T) * Normal.CDF(0, 1, d2);
            }
            if (cpflg.Equals("p"))
            {
                theta = -S * Math.Exp((b - r) * T) * Normal.PDF(0, 1, d1) * v / (2 * Math.Sqrt(T))
                    + (b - r) * S * Math.Exp((b - r) * T) * Normal.CDF(0, 1, d1)
                    + r * X * Math.Exp(-r * T) * Normal.CDF(0, 1, d2);
            }
            return theta;
        }

        public static double FDA_Delta(string cpflg, double S, double X, double T, double r, double b, double v, double ds)
        {
            double delta = double.NaN;
            double bsr = BlackScholes(cpflg, S + ds, X, T, r, b, v);
            double bs = BlackScholes(cpflg, S, X, T, r, b, v);
            double bsl = BlackScholes(cpflg, S - ds, X, T, r, b, v);
            delta = (bsr - bsl) / (2 * ds);
            return delta;
        }

        public static double FDA_DeltaR(string cpflg, double S, double X, double T, double r, double b, double v, double ds)
        {
            double delta = double.NaN;
            double bsr = BlackScholes(cpflg, S + ds, X, T, r, b, v);
            double bs = BlackScholes(cpflg, S, X, T, r, b, v);
            double bsl = BlackScholes(cpflg, S - ds, X, T, r, b, v);
            delta = (bsr - bs) / (ds);
            return delta;
        }

        public static double FDA_DeltaL(string cpflg, double S, double X, double T, double r, double b, double v, double ds)
        {
            double delta = double.NaN;
            double bsr = BlackScholes(cpflg, S + ds, X, T, r, b, v);
            double bs = BlackScholes(cpflg, S, X, T, r, b, v);
            double bsl = BlackScholes(cpflg, S - ds, X, T, r, b, v);
            delta = (bs - bsl) / (ds);
            return delta;
        }

        public static double FDA_GammaP(string cpflg, double S, double X, double T, double r, double b, double v, double ds)
        {
            double gammap = double.NaN;
            double bsr = BlackScholes(cpflg, S + ds, X, T, r, b, v);
            double bs = BlackScholes(cpflg, S, X, T, r, b, v);
            double bsl = BlackScholes(cpflg, S - ds, X, T, r, b, v);
            gammap = S / 100 * (bsr - 2 * bs + bsl) / (ds * ds);
            return gammap;
        }

        public static double FDA_Vega(string cpflg, double S, double X, double T, double r, double b, double v, double ds)
        {
            double vega = double.NaN;
            double bsr = BlackScholes(cpflg, S, X, T, r, b, v + 0.01);
            double bsl = BlackScholes(cpflg, S, X, T, r, b, v - 0.01);
            vega = (bsr - bsl) / 2.0;
            return vega;
        }

        public static double FDA_Theta(string cpflg, double S, double X, double T, double r, double b, double v, double ds)
        {
            double theta = double.NaN;
            double deltaT;
            if (T <= 1 / 365.0)
            {
                deltaT = 1 - 0.000005;
            }
            else
            {
                deltaT = 1 / 365.0;
            }
            double bsr = BlackScholes(cpflg, S, X, T - deltaT, r, b, v);
            double bsl = BlackScholes(cpflg, S, X, T, r, b, v);
            theta = bsr - bsl;
            return theta;
        }
    }
}
