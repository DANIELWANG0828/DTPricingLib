using MathNet.Numerics.Distributions;
using System;
namespace OptionPricingLib
{
    public class DoubleBarrierMethod
    {
        //Util Functions
        private static double Log(double X) { return Math.Log(X); }
        private static double CND(double X) { return Normal.CDF(0, 1, X); }
        private static double Exp(double X) { return Math.Exp(X); }
        private static double Sqr(double x) { return Math.Sqrt(x); }



        public static double DoubleBarrier(string TypeFlag, double S, double X, double l, double U, double T,
        double r, double b, double v, double delta1, double delta2)
        {
            double E, F, Sum1, Sum2, d1, d2, d3, d4, mu1, mu2, mu3, OutValue,price;
            OutValue = 0;
            price = double.NaN;
            F = U * Exp(delta1 * T);
            E = l * Exp(delta2 * T);
            Sum1 = 0;
            Sum2 = 0;
            if (TypeFlag == "co" | TypeFlag == "ci")
            {
                for (int n = -5; n < 5; n++)
                {
                    d1 = (Log(S * Math.Pow(U, 2 * n) / (X * Math.Pow(l, 2 * n))) + (b + v * v / 2) * T) / (v * Sqr(T));
                    d2 = (Log(S * Math.Pow(U, 2 * n) / (F * Math.Pow(l, 2 * n))) + (b + v * v / 2) * T) / (v * Sqr(T));
                    d3 = (Log(Math.Pow(l, 2 * n + 2) / (X * S * Math.Pow(U, 2 * n))) + (b + v * v / 2) * T) / (v * Sqr(T));
                    d4 = (Log(Math.Pow(l, 2 * n + 2) / (F * S * Math.Pow(U, 2 * n))) + (b + v * v / 2) * T) / (v * Sqr(T));
                    mu1 = 2 * (b - delta2 - n * (delta1 - delta2)) / (v * v) + 1;
                    mu2 = 2 * n * (delta1 - delta2) / (v * v);
                    mu3 = 2 * (b - delta2 + n * (delta1 - delta2)) / (v * v) + 1;
                    Sum1 = Sum1 + Math.Pow((Math.Pow(U, n) / Math.Pow(l, n)), mu1) * Math.Pow(l / S, mu2) * (CND(d1) - CND(d2)) - (Math.Pow(Math.Pow(l, n + 1) / (Math.Pow(U, n) * S), mu3)) * (CND(d3) - CND(d4));
                    Sum2 = Sum2 + Math.Pow((Math.Pow(U, n) / Math.Pow(l, n)), (mu1 - 2)) * Math.Pow(l / S, mu2) * (CND(d1 - v * Sqr(T)) - CND(d2 - v * Sqr(T))) - (Math.Pow(Math.Pow(l, n + 1) / (Math.Pow(U, n) * S), mu3 - 2) * (CND(d3 - v * Sqr(T)) - CND(d4 - v * Sqr(T))));
                }
                OutValue = S * Exp((b - r) * T) * Sum1 - X * Exp(-r * T) * Sum2;
            }
            else if (TypeFlag == "po" | TypeFlag == "pi")
            {
                for (int n = -5; n < 5; n++)
                {
                    d1 = (Log(S * Math.Pow(U, 2 * n) / (E * Math.Pow(l, 2 * n))) + (b + v * v / 2) * T) / (v * Sqr(T));
                    d2 = (Log(S * Math.Pow(U, 2 * n) / (X * Math.Pow(l, 2 * n))) + (b + v * v / 2) * T) / (v * Sqr(T));
                    d3 = (Log(Math.Pow(l, 2 * n + 2) / (E * S * Math.Pow(U, 2 * n))) + (b + v * v / 2) * T) / (v * Sqr(T));
                    d4 = (Log(Math.Pow(l, 2 * n + 2) / (X * S * Math.Pow(U, 2 * n))) + (b + v * v / 2) * T) / (v * Sqr(T));
                    mu1 = 2 * (b - delta2 - n * (delta1 - delta2)) / (v * v) + 1;
                    mu2 = 2 * n * (delta1 - delta2) / (v * v);
                    mu3 = 2 * (b - delta2 + n * (delta1 - delta2)) / (v * v) + 1;
                    Sum1 = Sum1 + Math.Pow((Math.Pow(U, n) / Math.Pow(l, n)), mu1) * Math.Pow(l / S, mu2) * (CND(d1) - CND(d2)) - (Math.Pow(Math.Pow(l, n + 1) / (Math.Pow(U, n) * S), mu3)) * (CND(d3) - CND(d4));
                    Sum2 = Sum2 + Math.Pow((Math.Pow(U, n) / Math.Pow(l, n)), (mu1 - 2)) * Math.Pow(l / S, mu2) * (CND(d1 - v * Sqr(T)) - CND(d2 - v * Sqr(T))) - (Math.Pow(Math.Pow(l, n + 1) / (Math.Pow(U, n) * S), mu3 - 2) * (CND(d3 - v * Sqr(T)) - CND(d4 - v * Sqr(T))));
                }
                OutValue = X * Exp(-r * T) * Sum2 - S * Exp((b - r) * T) * Sum1;

            }
            if (TypeFlag == "co" | TypeFlag == "po") { price = OutValue; }

            else if (TypeFlag == "ci") { price = BlackScholesMethod.BlackScholes("c", S, X, T, r, b, v) - OutValue; }
            else if (TypeFlag == "pi") { price = BlackScholesMethod.BlackScholes("p", S, X, T, r, b, v) - OutValue; }



            return price;
        }









