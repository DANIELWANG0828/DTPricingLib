using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Linq;
namespace OptionPricingLib
{
    class Halton
    {
        private int _dim;
        private int _seed;
        public static int[] primes = { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 43, 47, 53, 59 };
        public static double[] A = { 2.50662823884, -18.61500062529, 41.39119773534, -25.44106049637 };
        public static double[] B = { -8.47351093090, 23.08336743743, -21.06224101826, 3.13082909833 };
        public static double[] C = {0.3374754822726147, 0.9761690190917186,
                                    0.1607979714918209, 0.0276438810333863,
                                    0.0038405729373609, 0.0003951896511919,
                                    0.0000321767881768, 0.0000002888167364,
                                    0.0000003960315187};
        public Halton(int dim, int seed)
        {
            _dim = dim;
            _seed = seed;
        }
        private double GetHalton(int index, int halton_base)
        {
            int digit;
            double base_inv = 1.0 / ((double)halton_base);
            double r = 0.0;
            while (index != 0)
            {
                digit = index % halton_base;
                r = r + (double)digit * base_inv;
                base_inv = base_inv / (double)halton_base;
                index = index / halton_base;
            }
            return r;
        }
        private double StdNormInv(double u)
        {
            double x = 0.0;
            double y = u - 0.5;
            if (Math.Abs(y) < 0.42)
            {
                double r = y * y;
                x = y * (((A[3] * r + A[2]) * r + A[1]) * r + A[0]) / ((((B[3] * r + B[2]) * r + B[1]) * r + B[0]) * r + 1);
            }
            else
            {
                double r = u;
                if (y > 0) r = 1 - u; // really y >= 0.42
                r = Math.Log(-Math.Log(r));
                x = C[0] + r * (C[1] + r * (C[2] + r * (C[3] + r * (C[4] + r * (C[5] + r * (C[6] + r * (C[7] + r * C[8])))))));
                if (y < 0) x = -x;
            }
            return x;
        }
        private double[] GetNext()
        {
            double[] Ran = new double[_dim];
            for (int i = 0; i < _dim; i++)
            {
                double z = GetHalton(_seed, primes[i]);
                Ran[i] = z;
            }
            return Ran;
        }
        public double[] GetNextGaussian()
        {
            double[] Gau = new double[_dim];
            double[] Ran = GetNext();
            for (int i = 0; i < _dim; i++)
                Gau[i] = (StdNormInv(Ran[i]));
            return Gau;
        }
    }
    public class MultiAssetSpreadQMCMethod
    {
        public static double GetST0(double S, double T, double b, double v, double fi)
        {
            return S * Math.Pow(Math.E, ((b - v * v / 2.0) * T + v * Math.Sqrt(T) * fi));
        }
        public static double GetST0_Antithetic(double S, double T, double b, double v, double fi)
        {
            return S * Math.Pow(Math.E, ((b - v * v / 2.0) * T - v * Math.Sqrt(T) * fi));
        }

