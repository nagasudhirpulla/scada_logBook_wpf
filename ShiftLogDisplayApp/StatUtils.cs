using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftLogDisplayApp
{
    public class StatUtils
    {
        public static (string, double) GetMaxValueData(List<(string, double)> dataInp)
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
                if (res.Item2 < dataInp[i].Item2)
                {
                    res = dataInp[i];
                }
            }
            return res;
        }

        public static (string, double) GetMinValueData(List<(string, double)> dataInp)
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
                if (res.Item2 > dataInp[i].Item2)
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