        public static double FDA_Delta(string TypeFlag, double S, double X, double l, double U, double k, double T,
                                       double r, double b, double v, double ds)
        {
            double delta = double.NaN;
            double bsr = DoubleBarrier(TypeFlag, S + ds, X, l, U, k, T, r, b, v, ds);
            double bs = DoubleBarrier(TypeFlag, S, X, l, U, k, T, r, b, v, ds);
            double bsl = DoubleBarrier(TypeFlag, S - ds, X, l, U, k, T, r, b, v, ds);
            delta = (bsr - bsl) / (2 * ds);
            return delta;
        }

        public static double FDA_DeltaR(string TypeFlag, double S, double X, double l, double U, double k, double T,
                                       double r, double b, double v, double ds)
        {
            double delta = double.NaN;
            double bsr = DoubleBarrier(TypeFlag, S + ds, X, l, U, k, T, r, b, v, ds);
            double bs = DoubleBarrier(TypeFlag, S, X, l, U, k, T, r, b, v, ds);
            double bsl = DoubleBarrier(TypeFlag, S - ds, X, l, U, k, T, r, b, v, ds);
            delta = (bsr - bs) / (ds);
            return delta;
        }

        public static double FDA_DeltaL(string TypeFlag, double S, double X, double l, double U, double k, double T,
                                       double r, double b, double v, double ds)
        {
            double delta = double.NaN;
            double bsr = DoubleBarrier(TypeFlag, S + ds, X, l, U, k, T, r, b, v, ds);
            double bs = DoubleBarrier(TypeFlag, S, X, l, U, k, T, r, b, v, ds);
            double bsl = DoubleBarrier(TypeFlag, S - ds, X, l, U, k, T, r, b, v, ds);
            delta = (bs - bsl) / (ds);
            return delta;
        }

        public static double FDA_GammaP(string TypeFlag, double S, double X, double l, double U, double k, double T,
                                       double r, double b, double v, double ds)
        {
            double gammap = double.NaN;
            double bsr = DoubleBarrier(TypeFlag, S + ds, X, l, U, k, T, r, b, v, ds);
            double bs = DoubleBarrier(TypeFlag, S, X, l, U, k, T, r, b, v, ds);
            double bsl = DoubleBarrier(TypeFlag, S - ds, X, l, U, k, T, r, b, v, ds);
            gammap = S / 100 * (bsr - 2 * bs + bsl) / (ds * ds);
            return gammap;
        }

        public static double FDA_Vega(string TypeFlag, double S, double X, double l, double U, double k, double T,
                                       double r, double b, double v, double ds)
        {
            double vega = double.NaN;
            double bsr = DoubleBarrier(TypeFlag, S, X, l, U, k, T, r, b, v + 0.01, ds);
            double bsl = DoubleBarrier(TypeFlag, S, X, l, U, k, T, r, b, v - 0.01, ds);
            vega = (bsr - bsl) / 2.0;
            return vega;
        }

        public static double FDA_Theta(string TypeFlag, double S, double X, double l, double U, double k, double T,
                                       double r, double b, double v, double ds)
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
            double bsr = DoubleBarrier(TypeFlag, S, X, l, U, k, T - deltaT, r, b, v, ds);
            double bsl = DoubleBarrier(TypeFlag, S, X, l, U, k, T, r, b, v, ds);
            theta = bsr - bsl;
            return theta;
        }
    }
}
