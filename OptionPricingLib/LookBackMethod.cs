using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MathNet.Numerics.Distributions;
using Accord.Statistics.Distributions.Multivariate;

namespace OptionPricingLib
{
    public class FloatingStrikeLookBackMethod
    {
        private static double Log(double X) { return Math.Log(X); }
        private static double CND(double X) { return Normal.CDF(0, 1, X); }
        private static double Exp(double X) { return Math.Exp(X); }
        private static double Sqr(double x) { return Math.Sqrt(x); }

        public static double FloatingStrikeLookBackOption(string cpflg, double S, double S_realized, double T, double r, double b, double vol)
        {
            double price = double.NaN;
            if (cpflg == "c")
            {
                double S_min = S_realized;
                double a1 = (Log(S / S_min) + (b + vol * vol / 2) * T) / (vol * Sqr(T));
                double a2 = a1 - vol * Sqr(T);
                price = S * Exp((b - r) * T) * CND(a1) - S_min * Exp(-r * T) * CND(a2)
                    + S * Exp(-r * T) * vol * vol / 2 / b * (Math.Pow(S / S_min, -2 * b / vol / vol) * CND(-a1 + 2 * b / vol * Sqr(T)) - Exp(b * T) * CND(-a1));
            }
            else if (cpflg == "p")
            {
                double S_max = S_realized;
                double b1 = (Log(S / S_max) + (b + vol * vol / 2) * T) / (vol * Sqr(T));
                double b2 = b1 - vol * Sqr(T);
                price = -S * Exp((b - r) * T) * CND(-b1) + S_max * Exp(-r * T) * CND(-b2)
                    + S * Exp(-r * T) * vol * vol / 2 / b * (-Math.Pow(S / S_max, -2 * b / vol / vol) * CND(b1 - 2 * b / vol * Sqr(T)) + Exp(b * T) * CND(b1));
            }
            return price;
        }
        public static double FDA_Delta(string cpflg, double S, double S_realized, double T, double r, double b, double v, double ds)
        {
            double delta = double.NaN;
            double bsr = FloatingStrikeLookBackOption(cpflg, S + ds, S_realized, T, r, b, v);
            double bs = FloatingStrikeLookBackOption(cpflg, S, S_realized, T, r, b, v);
            double bsl = FloatingStrikeLookBackOption(cpflg, S - ds, S_realized, T, r, b, v);
            delta = (bsr - bsl) / (2 * ds);
            return delta;
        }

        
        public static double FDA_GammaP(string cpflg, double S, double S_realized, double T, double r, double b, double v, double ds)
        {
            double gammap = double.NaN;
            double bsr = FloatingStrikeLookBackOption(cpflg, S + ds, S_realized, T, r, b, v);
            double bs = FloatingStrikeLookBackOption(cpflg, S, S_realized, T, r, b, v);
            double bsl = FloatingStrikeLookBackOption(cpflg, S - ds, S_realized, T, r, b, v);
            gammap = S / 100 * (bsr - 2 * bs + bsl) / (ds * ds);
            return gammap;
        }

        public static double FDA_Vega(string cpflg, double S, double S_realized, double T, double r, double b, double v, double ds)
        {
            double vega = double.NaN;
            double bsr = FloatingStrikeLookBackOption(cpflg, S, S_realized, T, r, b, v + 0.01);
            double bsl = FloatingStrikeLookBackOption(cpflg, S, S_realized, T, r, b, v - 0.01);
            vega = (bsr - bsl) / 2.0;
            return vega;
        }

