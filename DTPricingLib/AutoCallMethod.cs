using ExcelDna.Integration;
using OPLib = OptionPricingLib;

namespace DTPricingLib
{
    public class AutoCallMethod
    { 
        [ExcelFunction(IsVolatile = false,Description = "Returns autocall option price and greeks through Monte Carlo method")]
        public static object dtgm_phoenixautocall([ExcelArgument(Description = "S0")] double S0,
            [ExcelArgument(Description  = "Interest Rate")] double r,
            [ExcelArgument(Description = "Carry")] double b,
            [ExcelArgument(Description = "Volatility")] double vol,
            [ExcelArgument(Description = "fixing observation knock out days")] double[] fixings,
            [ExcelArgument(Description = "Remained Maturity")] double remained_T,
            [ExcelArgument(Description = "Total Maturity")] double total_T,
            [ExcelArgument(Description = "knock out price")] double ko_price,
            [ExcelArgument(Description = "knock in price")] double ki_price,
            [ExcelArgument(Description = "ki_type,swap or put")] string ki_type,
            [ExcelArgument(Description = "Strike")] double K,
            [ExcelArgument(Description = "Coupon Rate")] double coupon,
            [ExcelArgument(Description = "Rebate Rate")] double rebate,
            [ExcelArgument(Description = "Funding Rate")] double funding,
            [ExcelArgument(Description = "If pay is annualized")] double annpay,
            [ExcelArgument(Description = "N Simulations")] int nsims)
        {


            double[] _result = OPLib.AutoCallMethod.PhoenixAutoCallable(S0, r, b, vol, fixings, remained_T, total_T, ko_price, ki_price, ki_type, K, coupon, rebate,  funding, annpay, nsims);
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
        public static object dtem_phoenixautocall([ExcelArgument(Name = "OutPutFlag", Description = Flag.OutputFlag)] string outputflag,
            [ExcelArgument(Description = "S0")] double S0,
            [ExcelArgument(Description = "Interest Rate")] double r,
            [ExcelArgument(Description = "Carry")] double b,
            [ExcelArgument(Description = "Volatility")] double vol,
            [ExcelArgument(Description = "fixing observation knock out days")] double[] fixings,
            [ExcelArgument(Description = "Remained Maturity")] double remained_T,
            [ExcelArgument(Description = "Total Maturity")] double total_T,
            [ExcelArgument(Description = "knock out price")] double ko_price,
            [ExcelArgument(Description = "knock in price")] double ki_price,
            [ExcelArgument(Description = "ki_type,swap or put")] string ki_type,
            [ExcelArgument(Description = "Strike")] double K,
            [ExcelArgument(Description = "Coupon Rate")] double coupon,
            [ExcelArgument(Description = "Rebate Rate")] double rebate,
            [ExcelArgument(Description = "Funding Rate")] double funding,
            [ExcelArgument(Description = "If pay is annualized")] double annpay,
            [ExcelArgument(Description = "N Simulations")] int nsims)
        {

            double[] _result = OPLib.AutoCallMethod.PhoenixAutoCallable(S0, r, b, vol, fixings, remained_T, total_T, ko_price, ki_price, ki_type, K, coupon, rebate,  funding, annpay, nsims);
            object[] result = new object[5];
            for (int i = 0; i < 5; i++)
            {
                if (double.IsNaN(_result[i])) { result[i] = ExcelError.ExcelErrorValue; }
                else {result[i] = _result[i]; }
            }
            if (outputflag == "p") { return result[0]; }
            else if (outputflag == "d") { return result[1]; }
            else if (outputflag == "gp") { return result[2]; }
            else if (outputflag == "v") { return result[3]; }
            else if (outputflag == "t") { return result[4]; }
            else { return ExcelError.ExcelErrorValue; }
        }








        [ExcelFunction(IsVolatile = false, Description = "Returns vanilla option price and greeks through Black-Sholes-Merton method")]
        public static object dtgm_phoenixautocallAsync([ExcelArgument(Description = "S0")] double S0,
            [ExcelArgument(Description = "Interest Rate")] double r,
            [ExcelArgument(Description = "Carry")] double b,
            [ExcelArgument(Description = "Volatility")] double vol,
            [ExcelArgument(Description = "fixing observation knock out days")] double[] fixings,
            [ExcelArgument(Description = "Remained Maturity")] double remained_T,
            [ExcelArgument(Description = "Total Maturity")] double total_T,
            [ExcelArgument(Description = "knock out price")] double ko_price,
            [ExcelArgument(Description = "knock in price")] double ki_price,
            [ExcelArgument(Description = "ki_type,swap or put")] string ki_type,
            [ExcelArgument(Description = "Strike")] double K,
            [ExcelArgument(Description = "Coupon Rate")] double coupon,
            [ExcelArgument(Description = "Rebate Rate")] double rebate,
            [ExcelArgument(Description = "Funding Rate")] double funding,
            [ExcelArgument(Description = "If pay is annualized")] double annpay,
            [ExcelArgument(Description = "N Simulations")] int nsims)
        {

            object result_value = ExcelAsyncUtil.Run("dtgm_phoenixautocallAsync", new object[] { S0, r, b, vol, fixings, remained_T, total_T, ko_price, ki_price, ki_type, K, coupon, rebate,  funding, annpay, nsims, 0 },
            () => OPLib.AutoCallMethod.PhoenixAutoCallable(S0, r, b, vol, fixings, remained_T, total_T, ko_price, ki_price, ki_type, K, coupon, rebate,  funding, annpay, nsims));
            if (Equals(result_value, ExcelError.ExcelErrorNA))
            {
                return "Calculating...";
            }
            double[] _result_value = (double[])result_value;
            object[] result = new object[5];
            for (int i = 0; i < 5; i++)
            {
                if (double.IsNaN(_result_value[i]))
                {
                    result[i] = ExcelError.ExcelErrorValue;
                }
                else
                {
                    result[i] = _result_value[i];
                }
            }
            return result; 
        }



        [ExcelFunction(IsVolatile = false, Description = "Returns autocall option price or greek solely through Monte Carlo method")]
        public static object dtgm_autocall([ExcelArgument(Description = "S0")] double S0,
            [ExcelArgument(Description = "Interest Rate")] double r,
            [ExcelArgument(Description = "Carry")] double b,
            [ExcelArgument(Description = "Volatility")] double vol,
            [ExcelArgument(Description = "fixing observation knock out days")] double[] fixings,
            [ExcelArgument(Description = "Remained Maturity")] double remained_T,
            [ExcelArgument(Description = "Total Maturity")] double total_T,
            [ExcelArgument(Description = "knock out price")] double ko_price,
            [ExcelArgument(Description = "Coupon Rate")] double coupon,
            [ExcelArgument(Description = "Rebate Rate")] double rebate,
            [ExcelArgument(Description = "Funding Rate")] double funding,
            [ExcelArgument(Description = "If pay is annualized")] double annpay,
            [ExcelArgument(Description = "N Simulations")] int nsims)
        {

            double[] _result = OPLib.AutoCallMethod.AutoCallable_smooth(S0, r, b, vol, fixings, remained_T, total_T, ko_price, coupon, rebate, funding, annpay, nsims);
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




        [ExcelFunction(IsVolatile = false, Description = "Returns vanilla option price and greeks through Black-Sholes-Merton method")]
        public static object dtgm_autocallAsync([ExcelArgument(Description = "S0")] double S0,
            [ExcelArgument(Description = "Interest Rate")] double r,
            [ExcelArgument(Description = "Carry")] double b,
            [ExcelArgument(Description = "Volatility")] double vol,
            [ExcelArgument(Description = "fixing observation knock out days")] double[] fixings,
            [ExcelArgument(Description = "Remained Maturity")] double remained_T,
            [ExcelArgument(Description = "Total Maturity")] double total_T,
            [ExcelArgument(Description = "knock out price")] double ko_price,
            [ExcelArgument(Description = "Coupon Rate")] double coupon,
            [ExcelArgument(Description = "Rebate Rate")] double rebate,
            [ExcelArgument(Description = "Funding Rate")] double funding,
            [ExcelArgument(Description = "If pay is annualized")] double annpay,
            [ExcelArgument(Description = "N Simulations")] int nsims)
        {
            
            object result_value = ExcelAsyncUtil.Run("AutoCallable_smooth", new object[] { S0, r, b, vol, fixings, remained_T, total_T, ko_price, coupon, rebate, funding, annpay, nsims },
           () => OPLib.AutoCallMethod.AutoCallable_smooth(S0, r, b, vol, fixings, remained_T, total_T, ko_price, coupon, rebate, funding, annpay, nsims));
         
            

            
            return result_value;
        }



    }
}
