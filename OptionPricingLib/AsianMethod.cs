using MathNet.Numerics.Distributions;
using System;


namespace OptionPricingLib
{
    public class AsianMethod
    {
        //Util Functions
        private static double Log(double X) { return Math.Log(X); }
        private static double CND(double X) { return Normal.CDF(0, 1, X); }
        private static double Exp(double X) { return Math.Exp(X); }
        private static double Sqr(double x) { return Math.Sqrt(x); }


        public static double DiscreteAsianHHM(string cpflg, double S, double SA, double X,
              double t1, double T, double n, double m, double r, double b, double v)
        {
            double d1, d2, h, EA, EA2, vA, OptionValue, SA1, X1, price;
            SA1 = SA;
            X1 = X;
            h = (T - t1) / (n - 1);
            if (b == 0) { EA = S; }
            else { EA = S / n * Exp(b * t1) * (1 - Exp(b * h * n)) / (1 - Exp(b * h)); }

            if (m > 0)
            {
                if (SA > n / m * X)
                {
                    if (cpflg == "p")
                    {
                        price = 0;
                        return price;
                    }
                    else if (cpflg == "c")
                    {
                        SA = SA * m / n + EA * (n - m) / n;
                        price = (SA - X) * Exp(-r * T);
                        return price;
                    }
                    SA = SA1;
                }
            }
            if (m == n - 1)
            {
                X = n * X - (n - 1) * SA;
                price = BlackScholesMethod.BlackScholes(cpflg, S, X, T, r, b, v) * 1 / n;
                X = X1;
                SA = SA1;
                return price;
            }
            if (b == 0)
            {
                EA2 = S * S * Exp(v * v * t1) / (n * n)
              * ((1 - Exp(v * v * h * n)) / (1 - Exp(v * v * h))
             + 2 / (1 - Exp(v * v * h)) * (n - (1 - Exp(v * v * h * n))));
            }
            else
            {
                EA2 = S * S * Exp((2 * b + v * v) * t1) / (n * n)
               * ((1 - Exp((2 * b + v * v) * h * n)) / (1 - Exp((2 * b + v * v) * h))
               + 2 / (1 - Exp((b + v * v) * h)) * ((1 - Exp(b * h * n)) / (1 - Exp(b * h))
               - (1 - Exp((2 * b + v * v) * h * n)) /
               (1 - Exp((2 * b + v * v) * h))));
            }
            vA = Sqr((Log(EA2) - 2 * Log(EA)) / T);
            OptionValue = 0;
            if (m > 0)
            {
                X = n / (n - m) * X - m / (n - m) * SA;
            }
            d1 = (Log(EA / X) + vA * vA / 2 * T) / (vA * Sqr(T));
            d2 = d1 - vA * Sqr(T);
            if (cpflg == "c")
            {
                OptionValue = Exp(-r * T) * (EA * CND(d1) - X * CND(d2));
            }
            else if (cpflg == "p")
            {
                OptionValue = Exp(-r * T) * (X * CND(-d2) - EA * CND(-d1));
            }

            price = OptionValue * (n - m) / n;
            return price;
        }

        
        public static double FDA_Delta(string cpflg, double S, double SA, double X,
              double t1, double T, double n, double m, double r, double b, double v, double ds)
        {
            double delta = double.NaN;
            double bsr = DiscreteAsianHHM(cpflg, S+ds, SA, X, t1, T, n, m, r, b, v);
            double bs = DiscreteAsianHHM(cpflg, S, SA, X, t1, T, n, m, r, b, v);
            double bsl = DiscreteAsianHHM(cpflg, S-ds, SA, X, t1, T, n, m, r, b, v);
            delta = (bsr - bsl) / (2 * ds);
            return delta;
        }

        public static double FDA_DeltaR(string cpflg, double S, double SA, double X,
              double t1, double T, double n, double m, double r, double b, double v, double ds)
        {
            double delta = double.NaN;
            double bsr = DiscreteAsianHHM(cpflg, S + ds, SA, X, t1, T, n, m, r, b, v);
            double bs = DiscreteAsianHHM(cpflg, S, SA, X, t1, T, n, m, r, b, v);
            double bsl = DiscreteAsianHHM(cpflg, S - ds, SA, X, t1, T, n, m, r, b, v);
            delta = (bsr - bs) / (ds);
            return delta;
        }

        public static double FDA_DeltaL(string cpflg, double S, double SA, double X,
              double t1, double T, double n, double m, double r, double b, double v, double ds)
        {
            double delta = double.NaN;
            double bsr = DiscreteAsianHHM(cpflg, S + ds, SA, X, t1, T, n, m, r, b, v);
            double bs = DiscreteAsianHHM(cpflg, S, SA, X, t1, T, n, m, r, b, v);
            double bsl = DiscreteAsianHHM(cpflg, S - ds, SA, X, t1, T, n, m, r, b, v);
            delta = (bs - bsl) / (ds);
            return delta;
        }

        public static double FDA_GammaP(string cpflg, double S, double SA, double X,
              double t1, double T, double n, double m, double r, double b, double v, double ds)
        {
            double gammap = double.NaN;
            double bsr = DiscreteAsianHHM(cpflg, S + ds, SA, X, t1, T, n, m, r, b, v);
            double bs = DiscreteAsianHHM(cpflg, S, SA, X, t1, T, n, m, r, b, v);
            double bsl = DiscreteAsianHHM(cpflg, S - ds, SA, X, t1, T, n, m, r, b, v);
            gammap = S / 100 * (bsr - 2 * bs + bsl) / (ds * ds);
            return gammap;
        }

        public static double FDA_Vega(string cpflg, double S, double SA, double X,
              double t1, double T, double n, double m, double r, double b, double v, double ds)
        {
            double vega = double.NaN;
            double bsr = DiscreteAsianHHM(cpflg, S, SA, X, t1, T, n, m, r, b, v + 0.01);
            double bsl = DiscreteAsianHHM(cpflg, S, SA, X, t1, T, n, m, r, b, v - 0.01);
            vega = (bsr - bsl) / 2.0;
            return vega;
        }

        public static double FDA_Theta(string cpflg, double S, double SA, double X,
              double t1, double T, double n, double m, double r, double b, double v, double ds)
        {
            double theta = double.NaN;
            double deltaT;
            if (T <= 1 / 252)
            {
                deltaT = 1 - 0.000005;
            }
            else
            {
                deltaT = 1 / 252;
            }
            double bsr = DiscreteAsianHHM(cpflg, S, SA, X, t1 - deltaT, T - deltaT, n, m, r, b, v);
            double bsl = DiscreteAsianHHM(cpflg, S, SA, X, t1, T, n, m, r, b, v);
            theta = bsr - bsl;
            return theta;
        }

    }
}
