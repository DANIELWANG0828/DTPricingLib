using MathNet.Numerics.Distributions;
using System;

namespace OptionPricingLib
{
    public class ThreeAssetsSpreadApproxMethod
    {
        //Util Functions
        private static double Log(double X) { return Math.Log(X); }
        private static double CND(double X) { return Normal.CDF(0, 1, X); }
        private static double Exp(double X) { return Math.Exp(X); }
        private static double Sqr(double x) { return Math.Sqrt(x); }

        public static double Pricer(string cpflg, double S1, double S2, double S3, double Q1, double Q2, double Q3, double X, double T,
               double r, double b1, double b2, double b3, double v1, double v2, double v3, double rho1, double rho2, double rho3)
        {
            double F1, F2, F3, LogF;
            double temp1, temp2, temp3;
            double sigma;
            double d1, d2, d3, d4;
            double price = double.NaN;

            F1 = Q1 * S1 * Exp(b1 - r) * T;
            F2 = Q2 * S2 * Exp(b2 - r) * T;
            F3 = Q3 * S3 * Exp(b3 - r) * T;

            d1 = F2 + F3 + X * Exp(-r) * T;
            d2 = F2 / d1;
            d3 = F3 / d1;
            temp1 = v1 * v1 + d2 * d2 * v2 * v2 + d3 * d3 * v3 * v3;
            temp2 = d2 * v1 * v2 * rho1 + d3 * v1 * v3 * rho2 - d2 * d3 * v2 * v3 * rho3;
            temp3 = 0.5 * (d2 * d2 * v2 * v2 + d3 * d3 * v3 * v3 - v1 * v1);
            sigma = Sqr(temp1 - 2 * temp2);
            LogF = Math.Log(F1 / d1);
            d1 = (LogF + (0.5 * temp1 - temp2) * T) / (sigma * Sqr(T));
            d2 = (LogF + (temp3 - d2 * v2 * v2 + d2 * d3 * v2 * v3 * rho3 - d3 * v2 * v3 * rho3 + v1 * v2 * rho1) * T) / (sigma * Sqr(T));
            d3 = (LogF + (temp3 - d3 * v3 * v3 + d2 * d3 * v2 * v3 * rho3 - d2 * v2 * v3 * rho3 + v1 * v3 * rho2) * T) / (sigma * Sqr(T));
            d4 = (LogF + (temp3 + d2 * d3 * v2 * v3 * rho3) * T) / (sigma * Sqr(T));
            if (cpflg.Equals("c"))
            {
                price = F1 * CND(d1) - F2 * CND(d2) - F3 * CND(d3) - X * CND(d4) * Exp(-r * T);
            }
            else if (cpflg.Equals("p"))
            {
                price = -F1 * CND(-d1) + F2 * CND(-d2) + F3 * CND(-d3) + X * CND(-d4) * Exp(-r * T);
            }
            else
            {
                price = double.NaN;
            }
            return price;
        }

        public static double FdDelta1(string cpflg, double S1, double S2, double S3, double Q1, double Q2, double Q3, double X, double T,
                double r, double b1, double b2, double b3, double v1, double v2, double v3, double rho1, double rho2, double rho3, double dS)
        {
            double result = double.NaN;
            result = (Pricer(cpflg, S1 + dS, S2, S3, Q1, Q2, Q3, X, T, r, b1, b2, b3, v1, v2, v3, rho1, rho2, rho3)
                    - Pricer(cpflg, S1 - dS, S2, S3, Q1, Q2, Q3, X, T, r, b1, b2, b3, v1, v2, v3, rho1, rho2, rho3)) / (2 * dS);
            return result;
        }

        public static double FdDelta2(string cpflg, double S1, double S2, double S3, double Q1, double Q2, double Q3, double X, double T,
                double r, double b1, double b2, double b3, double v1, double v2, double v3, double rho1, double rho2, double rho3, double dS)
        {
            double result = double.NaN;
            result = (Pricer(cpflg, S1, S2 + dS, S3, Q1, Q2, Q3, X, T, r, b1, b2, b3, v1, v2, v3, rho1, rho2, rho3)
                    - Pricer(cpflg, S1, S2 - dS, S3, Q1, Q2, Q3, X, T, r, b1, b2, b3, v1, v2, v3, rho1, rho2, rho3)) / (2 * dS);
            return result;
        }
        public static double FdDelta3(string cpflg, double S1, double S2, double S3, double Q1, double Q2, double Q3, double X, double T,
        double r, double b1, double b2, double b3, double v1, double v2, double v3, double rho1, double rho2, double rho3, double dS)
        {
            double result = double.NaN;
            result = (Pricer(cpflg, S1, S2, S3 + dS, Q1, Q2, Q3, X, T, r, b1, b2, b3, v1, v2, v3, rho1, rho2, rho3)
                    - Pricer(cpflg, S1, S2, S3 - dS, Q1, Q2, Q3, X, T, r, b1, b2, b3, v1, v2, v3, rho1, rho2, rho3)) / (2 * dS);
            return result;
        }