        public static double GetPayOff(string cpflg, double[] Wgt, double[] S, double X, double T, double r, double[] b, double[] v, Matrix<double> cholesky, int p)
        {
            double option_price = double.NaN;
            int num = S.Length;
            double[] ST0_vec = new double[num];
            double[] ST0_antithetic_vec = new double[num];
            double ST0_sum = 0;
            double ST0_antithetic_sum = 0;
            double[] fi_vec = new double[num];

            //double b = Normal.Sample(new MersenneTwister(), 0.0, 1.0);
            //double[] c = new double[num];
            //Normal.Samples(c, 0.0, 1.0);
            Halton ht = new Halton(num, p + 1);
            double[] c = ht.GetNextGaussian();
            for (var i = 0; i < num; i++)
            {
                double _fi = 0;
                for (var j = 0; j < num; j++)
                {
                    _fi = _fi + cholesky[i, j] * c[j];
                }
                fi_vec[i] = _fi;
                ST0_vec[i] = Wgt[i] * GetST0(S[i], T, b[i], v[i], _fi);
                ST0_antithetic_vec[i] = Wgt[i] * GetST0_Antithetic(S[i], T, b[i], v[i], _fi);
                ST0_sum = ST0_sum + ST0_vec[i];
                ST0_antithetic_sum = ST0_antithetic_sum + ST0_antithetic_vec[i];
            }
            if (cpflg == "c")
            {
                option_price = (Math.Max(ST0_sum - X, 0) + Math.Max(ST0_antithetic_sum - X, 0)) / 2.0;
            }
            else if (cpflg == "p")
            {
                option_price = (Math.Max(-ST0_sum + X, 0) + Math.Max(-ST0_antithetic_sum + X, 0)) / 2.0;
            }
            else
            {
                option_price = double.NaN;
            }
            return option_price;
        }
        public static double SpreadOptionQMC(string cpflg, double[] Wgt, double[] S, double X, double T, double r, double[] b, double[] v, double[,] CorrMat, int SimTimes)
        {
            int num = S.Length;
            var matrix = new DenseMatrix(num);
            for (var i = 0; i < num; i++)
            {
                for (var j = 0; j < num; j++)
                {
                    matrix[i, j] = CorrMat[i, j];
                }
            }
            var cholesky = matrix.Cholesky().Factor;
            double option_price = 0;
            for (int i = 0; i < SimTimes; i++)
            {
                option_price = option_price + GetPayOff(cpflg, Wgt, S, X, T, r, b, v, cholesky, i);
            }
            return option_price / SimTimes;
        }
    }

    public class BestOrWorstCallOptionQMCMethod
    {
        public static double GetST0(double S, double T, double b, double v, double fi)
        {
            return S * Math.Pow(Math.E, ((b - v * v / 2.0) * T + v * Math.Sqrt(T) * fi));
        }
        public static double GetST0_Antithetic(double S, double T, double b, double v, double fi)
        {
            return S * Math.Pow(Math.E, ((b - v * v / 2.0) * T - v * Math.Sqrt(T) * fi));
        }

        public static double GetPayOff(string BestOrWorstFlg, double[] S, double[] X, double T, double r, double[] b, double[] v, Matrix<double> cholesky, int p)
        {
            double option_price = double.NaN;
            int num = S.Length;
            double[] ST0_vec = new double[num];
            double[] ST0_antithetic_vec = new double[num];
            double[] po = new double[num];
            double[] fi_vec = new double[num];
            double[] a = { 1, 23, 4 };
            //double b = Normal.Sample(new MersenneTwister(), 0.0, 1.0);
            //double[] c = new double[num];
            //Normal.Samples(c, 0.0, 1.0);
            Halton ht = new Halton(num, p + 1);
            double[] c = ht.GetNextGaussian();
            for (var i = 0; i < num; i++)
            {
                double _fi = 0;
                for (var j = 0; j < num; j++)
                {
                    _fi = _fi + cholesky[i, j] * c[j];
                }
                fi_vec[i] = _fi;
                ST0_vec[i] = GetST0(S[i], T, b[i], v[i], _fi);
                //ST0_antithetic_vec[i] = (GetST0_Antithetic(S[i], T, b[i], v[i], _fi) + GetST0(S[i], T, b[i], v[i], _fi))/2;
                po[i] = Math.Max(ST0_vec[i] - X[i], 0);
                //po[i] = Math.Max(ST0_antithetic_vec[i] - X[i],0);
            }
            if (BestOrWorstFlg == "best")
            {   
                option_price = po.Max();
            }
            else if (BestOrWorstFlg == "worst")
            {
                option_price = po.Min();
            }
            else
            {
                option_price = double.NaN;
            }
            return option_price;
        }
        public static double BestOrWorstCallOptionQMC(string BestOrWorstFlg, double[] S, double[] X, double T, double r, double[] b, double[] v, double[,] CorrMat, int SimTimes)
        {
            int num = S.Length;
            var matrix = new DenseMatrix(num);
            for (var i = 0; i < num; i++)
            {
                for (var j = 0; j < num; j++)
                {
                    matrix[i, j] = CorrMat[i, j];
                }
            }
            var cholesky = matrix.Cholesky().Factor;
            double option_price = 0;
            for (int i = 0; i < SimTimes; i++)
            {
                option_price = option_price + GetPayOff(BestOrWorstFlg, S, X, T, r, b, v, cholesky, i);
            }
            return option_price / SimTimes * Math.Exp(-T * r);
        }
    }



}
