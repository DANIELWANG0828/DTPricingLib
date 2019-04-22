using Accord.Statistics.Distributions.Multivariate;
using MathNet.Numerics.Distributions;
using System;
namespace OptionPricingLib
{

    public class TwoAssetsCashOrNothingMethod
    {//Util Functions
        private static double Log(double X) { return Math.Log(X); }
        private static double CND(double X) { return Normal.CDF(0, 1, X); }
        private static double Exp(double X) { return Math.Exp(X); }
        private static double Sqr(double x) { return Math.Sqrt(x); }
        private static double M(double x, double y, double rho)
        { return MultivariateNormalDistribution.Bivariate(0, 0, 1, 1, rho).DistributionFunction(new double[] { x, y }); }
        public static double TwoAssetsCashOrNothingOption(string tpflag, double S1, double S2, double X,
            double T, double r, double b1, double b2, double vol1, double vol2, double rho)
        {

            double price = double.NaN;
            double vol = Sqr(vol1 * vol1 + vol2 * vol2 - 2 * rho * vol1 * vol2);
            double y = (Log(S1 / S2) + (b1 -b2 +vol * vol / 2) * T) / vol / Sqr(T);
            double z1 = (Log(S1 / X) + (b1 + vol1 * vol1 / 2) * T) / vol1 / Sqr(T);
            double z2 = (Log(S2 / X) + (b2 + vol2 * vol2 / 2) * T) / vol2 / Sqr(T);
            double rho1 = (vol1 - rho * vol2) / vol;
            double rho2 = (vol2 - rho * vol1) / vol;
            
            if (tpflag == "wc")
            {
                price = X * Exp(-r * T) * (M(-y, z1, rho1) + M(y, z2, rho2));
            }
            else if (tpflag == "bc")
            {
                price = X * Exp(-r * T) * (M(y, z1, -rho1) + M(-y, z2, -rho2));
            }
            else if (tpflag == "wp")
            {
                price = X * Exp(-r * T) * (1 - M(-y, z1, rho1) - M(y, z2, rho2));



            }
            else if (tpflag == "bp")
            {
                price = X * Exp(-r * T) * (1 - M(y, z1, -rho1) - M(-y, z2, -rho2));
            }
            
            return price;

        }
    }
    public class BestOfOrWorstOfTwoMethod
    {
        //Util Functions
        private static double Log(double X) { return Math.Log(X); }
        private static double CND(double X) { return Normal.CDF(0, 1, X); }
        private static double Exp(double X) { return Math.Exp(X); }
        private static double Sqr(double x) { return Math.Sqrt(x); }
        private static double M(double x, double y, double rho)
        { return MultivariateNormalDistribution.Bivariate(0, 0, 1, 1, rho).DistributionFunction(new double[] { x, y }); }

        public static double BestOfOrWorstOfTwoOption(string tpflag, double S1, double S2, double X,
            double T, double r, double b1, double b2, double vol1, double vol2,double rho)
        {
            
            double price = double.NaN;
            double vol = Sqr(vol1 * vol1 + vol2 * vol2 - 2 * rho * vol1 * vol2);
            double y1 = (Log(S1 / X) + (b1 + vol1 * vol1 / 2) * T) / vol1 / Sqr(T);
            double y2 = (Log(S2 / X) + (b2 + vol2 * vol2 / 2) * T) / vol2 / Sqr(T);
            double d = (Log(S1 / S2) + (b1 - b2 + vol * vol / 2) * T) / vol / Sqr(T);
            double rho1 = (vol1 - rho * vol2) / vol;
            double rho2 = (vol2 - rho * vol1) / vol;
            if (tpflag == "wc")
            {
                price = S1 * Exp((b1 - r) * T) * M(y1, -d, -rho1) + S2 * Exp((b2 - r) * T) * M(y2, d - vol * Sqr(T), -rho2)
                    - X * Exp(-r * T) * M(y1 - vol1 * Sqr(T), y2 - vol2 * Sqr(T), rho);
            }
            else if (tpflag == "bc")
            {
                price = S1 * Exp((b1 - r) * T) * M(y1, d, rho1) + S2 * Exp((b2 - r) * T) * M(y2, -d + vol * Sqr(T), rho2)
                    - X * Exp(-r * T) *(1 - M(-y1 + vol1 * Sqr(T),- y2 + vol2 * Sqr(T), rho));
            }
            else if (tpflag == "wp")
            {
                price = X * Exp(-r * T) 
                    -(S1 * Exp((b1 - r) * T) * (1 - CND(d)) + S2 * Exp((b2 - r) * T) * CND(d - vol * Sqr(T)))
                    + BestOfOrWorstOfTwoOption("wc", S1, S2, X, T, r, b1, b2, vol1, vol2, rho);

            }
            else if (tpflag == "bp")
            {
                price = X * Exp(-r * T)
                    - (S2 * Exp((b2 - r) * T) * (1 - CND(d - vol * Sqr(T))) + S1 * Exp((b1 - r) * T) * CND(d))
                    + BestOfOrWorstOfTwoOption("bc", S1, S2, X, T, r, b1, b2, vol1, vol2, rho);
            }
            return price;

        }