        public static double FDA_Theta(string cpflg, double S, double S_realized, double T, double r, double b, double v, double ds)
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
            double bsr = FloatingStrikeLookBackOption(cpflg, S, S_realized, T - deltaT, r, b, v);
            double bsl = FloatingStrikeLookBackOption(cpflg, S, S_realized, T, r, b, v);
            theta = bsr - bsl;
            return theta;
        }


    }


    public class FixedStrikeLookBackMethod
    {
        private static double Log(double X) { return Math.Log(X); }
        private static double CND(double X) { return Normal.CDF(0, 1, X); }
        private static double Exp(double X) { return Math.Exp(X); }
        private static double Sqr(double x) { return Math.Sqrt(x); }

        public static double FixedStrikeLookBackCall(string cpflg, double S, double S_realized, double X, double T, double r, double b, double vol)
        {

            double price = double.NaN;
            if (cpflg == "c")
            {
                double S_max = S_realized;
                if (X > S_max)
                {
                    double d1 = (Log(S / X) + (b + vol * vol / 2) * T) / (vol * Sqr(T));
                    double d2 = d1 - vol * Sqr(T);
                    price = S * Exp((b - r) * T) * CND(d1) - X * Exp(-r * T) * CND(d2)
                        + S * Exp(-r * T) * vol * vol / 2 / b * (-Math.Pow(S / X, -2 * b / vol / vol) * CND(d1 - 2 * b / vol * Sqr(T)) - Exp(b * T) * CND(d1));
                }
                else
                {
                    double e1 = (Log(S / S_max) + (b + vol * vol / 2) * T) / (vol * Sqr(T));
                    double e2 = e1 - vol * Sqr(T);
                    price = (S_max - X) * Exp(-r * T) + S * Exp((b - r) * T) * CND(e1) - S_max * Exp(-r * T) * CND(e2)
                        + S * Exp(-r * T) * vol * vol / 2 / b * (-Math.Pow(S / S_max, -2 * b / vol / vol) * CND(e1 - 2 * b / vol * Sqr(T)) + Exp(b * T) * CND(e1));
                }

            }
            else if (cpflg == "p")
            {
                double S_min = S_realized;
                if (X < S_min)
                {
                    double d1 = (Log(S / X) + (b + vol * vol / 2) * T) / (vol * Sqr(T));
                    double d2 = d1 - vol * Sqr(T);
                    price = -S * Exp((b - r) * T) * CND(-d1) + X * Exp(-r * T) * CND(-d2)
                        + S * Exp(-r * T) * vol * vol / 2 / b * (Math.Pow(S / X, -2 * b / vol / vol) * CND(-d1 + 2 * b / vol * Sqr(T)) - Exp(b * T) * CND(-d1));
                }
                else
                {
                    double f1 = (Log(S / X) + (b + vol * vol / 2) * T) / (vol * Sqr(T));
                    double f2 = f1 - vol * Sqr(T);
                    price = (X - S_min) * Exp(-r * T) - S * Exp((b - r) * T) * CND(-f1) + S_min * Exp(-r * T) * CND(-f2)
                        + S * Exp(-r * T) * vol * vol / 2 / b * (Math.Pow(S / S_min, -2 * b / vol / vol) * CND(-f1 + 2 * b / vol * Sqr(T)) - Exp(b * T) * CND(-f1));

                }

            }
            return price;
        }

        public static double FDA_Delta(string cpflg, double S, double S_realized, double X, double T, double r, double b, double v, double ds)
        {
            double delta = double.NaN;
            double bsr = FixedStrikeLookBackCall(cpflg, S + ds, S_realized, X, T, r, b, v);
            double bs = FixedStrikeLookBackCall(cpflg, S, S_realized, X, T, r, b, v);
            double bsl = FixedStrikeLookBackCall(cpflg, S - ds, S_realized, X, T, r, b, v);
            delta = (bsr - bsl) / (2 * ds);
            return delta;
        }


        public static double FDA_GammaP(string cpflg, double S, double S_realized, double X, double T, double r, double b, double v, double ds)
        {
            double gammap = double.NaN;
            double bsr = FixedStrikeLookBackCall(cpflg, S + ds, S_realized, X, T, r, b, v);
            double bs = FixedStrikeLookBackCall(cpflg, S, S_realized, X, T, r, b, v);
            double bsl = FixedStrikeLookBackCall(cpflg, S - ds, S_realized, X, T, r, b, v);
            gammap = S / 100 * (bsr - 2 * bs + bsl) / (ds * ds);
            return gammap;
        }

        public static double FDA_Vega(string cpflg, double S, double S_realized, double X, double T, double r, double b, double v, double ds)
        {
            double vega = double.NaN;
            double bsr = FixedStrikeLookBackCall(cpflg, S, S_realized, X, T, r, b, v + 0.01);
            double bsl = FixedStrikeLookBackCall(cpflg, S, S_realized, X, T, r, b, v - 0.01);
            vega = (bsr - bsl) / 2.0;
            return vega;
        }

        public static double FDA_Theta(string cpflg, double S, double S_realized, double X, double T, double r, double b, double v, double ds)
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
            double bsr = FixedStrikeLookBackCall(cpflg, S, S_realized, X, T - deltaT, r, b, v);
            double bsl = FixedStrikeLookBackCall(cpflg, S, S_realized, X, T, r, b, v);
            theta = bsr - bsl;
            return theta;
        }
    }


    public class PartialTimeFloatingStrikeLookBackMethod
    {
        private static double Log(double X) { return Math.Log(X); }
        private static double CND(double X) { return Normal.CDF(0, 1, X); }
        private static double Exp(double X) { return Math.Exp(X); }
        private static double Sqr(double x) { return Math.Sqrt(x); }
        private static double M(double x, double y, double rho)
        { return MultivariateNormalDistribution.Bivariate(0, 0, 1, 1, rho).DistributionFunction(new double[] { x, y }); }
        public static double PartialTimeFloatingStrikeLookBackOption(string cpflg, double S, double lambda, double S_realized, double t1, double T2, double r, double b, double vol)
        {
            double price = double.NaN;
            if (cpflg == "c")
            {
                double S_min = S_realized;
                double d1 = (Log(S / S_min) + (b + vol * vol / 2) * T2) / (vol * Sqr(T2));
                double d2 = d1 - vol * Sqr(T2);
                double e1 =  (b + vol * vol / 2) * (T2-t1) / (vol * Sqr(T2-t1));
                double e2 = d1 - vol * Sqr(T2-t1);
                double f1 = (Log(S / S_min) + (b + vol * vol / 2) * t1) / (vol * Sqr(t1));
                double f2 = f1 - vol * Sqr(t1);
                double g1 = Log(lambda) / vol / Sqr(T2);
                double g2 = Log(lambda) / vol / Sqr(T2-t1);
                price = S * Exp((b - r) * T2) * CND(d1 - g1) - lambda * S_min * Exp(-r * T2) * CND(d2 - g2)
                    + lambda * S * Exp(-r * T2) * vol * vol / 2 / b * (Math.Pow(S / S_min, -2 * b / vol / vol) * M(-f1 + 2 * b * Sqr(t1) / vol, -d1 + 2 * b * Sqr(T2) / vol - g1, Sqr(t1 / T2))
                    - Exp(b * T2) * lambda * Math.Pow(lambda, 2 * b / vol / vol) * M(-d1 - g1, e1 + g2, -Sqr(1 - t1 / T2)))
                    + S * Exp((b - r) * T2) * M(-d1 + g1, e1 - g2, -Sqr(t1 / T2))
                    + lambda * S_min * Exp(-r * T2) * M(-f2, d2 - g1, -Sqr(t1 / T2))
                    - Exp(-b * (T2 - t1)) * (1 + vol * vol / 2 / b) * lambda * S * Exp((b - r) * T2) * CND(e2 - g2) * CND(-f1);

            }
            else if (cpflg == "p")
            {
                double S_max = S_realized;
                double d1 = (Log(S / S_max) + (b + vol * vol / 2) * T2) / (vol * Sqr(T2));
                double d2 = d1 - vol * Sqr(T2);
                double e1 = (b + vol * vol / 2) * (T2 - t1) / (vol * Sqr(T2 - t1));
                double e2 = d1 - vol * Sqr(T2 - t1);
                double f1 = (Log(S / S_max) + (b + vol * vol / 2) * t1) / (vol * Sqr(t1));
                double f2 = f1 - vol * Sqr(t1);
                double g1 = Log(lambda) / vol / Sqr(T2);
                double g2 = Log(lambda) / vol / Sqr(T2 - t1);
                price = -S * Exp((b - r) * T2) * CND(-d1 + g1) + lambda * S_max * Exp(-r * T2) * CND(-d2 + g1)
                    + lambda * S * Exp(-r * T2) * vol * vol / 2 / b * (-Math.Pow(S / S_max, -2 * b / vol / vol) * M(f1 - 2 * b * Sqr(t1) / vol, d1 - 2 * b * Sqr(T2) / vol + g1, Sqr(t1 / T2))
                    + Exp(b * T2) * lambda * Math.Pow(lambda, 2 * b / vol / vol) * M(d1 + g1, -e1 - g2, -Sqr(1 - t1 / T2)))
                    - S * Exp((b - r) * T2) * M(d1 - g1, -e1 + g2, -Sqr(t1 / T2))
                    - lambda * S_max * Exp(-r * T2) * M(f2, -d2 + g1, -Sqr(t1 / T2))
                    + Exp(-b * (T2 - t1)) * (1 + vol * vol / 2 / b) * lambda * S * Exp((b - r) * T2) * CND(-e2 + g2) * CND(f1);
            }
            return price;
        }


    }


    public class PartialTimeFixedStrikeLookBackMethod
    {
        private static double Log(double X) { return Math.Log(X); }
        private static double CND(double X) { return Normal.CDF(0, 1, X); }
        private static double Exp(double X) { return Math.Exp(X); }
        private static double Sqr(double x) { return Math.Sqrt(x); }
        private static double M(double x, double y, double rho)
        { return MultivariateNormalDistribution.Bivariate(0, 0, 1, 1, rho).DistributionFunction(new double[] { x, y }); }
        public static double PartialTimeFixedStrikeLookBackOption(string cpflg, double S, double X, double t1, double T2, double r, double b, double vol)
        {

            double price = double.NaN;
            if (cpflg == "c")
            {
                double S_min = X;
                double d1 = (Log(S / S_min) + (b + vol * vol / 2) * T2) / (vol * Sqr(T2));
                double d2 = d1 - vol * Sqr(T2);
                double e1 = (b + vol * vol / 2) * (T2 - t1) / (vol * Sqr(T2 - t1));
                double e2 = d1 - vol * Sqr(T2 - t1);
                double f1 = (Log(S / S_min) + (b + vol * vol / 2) * t1) / (vol * Sqr(t1));
                double f2 = f1 - vol * Sqr(t1);
                price = S * Exp((b - r) * T2) * CND(d1) - X * Exp(-r * T2) * CND(d2)
                    + S * Exp(-r * T2) * vol * vol / 2 / b * (-Math.Pow(S / X, -2 * b / vol / vol) * M(d1 - 2 * b * Sqr(t1) / vol, -f1 + 2 * b * Sqr(T2) / vol, -Sqr(t1 / T2))
                    + Exp(b * T2) * M(e1, d1, Sqr(1 - t1 / T2)))
                    - S * Exp((b - r) * T2) * M(-e1, d1, -Sqr(1- t1 / T2))
                    - X * Exp(-r * T2) * M(f2, -d2, -Sqr(t1 / T2))
                    + Exp(-b * (T2 - t1)) * (1 - vol * vol / 2 / b) * S * Exp((b - r) * T2) * CND(f1) * CND(-e2);

            }
            else if (cpflg == "p")
            {
                double S_max = X;
                double d1 = (Log(S / S_max) + (b + vol * vol / 2) * T2) / (vol * Sqr(T2));
                double d2 = d1 - vol * Sqr(T2);
                double e1 = (b + vol * vol / 2) * (T2 - t1) / (vol * Sqr(T2 - t1));
                double e2 = d1 - vol * Sqr(T2 - t1);
                double f1 = (Log(S / S_max) + (b + vol * vol / 2) * t1) / (vol * Sqr(t1));
                double f2 = f1 - vol * Sqr(t1);
                price = -S * Exp((b - r) * T2) * CND(-d1) + X * Exp(-r * T2) * CND(-d2)
                    + S * Exp(-r * T2) * vol * vol / 2 / b * (Math.Pow(S / X, -2 * b / vol / vol) * M(-d1 + 2 * b * Sqr(t1) / vol, f1 - 2 * b * Sqr(T2) / vol, -Sqr(t1 / T2))
                    - Exp(b * T2) * M(-e1, -d1, Sqr(1 - t1 / T2)))
                    + S * Exp((b - r) * T2) * M(e1, -d1, -Sqr(1 - t1 / T2))
                    + X * Exp(-r * T2) * M(-f2, d2, -Sqr(t1 / T2))
                    - Exp(-b * (T2 - t1)) * (1 - vol * vol / 2 / b) * S * Exp((b - r) * T2) * CND(-f1) * CND(e2);
            }
            return price;
        }

    }

    public class ExtremeSpreadMethod
    {
        private static double Log(double X) { return Math.Log(X); }
        private static double CND(double X) { return Normal.CDF(0, 1, X); }
        private static double CND(double eta, double X) { return Normal.CDF(0, 1, eta*X); }
        private static double Exp(double X) { return Math.Exp(X); }
        private static double Sqr(double x) { return Math.Sqrt(x); }
        public static double ExtremeSpreadOption(string cpflg, string ReverseOrNot, double S, double lambda, double SRealizedMin, double SRealizedMax, double t1, double T2, double r, double b, double vol)
        {
            int eta,phi;
            if (cpflg == "c") { eta = 1; }
            else { eta = -1; }
            if (ReverseOrNot == "T") { phi = 1; }
            else { phi = -1; }
            double M0;
            double price = double.NaN;
            if (phi * eta == 1) { M0 = SRealizedMax; }
            else { M0 = SRealizedMin; }
            double m = Log(M0 / S);
            double mu1 = b - vol * vol / 2;
            double mu2 = b + vol * vol / 2;
            price = eta * (S * Exp((b - r) * T2) * (1 + vol * vol / 2 / b) * CND(eta, (-m + mu2 * T2) / vol / Sqr(T2)) - Exp(-b * (T2 - t1)) * S * Exp((b - r) * T2) *
                (1 + vol * vol / 2 / b) * CND(eta, (-m + mu2 * t1) / vol / Sqr(t1)) + Exp(-r * T2) * M0 * CND(eta, (-m + mu1 * T2) / vol / Sqr(T2))
                - Exp(-r * T2) * M0 * vol * vol / 2 / b * Exp(2 * mu1 * m / vol / vol) * CND(eta, (-m - mu1 * T2) / vol / Sqr(T2))
                - Exp(-r * T2) * M0 * CND(eta, (m - mu1 * t1) / vol / Sqr(t1))
                + Exp(-r * T2) * M0 * vol * vol / 2 / b * Exp(2 * mu1 * m / vol / vol) * CND(eta, (-m - mu1 * t1) / vol / Sqr(t1)));
            return price;

        }

    }


    public class LookBackBarrierMethod
    {
        private static double Log(double X) { return Math.Log(X); }
        private static double CND(double X) { return Normal.CDF(0, 1, X); }
        private static double Exp(double X) { return Math.Exp(X); }
        private static double Sqr(double x) { return Math.Sqrt(x); }
        private static double M(double x, double y, double rho)
        { return MultivariateNormalDistribution.Bivariate(0, 0, 1, 1, rho).DistributionFunction(new double[] { x, y }); }
        public static double LookBackBarrierOption(string TypeFlag, string ReverseOrNot, double S, double X, double h, double t1, double T2, double r, double b, double vol)
        {
            double hh, k, mu1, mu2, rho, eta, m, g1, g2, OutValue, part1, part2, part3, part4;
            hh = Log(h / S);
            k = Log(X / S);
            mu1 = b - vol * vol / 2;
            mu2 = b + vol * vol / 2;
            rho = Sqr(t1 / T2);
            eta = 1;

            
            if (TypeFlag == "cuo" | TypeFlag == "cui")
            { eta = 1;
                m = Math.Min(hh, k);
            }
            else if (TypeFlag == "pdo" | TypeFlag == "pdi")
            { eta = -1;
                m = Math.Max(hh, k);
            }
            else
            { m = double.NaN; }


            g1 = (CND(eta * (hh - mu2 * t1) / (vol * Sqr(t1))) - Exp(2 * mu2 * hh / vol * vol) * CND(eta * (-hh - mu2 * t1) / (vol* Sqr(t1))) ) 
                - (CND(eta * (m - mu2 * t1) / (vol * Sqr(t1))) - Exp(2 * mu2 * hh / vol * vol) * CND(eta * (m - 2 * hh - mu2 * t1) / (vol * Sqr(t1))));
            g2 = (CND(eta * (hh - mu1 * t1) / (vol * Sqr(t1))) - Exp(2 * mu1 * hh / vol * vol) * CND(eta * (-hh - mu1 * t1) / (vol * Sqr(t1)))) 
                - (CND(eta * (m - mu1 * t1) / (vol * Sqr(t1))) - Exp(2 * mu1 * hh / vol * vol) * CND(eta * (m - 2 * hh - mu1 * t1) / (vol * Sqr(t1))));

            part1 = S * Exp((b - r) * T2) * (1 + vol * vol / (2 * b)) * (M(eta * (m - mu2 * t1) / (vol * Sqr(t1)), eta * (-k + mu2 * T2) / (vol * Sqr(T2)), -rho) - Exp(2 * mu2 * hh / vol * vol) 
                * M(eta* (m -2 * hh - mu2 * t1) / (vol * Sqr(t1)), eta * (2 * hh - k + mu2 * T2) / (vol * Sqr(T2)), -rho));
            part2 = -Exp(-r * T2) * X * (M(eta * (m - mu1 * t1) / (vol * Sqr(t1)), eta * (-k + mu1 * T2) / (vol * Sqr(T2)), -rho) 
                - Exp(2 * mu1 * hh / vol * vol) * M(eta * (m - 2 * hh - mu1 * t1) / (vol * Sqr(t1)), eta * (2 * hh - k + mu1 * T2) / (vol * Sqr(T2)), -rho));
            part3 = -Exp(-r * T2) * vol * vol / (2 * b) * Math.Pow(S * (S / X) , -2 * b / vol * vol) * M(eta * (m + mu1 * t1) / (vol * Sqr(t1)), eta * (-k - mu1 * T2) / (vol * Sqr(T2)), -rho) 
                - Math.Pow(h * (h / X) , -2 * b / vol * vol) * M(eta * (m - 2 * hh + mu1 * t1) / (vol * Sqr(t1)), eta * (2 * hh - k - mu1 * T2) / (vol * Sqr(T2)), -rho);
            part4 = S * Exp((b - r) * T2) * ((1 + vol * vol / (2 * b)) * CND(eta * mu2 * (T2 - t1) / (vol * Sqr(T2 - t1))) + Exp(-b * (T2 - t1)) * (1 - vol * vol / (2 * b))
                * CND(eta * (-mu1 * (T2 - t1)) / (vol * Sqr(T2 - t1)))) * g1 - Exp(-r * T2) * X * g2;
            OutValue = eta * (part1 + part2 + part3 + part4);

            double price = double.NaN;
            if (TypeFlag == "cuo" | TypeFlag == "pdo")
                price = OutValue;
            else if (TypeFlag == "cui")
                price = PartialTimeFixedStrikeLookBackMethod.PartialTimeFixedStrikeLookBackOption("c", S, X, t1, T2, r, b, vol) - OutValue;
            else if (TypeFlag == "pdi")
                price = PartialTimeFixedStrikeLookBackMethod.PartialTimeFixedStrikeLookBackOption("p", S, X, t1, T2, r, b, vol) - OutValue;
            return price;

        }

    }


}