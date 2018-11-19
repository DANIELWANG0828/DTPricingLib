using MathNet.Numerics.Distributions;
using System;

namespace OptionPricingLib
{
    public class TwoAssetsSpreadApproxMethod
    {
        //Util Functions
        private static double Log(double X) { return Math.Log(X); }
        private static double CND(double X) { return Normal.CDF(0, 1, X); }
        private static double Exp(double X) { return Math.Exp(X); }
        private static double Sqr(double x) { return Math.Sqrt(x); }


        public static double TwoAssetsSpread(string cpflg, double S1, double S2, double Q1, double Q2, double X, double T,
                double r, double b1, double b2, double v1, double v2, double rho)
        {
            double v;
            double S;
            double d1, d2;
            double F;
            double price = double.NaN;
            F = Q2 * S2 * Exp((b2 - r) * T) / (Q2 * S2 * Exp((b2 - r) * T) + X * Exp(-r * T));
            v = Sqr(v1 * v1 + (v2 * F) * (v2 * F) - 2 * rho * v1 * v2 * F);
            S = Q1 * S1 * Exp((b1 - r) * T) / (Q2 * S2 * Exp((b2 - r) * T) + X * Exp(-r * T));
            d1 = (Log(S) + v * v / 2 * T) / (v * Sqr(T));
            d2 = d1 - v * Sqr(T);

            if (cpflg.Equals("c"))
            {
                price = (Q2 * S2 * Exp((b2 - r) * T) + X * Exp(-r * T)) * (S * CND(d1) - CND(d2));
            }
            else
            {
                price = (Q2 * S2 * Exp((b2 - r) * T) + X * Exp(-r * T)) * (CND(-d2) - S * CND(-d1));
            }
            return price;
        }

        public static double FdDelta1(string cpflg, double S1, double S2, double Q1, double Q2, double X, double T,
                double r, double b1, double b2, double v1, double v2, double rho, double dS)
        {
            double result = double.NaN;
            result = (TwoAssetsSpread(cpflg, S1 + dS, S2, Q1, Q2, X, T, r, b1, b2, v1, v2, rho) - TwoAssetsSpread(cpflg, S1 - dS, S2, Q1, Q2, X, T, r, b1, b2, v1, v2, rho)) / (2 * dS);
            return result;
        }

        public static double FdDelta2(string cpflg, double S1, double S2, double Q1, double Q2, double X, double T,
        double r, double b1, double b2, double v1, double v2, double rho, double dS)
        {
            double result = double.NaN;
            result = (TwoAssetsSpread(cpflg, S1, S2 + dS, Q1, Q2, X, T, r, b1, b2, v1, v2, rho) - TwoAssetsSpread(cpflg, S1, S2 - dS, Q1, Q2, X, T, r, b1, b2, v1, v2, rho)) / (2 * dS);
            return result;
        }

        ///ElseIf OutPutFlag = "e1" Then 'Elasticity S1
        ///    ESpreadApproximation = (SpreadApproximation(CallPutFlag, S1 + dS, S2, Q1, Q2, X, T, r, b1, b2, v1, v2, rho) - SpreadApproximation(CallPutFlag, S1 - dS, S2, Q1, Q2, X, T, r, b1, b2, v1, v2, rho)) / (2 * dS) * S1 / SpreadApproximation(CallPutFlag, S1, S2, Q1, Q2, X, T, r, b1, b2, v1, v2, rho)
        ///ElseIf OutPutFlag = "e2" Then 'Elasticity S2
        ///     ESpreadApproximation = (SpreadApproximation(CallPutFlag, S1, S2 + dS, Q1, Q2, X, T, r, b1, b2, v1, v2, rho) - SpreadApproximation(CallPutFlag, S1, S2 - dS, Q1, Q2, X, T, r, b1, b2, v1, v2, rho)) / (2 * dS) * S2 / SpreadApproximation(CallPutFlag, S1, S2, Q1, Q2, X, T, r, b1, b2, v1, v2, rho)