        public static double FdDelta1(string tpflag, double S1, double S2, double X, double T,
               double r, double b1, double b2, double v1, double v2, double rho, double dS)
        {
            double result = double.NaN;
            result = (BestOfOrWorstOfTwoOption(tpflag, S1 + dS, S2, X, T, r, b1, b2, v1, v2, rho) - BestOfOrWorstOfTwoOption(tpflag, S1 - dS, S2, X, T, r, b1, b2, v1, v2, rho)) / (2 * dS);
            return result;
        }

        public static double FdDelta2(string tpflag, double S1, double S2, double X, double T,
        double r, double b1, double b2, double v1, double v2, double rho, double dS)
        {
            double result = double.NaN;
            result = (BestOfOrWorstOfTwoOption(tpflag, S1, S2 + dS, X, T, r, b1, b2, v1, v2, rho) - BestOfOrWorstOfTwoOption(tpflag, S1, S2 - dS, X, T, r, b1, b2, v1, v2, rho)) / (2 * dS);
            return result;
        }

        ///ElseIf OutPutFlag = "e1" Then 'Elasticity S1
        ///    ESpreadApproximation = (SpreadApproximation(CallPutFlag, S1 + dS, S2, X, T, r, b1, b2, v1, v2, rho) - SpreadApproximation(CallPutFlag, S1 - dS, S2, X, T, r, b1, b2, v1, v2, rho)) / (2 * dS) * S1 / SpreadApproximation(CallPutFlag, S1, S2, X, T, r, b1, b2, v1, v2, rho)
        ///ElseIf OutPutFlag = "e2" Then 'Elasticity S2
        ///     ESpreadApproximation = (SpreadApproximation(CallPutFlag, S1, S2 + dS, X, T, r, b1, b2, v1, v2, rho) - SpreadApproximation(CallPutFlag, S1, S2 - dS, X, T, r, b1, b2, v1, v2, rho)) / (2 * dS) * S2 / SpreadApproximation(CallPutFlag, S1, S2, X, T, r, b1, b2, v1, v2, rho)

        public static double FdGammaP1(string tpflag, double S1, double S2, double X, double T,
                                        double r, double b1, double b2, double v1, double v2, double rho, double dS)
        {
            double result = double.NaN;
            result = S1 / 100 * (BestOfOrWorstOfTwoOption(tpflag, S1 + dS, S2, X, T, r, b1, b2, v1, v2, rho)
                - 2 * BestOfOrWorstOfTwoOption(tpflag, S1, S2, X, T, r, b1, b2, v1, v2, rho)
                + BestOfOrWorstOfTwoOption(tpflag, S1 - dS, S2, X, T, r, b1, b2, v1, v2, rho)) / (dS * dS);
            return result;
        }

