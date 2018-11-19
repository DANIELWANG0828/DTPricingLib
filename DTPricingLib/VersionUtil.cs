using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExcelDna.Integration;
namespace DTPricingLib
{
    public class VersionUtil
    {
        public static object dtpricinglib_version()
        {
            return FuncsList.funcs;
        }
    }
}
