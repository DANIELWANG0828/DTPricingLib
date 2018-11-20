using ExcelDna.Integration;
using OPLib = OptionPricingLib;

namespace DTPricingLib
{
    public class ForwardStart
    {
        [ExcelFunction(Description = "Returns forward_start_option price and greeks through  integration")]
        public static object dtei_forwardstart([ExcelArgument(Name = "OutPutFlag", Description = "OutPutFlag")] string OutPutFlag,
            [ExcelArgument(Name = "call or put", Description = "cpflg")] string cpflg,
            [ExcelArgument(Name = "spot price", Description = "S0")] double S0,
            [ExcelArgument(Name = "time to start", Description = "t1")] double t1,
            [ExcelArgument(Name = "time to option maturity", Description = "Days to expiration")] double t2,
            [ExcelArgument(Name = "r", Description = "r")] double r,
            [ExcelArgument(Name = "b", Description = "b")] double b,
            [ExcelArgument(Name = "vol", Description = "vol")] double vol,
            [ExcelArgument(Name = "shift in strike price", Description = "shift in strike price")] double a)
        {

            double return_value = double.NaN;

            if (OutPutFlag == "p")
            {

                return_value = OPLib.ForwardStartOption.ForwardStart(cpflg, S0, t1, t2, r, b, vol, a);


            }

            else if (OutPutFlag == "d")
            {

                return_value = OPLib.ForwardStartOption.Delta(cpflg, S0, t1, t2, r, b, vol, a);


            }

            else if (OutPutFlag == "v")
            {

                return_value = OPLib.ForwardStartOption.Vega(cpflg, S0, t1, t2, r, b, vol, a);


            }

            if (double.IsNaN(return_value))
            {
                return ExcelError.ExcelErrorValue;
            }
            else
            {
                return return_value;
            }


        }
    }
}