        public static double FdGammaP2(string tpflag, double S1, double S2, double X, double T,
                                        double r, double b1, double b2, double v1, double v2, double rho, double dS)
        {
            double result = double.NaN;
            result = S2 / 100 * (BestOfOrWorstOfTwoOption(tpflag, S1, S2 + dS, X, T, r, b1, b2, v1, v2, rho)
                    - 2 * BestOfOrWorstOfTwoOption(tpflag, S1, S2, X, T, r, b1, b2, v1, v2, rho)
                    + BestOfOrWorstOfTwoOption(tpflag, S1, S2 - dS, X, T, r, b1, b2, v1, v2, rho)) / (dS * dS);
            return result;
        }

        public static double FdVega1(string tpflag, double S1, double S2, double X, double T,
                                      double r, double b1, double b2, double v1, double v2, double rho, double dS)
        {
            double result = double.NaN;
            double dv = 0.01;
            result = (BestOfOrWorstOfTwoOption(tpflag, S1, S2, X, T, r, b1, b2, v1 + dv, v2, rho)
                - BestOfOrWorstOfTwoOption(tpflag, S1, S2, X, T, r, b1, b2, v1 - dv, v2, rho)) / 2;
            return result;
        }

        public static double FdVega2(string tpflag, double S1, double S2, double X, double T,
                              double r, double b1, double b2, double v1, double v2, double rho, double dS)
        {
            double result = double.NaN;
            double dv = 0.01;
            result = (BestOfOrWorstOfTwoOption(tpflag, S1, S2, X, T, r, b1, b2, v2 + dv, v2, rho)
                - BestOfOrWorstOfTwoOption(tpflag, S1, S2, X, T, r, b1, b2, v2 - dv, v2, rho)) / 2;
            return result;
        }

        public static double FdCorr(string tpflag, double S1, double S2, double X, double T,
                      double r, double b1, double b2, double v1, double v2, double rho, double dS)
        {
            double result = double.NaN;
            double dRho = 0.01;
            result = (BestOfOrWorstOfTwoOption(tpflag, S1, S2, X, T, r, b1, b2, v1, v2, rho + dRho) - BestOfOrWorstOfTwoOption(tpflag, S1, S2, X, T, r, b1, b2, v1, v2, rho - dRho)) / 2;
            return result;
        }

        public static double FdTheta(string tpflag, double S1, double S2, double X, double T,
                      double r, double b1, double b2, double v1, double v2, double rho, double dS)
        {
            double result = double.NaN;
            if (T <= 1 / 252.0)
            {
                result = BestOfOrWorstOfTwoOption(tpflag, S1, S2, X, 0.00001, r, b1, b2, v1, v2, rho) - BestOfOrWorstOfTwoOption(tpflag, S1, S2, X, T, r, b1, b2, v1, v2, rho);
            }
            else
            {
                result = BestOfOrWorstOfTwoOption(tpflag, S1, S2, X, T - 1 / 252.0, r, b1, b2, v1, v2, rho) - BestOfOrWorstOfTwoOption(tpflag, S1, S2, X, T, r, b1, b2, v1, v2, rho);
            }
            return result;
        }

    }


    public class RelativeOutperformanceMethod
    {//Util Functions
        private static double Log(double X) { return Math.Log(X); }
        private static double CND(double X) { return Normal.CDF(0, 1, X); }
        private static double Exp(double X) { return Math.Exp(X); }
        private static double Sqr(double x) { return Math.Sqrt(x); }
        private static double M(double x, double y, double rho)
        { return MultivariateNormalDistribution.Bivariate(0, 0, 1, 1, rho).DistributionFunction(new double[] { x, y }); }

    }

    public class ProductMethod
    {//Util Functions
        private static double Log(double X) { return Math.Log(X); }
        private static double CND(double X) { return Normal.CDF(0, 1, X); }
        private static double Exp(double X) { return Math.Exp(X); }
        private static double Sqr(double x) { return Math.Sqrt(x); }
        private static double M(double x, double y, double rho)
        { return MultivariateNormalDistribution.Bivariate(0, 0, 1, 1, rho).DistributionFunction(new double[] { x, y }); }

    }