        public static double FdGammaP1(string cpflg, double S1, double S2, double Q1, double Q2, double X, double T,
                                        double r, double b1, double b2, double v1, double v2, double rho, double dS)
        {
            double result = double.NaN;
            result = S1 / 100 * (TwoAssetsSpread(cpflg, S1 + dS, S2, Q1, Q2, X, T, r, b1, b2, v1, v2, rho)
                - 2 * TwoAssetsSpread(cpflg, S1, S2, Q1, Q2, X, T, r, b1, b2, v1, v2, rho)
                + TwoAssetsSpread(cpflg, S1 - dS, S2, Q1, Q2, X, T, r, b1, b2, v1, v2, rho)) / (dS * dS);
            return result;
        }

        public static double FdGammaP2(string cpflg, double S1, double S2, double Q1, double Q2, double X, double T,
                                        double r, double b1, double b2, double v1, double v2, double rho, double dS)
        {
            double result = double.NaN;
            result = S2 / 100 * (TwoAssetsSpread(cpflg, S1, S2 + dS, Q1, Q2, X, T, r, b1, b2, v1, v2, rho)
                    - 2 * TwoAssetsSpread(cpflg, S1, S2, Q1, Q2, X, T, r, b1, b2, v1, v2, rho)
                    + TwoAssetsSpread(cpflg, S1, S2 - dS, Q1, Q2, X, T, r, b1, b2, v1, v2, rho)) / (dS * dS);
            return result;
        }

        public static double FdVega1(string cpflg, double S1, double S2, double Q1, double Q2, double X, double T,
                                      double r, double b1, double b2, double v1, double v2, double rho, double dS)
        {
            double result = double.NaN;
            double dv = 0.01;
            result = (TwoAssetsSpread(cpflg, S1, S2, Q1, Q2, X, T, r, b1, b2, v1 + dv, v2, rho)
                - TwoAssetsSpread(cpflg, S1, S2, Q1, Q2, X, T, r, b1, b2, v1 - dv, v2, rho)) / 2;
            return result;
        }

        public static double FdVega2(string cpflg, double S1, double S2, double Q1, double Q2, double X, double T,
                              double r, double b1, double b2, double v1, double v2, double rho, double dS)
        {
            double result = double.NaN;
            double dv = 0.01;
            result = (TwoAssetsSpread(cpflg, S1, S2, Q1, Q2, X, T, r, b1, b2, v2 + dv, v2, rho)
                - TwoAssetsSpread(cpflg, S1, S2, Q1, Q2, X, T, r, b1, b2, v2 - dv, v2, rho)) / 2;
            return result;
        }

        public static double FdCorr(string cpflg, double S1, double S2, double Q1, double Q2, double X, double T,
                      double r, double b1, double b2, double v1, double v2, double rho, double dS)
        {
            double result = double.NaN;
            double dRho = 0.01;
            result = (TwoAssetsSpread(cpflg, S1, S2, Q1, Q2, X, T, r, b1, b2, v1, v2, rho + dRho) - TwoAssetsSpread(cpflg, S1, S2, Q1, Q2, X, T, r, b1, b2, v1, v2, rho - dRho)) / 2;
            return result;
        }

        public static double FdTheta(string cpflg, double S1, double S2, double Q1, double Q2, double X, double T,
                      double r, double b1, double b2, double v1, double v2, double rho, double dS)
        {
            double result = double.NaN;
            if (T <= 1 / 252.0)
            {
                result = TwoAssetsSpread(cpflg, S1, S2, Q1, Q2, X, 0.00001, r, b1, b2, v1, v2, rho) - TwoAssetsSpread(cpflg, S1, S2, Q1, Q2, X, T, r, b1, b2, v1, v2, rho);
            }
            else
            {
                result = TwoAssetsSpread(cpflg, S1, S2, Q1, Q2, X, T - 1 / 252.0, r, b1, b2, v1, v2, rho) - TwoAssetsSpread(cpflg, S1, S2, Q1, Q2, X, T, r, b1, b2, v1, v2, rho);
            }
            return result;
        }
    }
}