        public static double FdGammaP1(string cpflg, double S1, double S2, double S3, double Q1, double Q2, double Q3, double X, double T,
                double r, double b1, double b2, double b3, double v1, double v2, double v3, double rho1, double rho2, double rho3, double dS)
        {
            double result = double.NaN;
            result = S1 / 100 * (Pricer(cpflg, S1 + dS, S2, S3, Q1, Q2, Q3, X, T, r, b1, b2, b3, v1, v2, v3, rho1, rho2, rho3)
                - 2 * Pricer(cpflg, S1, S2, S3, Q1, Q2, Q3, X, T, r, b1, b2, b3, v1, v2, v3, rho1, rho2, rho3)
                + Pricer(cpflg, S1 - dS, S2, S3, Q1, Q2, Q3, X, T, r, b1, b2, b3, v1, v2, v3, rho1, rho2, rho3)) / (dS * dS);
            return result;
        }

        public static double FdGammaP2(string cpflg, double S1, double S2, double S3, double Q1, double Q2, double Q3, double X, double T,
        double r, double b1, double b2, double b3, double v1, double v2, double v3, double rho1, double rho2, double rho3, double dS)
        {
            double result = double.NaN;
            result = S1 / 100 * (Pricer(cpflg, S1, S2 + dS, S3, Q1, Q2, Q3, X, T, r, b1, b2, b3, v1, v2, v3, rho1, rho2, rho3)
                - 2 * Pricer(cpflg, S1, S2, S3, Q1, Q2, Q3, X, T, r, b1, b2, b3, v1, v2, v3, rho1, rho2, rho3)
                + Pricer(cpflg, S1, S2 - dS, S3, Q1, Q2, Q3, X, T, r, b1, b2, b3, v1, v2, v3, rho1, rho2, rho3)) / (dS * dS);
            return result;
        }

        public static double FdGammaP3(string cpflg, double S1, double S2, double S3, double Q1, double Q2, double Q3, double X, double T,
        double r, double b1, double b2, double b3, double v1, double v2, double v3, double rho1, double rho2, double rho3, double dS)
        {
            double result = double.NaN;
            result = S1 / 100 * (Pricer(cpflg, S1, S2, S3 + dS, Q1, Q2, Q3, X, T, r, b1, b2, b3, v1, v2, v3, rho1, rho2, rho3)
                - 2 * Pricer(cpflg, S1, S2, S3, Q1, Q2, Q3, X, T, r, b1, b2, b3, v1, v2, v3, rho1, rho2, rho3)
                + Pricer(cpflg, S1, S2, S3 - dS, Q1, Q2, Q3, X, T, r, b1, b2, b3, v1, v2, v3, rho1, rho2, rho3)) / (dS * dS);
            return result;
        }


        public static double FdVega1(string cpflg, double S1, double S2, double S3, double Q1, double Q2, double Q3, double X, double T,
        double r, double b1, double b2, double b3, double v1, double v2, double v3, double rho1, double rho2, double rho3, double dS)
        {
            double result = double.NaN;
            double dv = 0.01;
            result = (Pricer(cpflg, S1, S2, S3 + dS, Q1, Q2, Q3, X, T, r, b1, b2, b3, v1 + dv, v2, v3, rho1, rho2, rho3)
                - Pricer(cpflg, S1, S2, S3 + dS, Q1, Q2, Q3, X, T, r, b1, b2, b3, v1 - dv, v2, v3, rho1, rho2, rho3)) / 2;
            return result;
        }

        public static double FdVega2(string cpflg, double S1, double S2, double S3, double Q1, double Q2, double Q3, double X, double T,
        double r, double b1, double b2, double b3, double v1, double v2, double v3, double rho1, double rho2, double rho3, double dS)
        {
            double result = double.NaN;
            double dv = 0.01;
            result = (Pricer(cpflg, S1, S2, S3 + dS, Q1, Q2, Q3, X, T, r, b1, b2, b3, v1, v2 + dv, v3, rho1, rho2, rho3)
                - Pricer(cpflg, S1, S2, S3 + dS, Q1, Q2, Q3, X, T, r, b1, b2, b3, v1, v2 - dv, v3, rho1, rho2, rho3)) / 2;
            return result;
        }

