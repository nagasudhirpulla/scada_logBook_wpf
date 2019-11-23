using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftLogDisplayApp
{
    public class StatUtils
    {
        public static (string, double) GetMaxValueInfo(List<(string, double)> dataInp, bool considerSign)
        {
            (string, double) res = ("", 0);
            if (dataInp.Count == 0)
            {
                return res;
            }
            else
            {
                res = (dataInp[0]);
            }
            for (int i = 0; i < dataInp.Count; i++)
            {
                if (considerSign == true && (res.Item2 < dataInp[i].Item2))
                {
                    res = dataInp[i];
                }
                else if (considerSign == false && (Math.Abs(res.Item2) < Math.Abs(dataInp[i].Item2)))
                {
                    res = dataInp[i];
                }
            }
            return res;
        }

        public static (string, double) GetMinValueInfo(List<(string, double)> dataInp, bool considerSign)
        {
            (string, double) res = ("", 0);
            if (dataInp.Count == 0)
            {
                return res;
            }
            else
            {
                res = (dataInp[0]);
            }
            for (int i = 0; i < dataInp.Count; i++)
            {
                if (considerSign == true && (res.Item2 > dataInp[i].Item2))
                {
                    res = dataInp[i];
                }
                else if (considerSign == false && (Math.Abs(res.Item2) > Math.Abs(dataInp[i].Item2)))
                {
                    res = dataInp[i];
                }
            }
            return res;
        }

        public static double GetAvgValue(List<(string, double)> dataInp)
        {
            double res = 0;
            if (dataInp.Count == 0)
            {
                return res;
            }

            for (int i = 0; i < dataInp.Count; i++)
            {
                res += dataInp[i].Item2;
            }
            return (res / dataInp.Count);
        }
    }
}
