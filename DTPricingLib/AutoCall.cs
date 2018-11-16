using ExcelDna.Integration;
using System;
using System.Threading;
using OPLib = OptionPricingLib;

namespace DTPricingLib
{
    public class AutoCalls
    { 
        [ExcelFunction(IsVolatile = false,Description = "Returns autocall option price and greeks through Monte Carlo method")]
        public static object dtgm_autocall([ExcelArgument(Description = "S0")] double S0,
            [ExcelArgument(Description  = "Interest Rate")] double r,
            [ExcelArgument(Description = "Carry")] double b,
            [ExcelArgument(Description = "Volatility")] double vol,
            [ExcelArgument(Description = "fixing observation knock out days")] double[] fixings,
            [ExcelArgument(Description = "knock out price")] double ko_price,
            [ExcelArgument(Description = "knock in price")] object ki_price,
            [ExcelArgument(Description = "Strike")] double K,
            [ExcelArgument(Description = "Coupon Rate")] double coupon,
            [ExcelArgument(Description = "Rebate Rate")] double rebate,
            [ExcelArgument(Description = "Nominal Price")] double nominal,
            [ExcelArgument(Description = "Funding Rate")] double funding,
            [ExcelArgument(Description = "If pay is annualized")] double annpay,
            [ExcelArgument(Description = "N Simulations")] int nsims)
        {
            double _ki_price;
            if (ki_price is ExcelEmpty) { _ki_price = -1; }
            else { _ki_price = Convert.ToDouble(ki_price); }

            double[] _result = OPLib.AutoCall.AutoCallable(S0, r, b, vol, fixings, ko_price, _ki_price, K, coupon, rebate, nominal, funding, annpay, nsims);
            object[] result = new object[5];
            for (int i = 0; i < 5; i++)
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


        [ExcelFunction(IsVolatile = false, Description = "Returns autocall option price or greek solely through Monte Carlo method")]
        public static object dtem_autocall([ExcelArgument(Name = "OutPutFlag", Description = Flag.OutputFlag)] string outputflag,
            [ExcelArgument(Description = "S0")] double S0,
            [ExcelArgument(Description = "Interest Rate")] double r,
            [ExcelArgument(Description = "Carry")] double b,
            [ExcelArgument(Description = "Volatility")] double vol,
            [ExcelArgument(Description = "fixing observation knock out days")] double[] fixings,
            [ExcelArgument(Description = "knock out price")] double ko_price,
            [ExcelArgument(Description = "knock in price")] object ki_price,
            [ExcelArgument(Description = "Strike")] double K,
            [ExcelArgument(Description = "Coupon Rate")] double coupon,
            [ExcelArgument(Description = "Rebate Rate")] double rebate,
            [ExcelArgument(Description = "Nominal Price")] double nominal,
            [ExcelArgument(Description = "Funding Rate")] double funding,
            [ExcelArgument(Description = "If pay is annualized")] double annpay,
            [ExcelArgument(Description = "N Simulations")] int nsims)
        {
            double _ki_price;
            if (ki_price is ExcelEmpty) { _ki_price = -1; }
            else { _ki_price = Convert.ToDouble(ki_price); }
       

            double[] _result = OPLib.AutoCall.AutoCallable(S0, r, b, vol, fixings, ko_price, _ki_price, K, coupon, rebate, nominal, funding, annpay, nsims);
            object[] result = new object[5];
            for (int i = 0; i < 5; i++)
            {
                if (double.IsNaN(_result[i])) { result[i] = ExcelError.ExcelErrorValue; }
                else {result[i] = _result[i]; }
            }
            if (outputflag == "price") { return result[0]; }
            else if (outputflag == "delta") { return result[1]; }
            else if (outputflag == "gamma") { return result[2]; }
            else if (outputflag == "vega") { return result[3]; }
            else if (outputflag == "theta") { return result[4]; }
            else { return ExcelError.ExcelErrorValue; }
        }








        [ExcelFunction(IsVolatile = false, Description = "Returns vanilla option price and greeks through Black-Sholes-Merton method")]
        public static object dtgm_autocallAsync([ExcelArgument(Description = "S0")] double S0,
            [ExcelArgument(Description = "Interest Rate")] double r,
            [ExcelArgument(Description = "Carry")] double b,
            [ExcelArgument(Description = "Volatility")] double vol,
            [ExcelArgument(Description = "fixing observation knock out days")] double[] fixings,
            [ExcelArgument(Description = "knock out price")] double ko_price,
            [ExcelArgument(Description = "knock in price")] object ki_price,
            [ExcelArgument(Description = "Strike")] double K,
            [ExcelArgument(Description = "Coupon Rate")] double coupon,
            [ExcelArgument(Description = "Rebate Rate")] double rebate,
            [ExcelArgument(Description = "Nominal Price")] double nominal,
            [ExcelArgument(Description = "Funding Rate")] double funding,
            [ExcelArgument(Description = "If pay is annualized")] double annpay,
            [ExcelArgument(Description = "N Simulations")] int nsims)
        {
            double _ki_price;
            if (ki_price is ExcelEmpty) { _ki_price = -1; }
            else { _ki_price = Convert.ToDouble(ki_price); }

            object result_value = ExcelAsyncUtil.Run("dtgm_autocallAsync", new object[] { S0, r, b, vol, fixings, ko_price, _ki_price, K, coupon, rebate, nominal, funding, annpay, nsims, 0 },
            () => OPLib.AutoCall.AutoCallable(S0, r, b, vol, fixings, ko_price, _ki_price, K, coupon, rebate, nominal, funding, annpay, nsims));
            if (Equals(result_value, ExcelError.ExcelErrorNA))
            {
                return "Calculating...";
            }
           
            return result_value;
        }


   

    }
}
