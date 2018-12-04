using MathNet.Numerics.Distributions;
using System;

namespace OptionPricingLib
{
    public class BinaryMethodCashOrNothing
    {

        //Util Functions
        private static double Log(double X) { return Math.Log(X); }
        private static double CND(double X) { return Normal.CDF(0, 1, X); }
        private static double Exp(double X) { return Math.Exp(X); }
        private static double Sqr(double x) { return Math.Sqrt(x); }


        public static double CashOrNothing(string tpflg, double S, double x, double k, double T, double r, double b, double v)
        {
            double price = double.NaN;
            double d;
            d = (Log(S / x) + (b - v * v / 2) * T) / (v * Sqr(T));

            if (tpflg.Equals("c"))
            {
                price = k * Exp(-r * T) * CND(d);
            }
            else if (tpflg.Equals("p"))
            {
                price = k * Exp(-r * T) * CND(-d);
            }
            else
            {
                price = double.NaN;
            }
            return price;
        }

        public static double FDA_Delta(string tpflg, double S, double x, double k, double T, double r, double b, double v, double ds)
        {
            double delta = double.NaN;
            double bsr = CashOrNothing(tpflg, S + ds, x, k, T, r, b, v);
            double bs = CashOrNothing(tpflg, S, x, k, T, r, b, v);
            double bsl = CashOrNothing(tpflg, S - ds, x, k, T, r, b, v);
            delta = (bsr - bsl) / (2 * ds);
            return delta;
        }

        public static double FDA_DeltaR(string tpflg, double S, double x, double k, double T, double r, double b, double v, double ds)
        {
            double delta = double.NaN;
            double bsr = CashOrNothing(tpflg, S + ds, x, k, T, r, b, v);
            double bs = CashOrNothing(tpflg, S, x, k, T, r, b, v);
            double bsl = CashOrNothing(tpflg, S - ds, x, k, T, r, b, v);
            delta = (bsr - bs) / (ds);
            return delta;
        }

        public static double FDA_DeltaL(string tpflg, double S, double x, double k, double T, double r, double b, double v, double ds)
        {
            double delta = double.NaN;
            double bsr = CashOrNothing(tpflg, S + ds, x, k, T, r, b, v);
            double bs = CashOrNothing(tpflg, S, x, k, T, r, b, v);
            double bsl = CashOrNothing(tpflg, S - ds, x, k, T, r, b, v);
            delta = (bs - bsl) / (ds);
            return delta;
        }

        public static double FDA_GammaP(string tpflg, double S, double x, double k, double T, double r, double b, double v, double ds)
        {
            double gammap = double.NaN;
            double bsr = CashOrNothing(tpflg, S + ds, x, k, T, r, b, v);
            double bs = CashOrNothing(tpflg, S, x, k, T, r, b, v);
            double bsl = CashOrNothing(tpflg, S - ds, x, k, T, r, b, v);
            gammap = S / 100 * (bsr - 2 * bs + bsl) / (ds * ds);
            return gammap;
        }

        public static double FDA_Vega(string tpflg, double S, double x, double k, double T, double r, double b, double v, double ds)
        {
            double vega = double.NaN;
            double bsr = CashOrNothing(tpflg, S, x, k, T, r, b, v + 0.01);
            double bsl = CashOrNothing(tpflg, S, x, k, T, r, b, v - 0.01);
            vega = (bsr - bsl) / 2.0;
            return vega;
        }

        public static double FDA_Theta(string tpflg, double S, double x, double k, double T, double r, double b, double v, double ds)
        {
            double theta = double.NaN;
            double deltaT;
            if (T <= 1 / 252.0)
            {
                deltaT = 1 - 0.000005;
            }
            else
            {
                deltaT = 1 / 252.0;
            }
            double bsr = CashOrNothing(tpflg, S, x, k, T - deltaT, r, b, v);
            double bsl = CashOrNothing(tpflg, S, x, k, T, r, b, v);
            theta = bsr - bsl;
            return theta;
        }
    }