        public static double FdVega3(string cpflg, double S1, double S2, double S3, double Q1, double Q2, double Q3, double X, double T,
        double r, double b1, double b2, double b3, double v1, double v2, double v3, double rho1, double rho2, double rho3, double dS)
        {
            double result = double.NaN;
            double dv = 0.01;
            result = (Pricer(cpflg, S1, S2, S3 + dS, Q1, Q2, Q3, X, T, r, b1, b2, b3, v1, v2, v3 + dv, rho1, rho2, rho3)
                - Pricer(cpflg, S1, S2, S3 + dS, Q1, Q2, Q3, X, T, r, b1, b2, b3, v1, v2, v3 - dv, rho1, rho2, rho3)) / 2;
            return result;
        }
        public static double FdCorr1(string cpflg, double S1, double S2, double S3, double Q1, double Q2, double Q3, double X, double T,
        double r, double b1, double b2, double b3, double v1, double v2, double v3, double rho1, double rho2, double rho3, double dS)
        {
            double result = double.NaN;
            double dRho = 0.01;
            result = (Pricer(cpflg, S1, S2, S3 + dS, Q1, Q2, Q3, X, T, r, b1, b2, b3, v1, v2, v3, rho1 + dRho, rho2, rho3)
                - Pricer(cpflg, S1, S2, S3 + dS, Q1, Q2, Q3, X, T, r, b1, b2, b3, v1, v2, v3, rho1 - dRho, rho2, rho3)) / 2;
            return result;
        }

        public static double FdCorr2(string cpflg, double S1, double S2, double S3, double Q1, double Q2, double Q3, double X, double T,
        double r, double b1, double b2, double b3, double v1, double v2, double v3, double rho1, double rho2, double rho3, double dS)
        {
            double result = double.NaN;
            double dRho = 0.01;
            result = (Pricer(cpflg, S1, S2, S3 + dS, Q1, Q2, Q3, X, T, r, b1, b2, b3, v1, v2, v3, rho1, rho2 + dRho, rho3)
                - Pricer(cpflg, S1, S2, S3 + dS, Q1, Q2, Q3, X, T, r, b1, b2, b3, v1, v2, v3, rho1, rho2 - dRho, rho3)) / 2;
            return result;
        }


        public static double FdCorr3(string cpflg, double S1, double S2, double S3, double Q1, double Q2, double Q3, double X, double T,
        double r, double b1, double b2, double b3, double v1, double v2, double v3, double rho1, double rho2, double rho3, double dS)
        {
            double result = double.NaN;
            double dRho = 0.01;
            result = (Pricer(cpflg, S1, S2, S3 + dS, Q1, Q2, Q3, X, T, r, b1, b2, b3, v1, v2, v3, rho1, rho2, rho3 + dRho)
                - Pricer(cpflg, S1, S2, S3 + dS, Q1, Q2, Q3, X, T, r, b1, b2, b3, v1, v2, v3, rho1, rho2, rho3 - dRho)) / 2;
            return result;
        }
        public static double FdTheta(string cpflg, double S1, double S2, double S3, double Q1, double Q2, double Q3, double X, double T,
        double r, double b1, double b2, double b3, double v1, double v2, double v3, double rho1, double rho2, double rho3, double dS)
        {
            double result = double.NaN;
            if (T <= 1 / 252.0)
            {
                result = Pricer(cpflg, S1, S2, S3, Q1, Q2, Q3, X, 0.00001, r, b1, b2, b3, v1, v2, v3, rho1, rho2, rho3)
                    - Pricer(cpflg, S1, S2, S3, Q1, Q2, Q3, X, T, r, b1, b2, b3, v1, v2, v3, rho1, rho2, rho3);
            }
            else
            {
                result = Pricer(cpflg, S1, S2, S3, Q1, Q2, Q3, X, T - 1 / 252.0, r, b1, b2, b3, v1, v2, v3, rho1, rho2, rho3)
                    - Pricer(cpflg, S1, S2, S3, Q1, Q2, Q3, X, T, r, b1, b2, b3, v1, v2, v3, rho1, rho2, rho3);
            }
            return result;
        }

    }
}

