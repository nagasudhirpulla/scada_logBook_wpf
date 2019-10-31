using InStep.eDNA.EzDNAApiNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftLogDisplayApp
{
    public class EdnaUtils
    {
        public const string FreqPnt = "WRLDCMP.SCADA1.A47000";
        public const string DemPnt = "WRLDCMP.SCADA1.A47000";
        public static List<(string, double)> GetData(DateTime startTime, DateTime endTime, string pnt)
        {
            List<(string, double)> historyResults = new List<(string, double)>();
            try
            {
                string status = "";
                // history request initiation
                int nret = History.DnaGetHistRaw(pnt, startTime, endTime, out uint s);

                while (nret == 0)
                {
                    nret = History.DnaGetNextHist(s, out double dval, out DateTime timestamp, out status);
                    if (status != null)
                    {
                        historyResults.Add((timestamp.ToString("HH:mm:ss"), dval));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while fetching history results " + ex.Message);
                historyResults = new List<(string, double)>();
            }
            return historyResults;
        }
    }
}
