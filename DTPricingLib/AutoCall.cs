using ExcelDna.Integration;
using System;
using OPLib = OptionPricingLib;

namespace DTPricingLib
{
    public class AutoCalls
    { 
        [ExcelFunction(IsVolatile = false,Description = "Returns vanilla option price and greeks through Black-Sholes-Merton method")]
        public static object dtgm_autocall(string outputflag, double S0, double r, double b,
           double vol, double[] fixings, double ko_price, object ki_price, double K,
           double coupon, double rebate, double nominal, double funding, double annpay, int nsims)
        {
            double _ki_price;
            if (ki_price is ExcelEmpty)
            {
                _ki_price = -1;
            }
            else
            {
                _ki_price = Convert.ToDouble(ki_price);
            }

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








        [ExcelFunction(IsVolatile = false, Description = "Returns vanilla option price and greeks through Black-Sholes-Merton method")]
        public static object dtgm_autocallAsync(string outputflag, double S0, double r, double b,
         double vol, double[] fixings, double ko_price, object ki_price, double K,
         double coupon, double rebate, double nominal, double funding, double annpay, int nsims)
        {
            double _ki_price;
            if (ki_price is ExcelEmpty)
            {
                _ki_price = -1;
            }
            else
            {
                _ki_price = Convert.ToDouble(ki_price);
            }

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