    public class BinaryMethodAssetOrNothing
    {

        //Util Functions
        private static double Log(double X) { return Math.Log(X); }
        private static double CND(double X) { return Normal.CDF(0, 1, X); }
        private static double Exp(double X) { return Math.Exp(X); }
        private static double Sqr(double x) { return Math.Sqrt(x); }


        public static double AssetOrNothing(string tpflg, double S, double x, double k, double T, double r, double b, double v)
        {
            double price = double.NaN;
            double d;
            d = (Log(S / x) + (b + v * v / 2) * T) / (v * Sqr(T));

            if (tpflg.Equals("c"))
            {
                price = S * Exp((b - r) * T) * CND(d);
            }
            else if (tpflg.Equals("p"))
            {
                price = S * Exp((b - r) * T) * CND(-d);
            }
            else
            {
                price = double.NaN;
            }
            return price;
        }

        public static double FDA_Delta(string tpflg, double S, double x, double k, double T, double r, double b, double v, double ds)
        {
            double delta = double.NaN;
            double bsr = AssetOrNothing(tpflg, S + ds, x, k, T, r, b, v);
            double bs = AssetOrNothing(tpflg, S, x, k, T, r, b, v);
            double bsl = AssetOrNothing(tpflg, S - ds, x, k, T, r, b, v);
            delta = (bsr - bsl) / (2 * ds);
            return delta;
        }

        public static double FDA_DeltaR(string tpflg, double S, double x, double k, double T, double r, double b, double v, double ds)
        {
            double delta = double.NaN;
            double bsr = AssetOrNothing(tpflg, S + ds, x, k, T, r, b, v);
            double bs = AssetOrNothing(tpflg, S, x, k, T, r, b, v);
            double bsl = AssetOrNothing(tpflg, S - ds, x, k, T, r, b, v);
            delta = (bsr - bs) / (ds);
            return delta;
        }

        public static double FDA_DeltaL(string tpflg, double S, double x, double k, double T, double r, double b, double v, double ds)
        {
            double delta = double.NaN;
            double bsr = AssetOrNothing(tpflg, S + ds, x, k, T, r, b, v);
            double bs = AssetOrNothing(tpflg, S, x, k, T, r, b, v);
            double bsl = AssetOrNothing(tpflg, S - ds, x, k, T, r, b, v);
            delta = (bs - bsl) / (ds);
            return delta;
        }

        public static double FDA_GammaP(string tpflg, double S, double x, double k, double T, double r, double b, double v, double ds)
        {
            double gammap = double.NaN;
            double bsr = AssetOrNothing(tpflg, S + ds, x, k, T, r, b, v);
            double bs = AssetOrNothing(tpflg, S, x, k, T, r, b, v);
            double bsl = AssetOrNothing(tpflg, S - ds, x, k, T, r, b, v);
            gammap = S / 100 * (bsr - 2 * bs + bsl) / (ds * ds);
            return gammap;
        }

        public static double FDA_Vega(string tpflg, double S, double x, double k, double T, double r, double b, double v, double ds)
        {
            double vega = double.NaN;
            double bsr = AssetOrNothing(tpflg, S, x, k, T, r, b, v + 0.01);
            double bsl = AssetOrNothing(tpflg, S, x, k, T, r, b, v - 0.01);
            vega = (bsr - bsl) / 2.0;
            return vega;
        }

        public static double FDA_Theta(string tpflg, double S, double x, double k, double T, double r, double b, double v, double ds)
        {
            double theta = double.NaN;
            double deltaT;
            if (T <= 1 / 252.0)
            {
                deltaT = 1 - 0.000005;
            }
            else
            {
                deltaT = 1 / 252.0;
            }
            double bsr = AssetOrNothing(tpflg, S, x, k, T - deltaT, r, b, v);
            double bsl = AssetOrNothing(tpflg, S, x, k, T, r, b, v);
            theta = bsr - bsl;
            return theta;
        }
    }
}
