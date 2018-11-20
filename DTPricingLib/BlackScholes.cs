using ExcelDna.Integration;
using OPLib = OptionPricingLib;

namespace DTPricingLib
{
    public class BlackScholesMethod
    {   [ExcelFunction(Description = "Returns vanilla option price or greek solely through Black-Sholes-Merton method")]
        public static object dtec_blackscholes([ExcelArgument(Name = "OutPutFlag", Description = Flag.OutputFlag)] string OutPutFlag,
                                               [ExcelArgument(Name = "CallPutFlag", Description = Flag.VanillaStyle)] string CallPutFlag,
                                               [ExcelArgument(Name = "S", Description = "Spot price")] double S,
                                               [ExcelArgument(Name = "X", Description = "Strike price")] double X,
                                               [ExcelArgument(Name = "T", Description = "Days to expiration")] double T,
                                               [ExcelArgument(Name = "r", Description = "Interest rate")] double r,
                                               [ExcelArgument(Name = "b", Description = "Cost of carry")] double b,
                                               [ExcelArgument(Name = "v", Description = "Volatility")] double v,
                                               [ExcelArgument(Name = "dS", Description = "Delta S")] double ds)
        {
            double result = double.NaN;
            if (OutPutFlag.Equals("p"))
            {
                result = OPLib.BlackScholesMethod.BlackScholes(CallPutFlag, S, X, T, r, b, v);
            }

            else if (OutPutFlag.Equals("d"))
            {
                result = OPLib.BlackScholesMethod.FDA_Delta(CallPutFlag, S, X, T, r, b, v, ds);
            }

            else if (OutPutFlag.Equals("d+"))
            {
                result = OPLib.BlackScholesMethod.FDA_DeltaR(CallPutFlag, S, X, T, r, b, v, ds);
            }

            else if (OutPutFlag.Equals("d-"))
            {
                result = OPLib.BlackScholesMethod.FDA_DeltaL(CallPutFlag, S, X, T, r, b, v, ds);
            }


            else if (OutPutFlag.Equals("gp"))
            {
                result = OPLib.BlackScholesMethod.FDA_GammaP(CallPutFlag, S, X, T, r, b, v, ds);
            }

            else if (OutPutFlag.Equals("v"))
            {
                result = OPLib.BlackScholesMethod.FDA_Vega(CallPutFlag, S, X, T, r, b, v, ds);
            }

            else if (OutPutFlag.Equals("t"))
            {
                result = OPLib.BlackScholesMethod.FDA_Theta(CallPutFlag, S, X, T, r, b, v, ds);
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

        [ExcelFunction(Description = "This function returns an array of Black-Sholes-Merton option value and greeks")]
        public static object dtgc_blackscholes([ExcelArgument(Name = "CallPutFlag", Description = "call put style")] string CallPutFlag,
                                       [ExcelArgument(Name = "S", Description = "Spot price")] double S,
                                       [ExcelArgument(Name = "X", Description = "Strike price")] double X,
                                       [ExcelArgument(Name = "T", Description = "Days to expiration")] double T,
                                       [ExcelArgument(Name = "r", Description = "Interest rate")] double r,
                                       [ExcelArgument(Name = "b", Description = "Cost of carry")] double b,
                                       [ExcelArgument(Name = "v", Description = "Volatility")] double v,
                                       [ExcelArgument(Name = "dS", Description = "S-difference")] double ds)
        {
            double price = OPLib.BlackScholesMethod.BlackScholes(CallPutFlag, S, X, T, r, b, v);
            double delta = OPLib.BlackScholesMethod.FDA_Delta(CallPutFlag, S, X, T, r, b, v, ds);
            double deltaR = OPLib.BlackScholesMethod.FDA_DeltaR(CallPutFlag, S, X, T, r, b, v, ds);
            double deltaL = OPLib.BlackScholesMethod.FDA_DeltaL(CallPutFlag, S, X, T, r, b, v, ds);
            double gammap = OPLib.BlackScholesMethod.FDA_GammaP(CallPutFlag, S, X, T, r, b, v, ds);
            double vega = OPLib.BlackScholesMethod.FDA_Vega(CallPutFlag, S, X, T, r, b, v, ds);
            double theta = OPLib.BlackScholesMethod.FDA_Theta(CallPutFlag, S, X, T, r, b, v, ds);
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