    public class TwoAssetCorrelationMethod
    {//Util Functions
        private static double Log(double X) { return Math.Log(X); }
        private static double CND(double X) { return Normal.CDF(0, 1, X); }
        private static double Exp(double X) { return Math.Exp(X); }
        private static double Sqr(double x) { return Math.Sqrt(x); }
        private static double M(double x, double y, double rho)
        { return MultivariateNormalDistribution.Bivariate(0, 0, 1, 1, rho).DistributionFunction(new double[] { x, y }); }

    }

    

    public class AmericanExchangeOneAssetForAnotherMethod
    {//Util Functions
        private static double Log(double X) { return Math.Log(X); }
        private static double CND(double X) { return Normal.CDF(0, 1, X); }
        private static double Exp(double X) { return Math.Exp(X); }
        private static double Sqr(double x) { return Math.Sqrt(x); }
        private static double M(double x, double y, double rho)
        { return MultivariateNormalDistribution.Bivariate(0, 0, 1, 1, rho).DistributionFunction(new double[] { x, y }); }

    }

    public class TwoAssetsBarrierAssetsMethod
    {//Util Functions
        private static double Log(double X) { return Math.Log(X); }
        private static double CND(double X) { return Normal.CDF(0, 1, X); }
        private static double Exp(double X) { return Math.Exp(X); }
        private static double Sqr(double x) { return Math.Sqrt(x); }
        private static double M(double x, double y, double rho)
        { return MultivariateNormalDistribution.Bivariate(0, 0, 1, 1, rho).DistributionFunction(new double[] { x, y }); }

    }
    public class PartialTimeTwoAssetsBarrierAssetsMethod
    {//Util Functions
        private static double Log(double X) { return Math.Log(X); }
        private static double CND(double X) { return Normal.CDF(0, 1, X); }
        private static double Exp(double X) { return Math.Exp(X); }
        private static double Sqr(double x) { return Math.Sqrt(x); }
        private static double M(double x, double y, double rho)
        { return MultivariateNormalDistribution.Bivariate(0, 0, 1, 1, rho).DistributionFunction(new double[] { x, y }); }

    }
    public class MargrabeBarrierMethod
    {//Util Functions
        private static double Log(double X) { return Math.Log(X); }
        private static double CND(double X) { return Normal.CDF(0, 1, X); }
        private static double Exp(double X) { return Math.Exp(X); }
        private static double Sqr(double x) { return Math.Sqrt(x); }
        private static double M(double x, double y, double rho)
        { return MultivariateNormalDistribution.Bivariate(0, 0, 1, 1, rho).DistributionFunction(new double[] { x, y }); }

    }
    public class DiscreteBarrierMethod
    {//Util Functions
        private static double Log(double X) { return Math.Log(X); }
        private static double CND(double X) { return Normal.CDF(0, 1, X); }
        private static double Exp(double X) { return Math.Exp(X); }
        private static double Sqr(double x) { return Math.Sqrt(x); }
        private static double M(double x, double y, double rho)
        { return MultivariateNormalDistribution.Bivariate(0, 0, 1, 1, rho).DistributionFunction(new double[] { x, y }); }

    }
    


    public class BestOrWorstCashOrNothingMethod
    {//Util Functions
        private static double Log(double X) { return Math.Log(X); }
        private static double CND(double X) { return Normal.CDF(0, 1, X); }
        private static double Exp(double X) { return Math.Exp(X); }
        private static double Sqr(double x) { return Math.Sqrt(x); }
        private static double M(double x, double y, double rho)
        { return MultivariateNormalDistribution.Bivariate(0, 0, 1, 1, rho).DistributionFunction(new double[] { x, y }); }


    }

    public class OptionsOnMaximumOrMinimumOfTwoAveragesMethod
    {//Util Functions
        private static double Log(double X) { return Math.Log(X); }
        private static double CND(double X) { return Normal.CDF(0, 1, X); }
        private static double Exp(double X) { return Math.Exp(X); }
        private static double Sqr(double x) { return Math.Sqrt(x); }
        private static double M(double x, double y, double rho)
        { return MultivariateNormalDistribution.Bivariate(0, 0, 1, 1, rho).DistributionFunction(new double[] { x, y }); }



    }

