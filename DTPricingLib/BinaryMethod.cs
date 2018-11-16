using ExcelDna.Integration;
using OPLib = OptionPricingLib;

namespace DTPricingLib
{
    class BinaryMethod
    {
        [ExcelFunction(Description = "Returns vanilla binary option price and greeks through Black-Sholes-Merton method")]
        public static object dtec_cashornothing([ExcelArgument(Name = "OutPutFlag", Description = Flag.OutputFlag)] string OutPutFlag,
                                               [ExcelArgument(Name = "CallPutFlag", Description = Flag.VanillaStyle)] string CallPutFlag,
                                               [ExcelArgument(Name = "S", Description = "Spot price")] double S,
                                               [ExcelArgument(Name = "x", Description = "Strike price")] double x,
                                               [ExcelArgument(Name = "k", Description = "Rebate cash")] double k,
                                               [ExcelArgument(Name = "T", Description = "Days to expiration")] double T,
                                               [ExcelArgument(Name = "r", Description = "Interest rate")] double r,
                                               [ExcelArgument(Name = "b", Description = "Cost of carry")] double b,
                                               [ExcelArgument(Name = "v", Description = "Volatility")] double v,
                                               [ExcelArgument(Name = "dS", Description = "Delta S")] double ds)
        {
            double result = double.NaN;
            if (OutPutFlag.Equals("price"))
            {
                result = OPLib.BinaryMethod.CashOrNothing(CallPutFlag, S, x, k, T, r, b, v);
            }

            else if (OutPutFlag.Equals("delta"))
            {
                result = OPLib.BinaryMethod.FDA_Delta(CallPutFlag, S, x, k, T, r, b, v, ds);
            }

            else if (OutPutFlag.Equals("delta+"))
            {
                result = OPLib.BinaryMethod.FDA_DeltaR(CallPutFlag, S, x, k, T, r, b, v, ds);
            }

            else if (OutPutFlag.Equals("delta-"))
            {
                result = OPLib.BinaryMethod.FDA_DeltaL(CallPutFlag, S, x, k, T, r, b, v, ds);
            }


            else if (OutPutFlag.Equals("gammap"))
            {
                result = OPLib.BinaryMethod.FDA_GammaP(CallPutFlag, S, x, k, T, r, b, v, ds);
            }

            else if (OutPutFlag.Equals("vega"))
            {
                result = OPLib.BinaryMethod.FDA_Vega(CallPutFlag, S, x, k, T, r, b, v, ds);
            }

            else if (OutPutFlag.Equals("theta"))
            {
                result = OPLib.BinaryMethod.FDA_Theta(CallPutFlag, S, x, k, T, r, b, v, ds);
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
    }
}
