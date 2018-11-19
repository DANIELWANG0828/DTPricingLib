using ExcelDna.Integration;
using OPLib = OptionPricingLib;

namespace DTPricingLib
{
    public class AsianMethod
    {
        [ExcelFunction(Description = "This function returns price or greek solely of  Discreted Observed Asian option")]
        public static object dtec_discreteasianhhm([ExcelArgument(Name = "OutPutFlag", Description = Flag.OutputFlag)] string OutPutFlag,
                                               [ExcelArgument(Name = "CallPutFlag", Description = Flag.VanillaStyle)] string CallPutFlag,
                                               [ExcelArgument(Name = "S", Description = "Spot price")] double S,
                                               [ExcelArgument(Name = "SA", Description = "Realized Average Price")] double SA,
                                               [ExcelArgument(Name = "X", Description = "Strike price")] double X,
                                               [ExcelArgument(Name = "t1", Description = "****************")] double t1,
                                               [ExcelArgument(Name = "T", Description = "Days to expiration")] double T,
                                               [ExcelArgument(Name = "n", Description = "****************")] double n,
                                               [ExcelArgument(Name = "m", Description = "****************")] double m,
                                               [ExcelArgument(Name = "r", Description = "Interest rate")] double r,
                                               [ExcelArgument(Name = "b", Description = "Cost of carry")] double b,
                                               [ExcelArgument(Name = "v", Description = "Volatility")] double v,
                                               [ExcelArgument(Name = "dS", Description = "Delta S")] double ds)
        {
            double result = double.NaN;
            if (OutPutFlag.Equals("price"))
            {
                result = OPLib.AsianMethod.DiscreteAsianHHM(CallPutFlag, S, SA, X, t1, T, n, m, r, b, v);
            }

            else if (OutPutFlag.Equals("delta"))
            {
                result = OPLib.AsianMethod.FDA_Delta(CallPutFlag, S, SA, X, t1, T, n, m, r, b, v,ds);
            }

            else if (OutPutFlag.Equals("delta+"))
            {
                result = OPLib.AsianMethod.FDA_DeltaR(CallPutFlag, S, SA, X, t1, T, n, m, r, b, v,ds);
            }

            else if (OutPutFlag.Equals("delta-"))
            {
                result = OPLib.AsianMethod.FDA_DeltaL(CallPutFlag, S, SA, X, t1, T, n, m, r, b, v,ds);
            }


            else if (OutPutFlag.Equals("gammap"))
            {
                result = OPLib.AsianMethod.FDA_GammaP(CallPutFlag, S, SA, X, t1, T, n, m, r, b, v,ds);
            }

            else if (OutPutFlag.Equals("vega"))
            {
                result = OPLib.AsianMethod.FDA_Vega(CallPutFlag, S, SA, X, t1, T, n, m, r, b, v,ds);
            }

            else if (OutPutFlag.Equals("theta"))
            {
                result = OPLib.AsianMethod.FDA_Theta(CallPutFlag, S, SA, X, t1, T, n, m, r, b, v,ds);
            }

            else
            {
                result = double.NaN;
            }

            if (double.IsNaN(result))
            {
                return ExcelError.ExcelErrorValue;
            }
            else
            {
                return result;
            }
        }

        [ExcelFunction(Description = "This function returns an array of Discreted Observed Asian option value and greeks")]
        public static object dtgc_discreteasianhhm([ExcelArgument(Name = "OutPutFlag", Description = Flag.OutputFlag)] string OutPutFlag,
                                               [ExcelArgument(Name = "CallPutFlag", Description = Flag.VanillaStyle)] string CallPutFlag,
                                               [ExcelArgument(Name = "S", Description = "Spot price")] double S,
                                               [ExcelArgument(Name = "SA", Description = "Realized Average Price")] double SA,
                                               [ExcelArgument(Name = "X", Description = "Strike price")] double X,
                                               [ExcelArgument(Name = "t1", Description = "****************")] double t1,
                                               [ExcelArgument(Name = "T", Description = "Days to expiration")] double T,
                                               [ExcelArgument(Name = "n", Description = "****************")] double n,
                                               [ExcelArgument(Name = "m", Description = "****************")] double m,
                                               [ExcelArgument(Name = "r", Description = "Interest rate")] double r,
                                               [ExcelArgument(Name = "b", Description = "Cost of carry")] double b,
                                               [ExcelArgument(Name = "v", Description = "Volatility")] double v,
                                               [ExcelArgument(Name = "dS", Description = "Delta S")] double ds)
        {
            double price = OPLib.AsianMethod.DiscreteAsianHHM(CallPutFlag, S, SA, X, t1, T, n, m, r, b, v);
            double delta = OPLib.AsianMethod.FDA_Delta(CallPutFlag, S, SA, X, t1, T, n, m, r, b, v,ds);
            double deltaR = OPLib.AsianMethod.FDA_DeltaR(CallPutFlag, S, SA, X, t1, T, n, m, r, b, v,ds);
            double deltaL = OPLib.AsianMethod.FDA_DeltaL(CallPutFlag, S, SA, X, t1, T, n, m, r, b, v,ds);
            double gammap = OPLib.AsianMethod.FDA_GammaP(CallPutFlag, S, SA, X, t1, T, n, m, r, b, v,ds);
            double vega = OPLib.AsianMethod.FDA_Vega(CallPutFlag, S, SA, X, t1, T, n, m, r, b, v,ds);
            double theta = OPLib.AsianMethod.FDA_Theta(CallPutFlag, S, SA, X, t1, T, n, m, r, b, v,ds);
            double[] _result = { price, delta, deltaR, deltaL, gammap, vega, theta };
            object[] result = new object[7];


            for (int i = 0; i < 7; i++)
            {
                if (double.IsNaN(_result[i]))
                {
                    result[i] = ExcelError.ExcelErrorValue;
                }
                else
                {
                    result[i] = _result[i];
                }
            }
            return result;
        }
    }
}