    public class FixedFXForeignAssetStruckinDomesticCurrencyMethod
    {
        //Util Functions
        private static double Log(double X) { return Math.Log(X); }
        private static double CND(double X) { return Normal.CDF(0, 1, X); }
        private static double Exp(double X) { return Math.Exp(X); }
        private static double Sqr(double x) { return Math.Sqrt(x); }
        private static double M(double x, double y, double rho)
        { return MultivariateNormalDistribution.Bivariate(0, 0, 1, 1, rho).DistributionFunction(new double[] { x, y }); }
        double Quanto(string cpflg, double Ep, double S, double X, double T, double rd, double rf, double b, double vS,
           double vE, double rho)
        {
            //Ep predetermined exchange rate specified in units of foreign currency per unit of domestic currency
            //S  asset price in foreign currency
            //X  strike price in foreign currency
            //rd  riskfree rate of domestics currency
            //rf  riskfree rate of foreign currency
            //rho correlation between asset and domestics FX rate
            double d1 = (Log(S / X) + (b - rho * vS * vE + vS * vS / 2) * T) / (vS * Sqr(T));
            double d2 = d1 - vS * Sqr(T);
            double price = double.NaN;
            if (cpflg == "c")
            {
                price = Ep * (Exp((b - rd - rho * vS * vE) * T) * S * CND(d1) - X * Exp(-rd * T) * CND(d2));
            }
            else if (cpflg == "p")
            {
                price = Ep * (-Exp((b - rd - rho * vS * vE) * T) * S * CND(-d1) + X * Exp(-rd * T) * CND(-d2));
            }
            return price;
        }
       

    }

    public class ForeignAssetStruckinDomesticCurrencyMethod
    {//Util Functions
        private static double Log(double X) { return Math.Log(X); }
        private static double CND(double X) { return Normal.CDF(0, 1, X); }
        private static double Exp(double X) { return Math.Exp(X); }
        private static double Sqr(double x) { return Math.Sqrt(x); }
        private static double M(double x, double y, double rho)
        { return MultivariateNormalDistribution.Bivariate(0, 0, 1, 1, rho).DistributionFunction(new double[] { x, y }); }
        double FixedExForeignAssetStruckinDomesticCurrencyOptionValueInForeignCurrency(string cpflg, 
            double S_star, double E_star, double X, double T, double r, double b, double volSstar, 
            double volEstar,double rhoEstarSstar)
        {
            double volEstarSstar = Sqr(volSstar * volSstar + volEstar * volEstar + 2 * rhoEstarSstar * volSstar * volSstar);
            double d1 = (Log(S_star / E_star / X) + (b + volEstarSstar * volEstarSstar / 2) * T) / (volEstarSstar * Sqr(T));
            double d2 = d1 - volEstarSstar * Sqr(T);
            double price = double.NaN;
            if (cpflg == "c")
            {
                price = Exp((b - r) * T) * S_star * CND(d1) - E_star * X * Exp(-r * T) * CND(d2);
            }
            else if (cpflg == "p")
            {
                price = -Exp((b - r) * T) * S_star * CND(-d1) + E_star * X * Exp(-r * T) * CND(-d2);
            }
            return price;
        }

        double FixedExForeignAssetStruckinDomesticCurrencyOptionValueInDomesticCurrency(string cpflg, 
            double S_star, double E, double X, double T, double r, double b, double volSstar, 
            double volE, double rhoESstar)
        {
            double volEstarSstar = Sqr(volSstar * volSstar + volE * volE + 2 * rhoESstar * volSstar * volSstar);
            double d1 = (Log(S_star / E / X) + (b + volEstarSstar * volEstarSstar / 2) * T) / (volEstarSstar * Sqr(T));
            double d2 = d1 - volEstarSstar * Sqr(T);
            double price = double.NaN;
            if (cpflg == "c")
            {
                price = E * Exp((b - r) * T) * S_star * CND(d1) - X * Exp(-r * T) * CND(d2);
            }
            else if (cpflg == "p")
            {
                price = -E * Exp((b - r) * T) * S_star * CND(-d1) +  X * Exp(-r * T) * CND(-d2);
            }
            return price;
        }

    }


}
