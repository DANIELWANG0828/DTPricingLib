// This source code is based on Jiri Pik's FinancialDataForExcel add-on from here:
// http://www.assembla.com/spaces/FinancialDataForExcel/wiki

using ExcelDna.Integration;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;

namespace DTPricingLib
{

    public class YahooFinance
    {
        
        [ExcelFunction("Get ...")]
        public static object MCpricingAsync([ExcelArgument("S0")] double S0, [ExcelArgument("Strike")] double X)
        {

            object result = ExcelAsyncUtil.Run("MCpricingAsync", new[] { S0, X },
                () => MCpricing(S0, X));

            if (Equals(result, ExcelError.ExcelErrorNA))
            {
                return "Calculating...";

            }

            return result;


        }


        /// <summary>
        /// This constant stores the duration of the cache in minutes. 
        /// </summary>
        private const double ExchangeRateCacheDuration = 15;

        /// <summary>
        /// This dictionary implements the caching of market data
        /// </summary>
        private static Dictionary<string, ExchangeRate> exchangeRateCache = new Dictionary<string, ExchangeRate>();

        /// <summary>
        /// This method retrieves the current FX Rate from Yahoo
        /// </summary>
        /// <param name="fromCcy">
        /// The FROM currency ISO code
        /// </param>
        /// <param name="toCcy">
        /// The TO currency ISO code
        /// </param>
        /// <returns>
        /// The method returns the current FX rate
        /// </returns>
        /// <exception cref="cellMatrixException">
        /// An exception occurs when the download of data fails for any reason
        /// </exception>
        [ExcelFunction("Get...")]
        public static double MCpricing([ExcelArgument("S0")] double S0, [ExcelArgument("Strike")] double X)
        {

            var ticker = "MCpricing" + Convert.ToString(S0) + "_" + Convert.ToString(X);

            if (exchangeRateCache.ContainsKey(ticker))
            {
                var exchangeRateCachedRecord = exchangeRateCache[ticker];
                var timeSpan = DateTime.Now.Subtract(exchangeRateCachedRecord.TimeStamp);
                if (timeSpan.TotalMinutes < ExchangeRateCacheDuration)
                {
                    return exchangeRateCachedRecord.Value;
                }
                exchangeRateCache.Remove(ticker);
            }






            double exchangeRate;
            Matrix<double> z = Matrix<double>.Build.Random(300, 3000);
            Matrix<double> x = Matrix<double>.Build.Random(3000, 300);
            Matrix<double> y = z.Multiply(x);
            exchangeRate = y[110, 120];

            exchangeRateCache.Add(ticker, new ExchangeRate { Ticker = ticker, TimeStamp = DateTime.Now, Value = exchangeRate });
            return exchangeRate;
        }



        /// <summary>
        /// This class is used for caching of the retrieved results.
        /// </summary>
        private class ExchangeRate
        {
            /// <summary>
            /// Gets or sets the Yahoo! Ticker
            /// </summary>
            public string Ticker { get; set; }

            /// <summary>
            /// Gets or sets the Exchange Rate
            /// </summary>
            public double Value { get; set; }

            /// <summary>
            /// Gets or sets TimeStamp of the value
            /// </summary>
            public DateTime TimeStamp { get; set; }
        }
    }
}