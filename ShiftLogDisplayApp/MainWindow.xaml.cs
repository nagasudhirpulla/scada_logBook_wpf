using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShiftLogDisplayApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = UIEditVM;
            SetInitialStartEndTimes();
        }

        private UIVM UIEditVM = new UIVM();

        private void SetInitialStartEndTimes()
        {
            DateTime nowTime = DateTime.Now;
            DateTime startTime = DateTime.Now;
            DateTime endTime = DateTime.Now;

            if (nowTime.Hour >= 6 && nowTime.Hour <= 11)
            {
                // if time is [06.00, 11.00] then set times for night shift - 21.00 to 08.00
                startTime = nowTime.Date.AddDays(-1).AddHours(21);
                endTime = nowTime.Date.AddMinutes(Math.Min(nowTime.Hour * 60 + nowTime.Minute, 8 * 60));
            }
            else if (nowTime.Hour >= 13 && nowTime.Hour <= 16)
            {
                // if time is [13.00, 16.00] then set times for morning shift - 08.00 to 14.30
                startTime = nowTime.Date.AddHours(8);
                endTime = nowTime.Date.AddMinutes(Math.Min(nowTime.Hour * 60 + nowTime.Minute, 14 * 60 + 30));
            }
            else if (nowTime.Hour >= 19 && nowTime.Hour <= 23)
            {
                // if time is [19.00, 23.00] then set times for evening shift - 14.30 to 21.00
                startTime = nowTime.Date.AddHours(14).AddMinutes(30);
                endTime = nowTime.Date.AddMinutes(Math.Min(nowTime.Hour * 60 + nowTime.Minute, 21 * 60));
            }

            // set the start and end Times of UI
            UIEditVM.StartTime = startTime;
            UIEditVM.EndTime = endTime;
            UIEditVM.OnPropertyChanged(nameof(UIEditVM.StartTime));
            UIEditVM.OnPropertyChanged(nameof(UIEditVM.EndTime));
        }



        private void Update_Click(object sender, RoutedEventArgs e)
        {
            // get start and end times
            DateTime startTime = StartTimePicker.Value.GetValueOrDefault();
            DateTime endTime = EndTimePicker.Value.GetValueOrDefault();
            // get freq data
            List<(string, double)> freqVals = EdnaUtils.GetData(startTime, endTime, EdnaUtils.FreqPnt);
            (string, double) maxFreq = StatUtils.GetMaxValueInfo(freqVals, true);
            (string, double) minFreq = StatUtils.GetMinValueInfo(freqVals, true);
            double avgFreq = StatUtils.GetAvgValue(freqVals);
            // get demand data
            List<(string, double)> demVals = EdnaUtils.GetData(startTime, endTime, EdnaUtils.DemPnt);
            (string, double) maxDem = StatUtils.GetMaxValueInfo(demVals, true);
            (string, double) minDem = StatUtils.GetMinValueInfo(demVals, true);
            // get WR-SR data
            List<(string, double)> wrSrVals = EdnaUtils.GetData(startTime, endTime, EdnaUtils.WrSrPnt);
            List<(string, double)> exportVals = wrSrVals.Where(x => x.Item2 < 0).ToList();
            List<(string, double)> importVals = wrSrVals.Where(x => x.Item2 > 0).ToList();
            (string, double) maxWrSrEx = StatUtils.GetMaxValueInfo(exportVals, true);
            (string, double) minWrSrEx = StatUtils.GetMinValueInfo(exportVals, true);
            (string, double) maxWrSrImp = StatUtils.GetMaxValueInfo(importVals, true);
            (string, double) minWrSrImp = StatUtils.GetMinValueInfo(importVals, true);
            // get WR-NR data
            List<(string, double)> wrNrVals = EdnaUtils.GetData(startTime, endTime, EdnaUtils.WrNrPnt);
            exportVals = wrNrVals.Where(x => x.Item2 < 0).ToList();
            importVals = wrNrVals.Where(x => x.Item2 > 0).ToList();
            (string, double) maxWrNrEx = StatUtils.GetMaxValueInfo(exportVals, true);
            (string, double) minWrNrEx = StatUtils.GetMinValueInfo(exportVals, true);
            (string, double) maxWrNrImp = StatUtils.GetMaxValueInfo(importVals, true);
            (string, double) minWrNrImp = StatUtils.GetMinValueInfo(importVals, true);
            // get WR-ER data
            List<(string, double)> wrErVals = EdnaUtils.GetData(startTime, endTime, EdnaUtils.WrErPnt);
            exportVals = wrErVals.Where(x => x.Item2 < 0).ToList();
            importVals = wrErVals.Where(x => x.Item2 > 0).ToList();
            (string, double) maxWrErEx = StatUtils.GetMaxValueInfo(exportVals, true);
            (string, double) minWrErEx = StatUtils.GetMinValueInfo(exportVals, true);
            (string, double) maxWrErImp = StatUtils.GetMaxValueInfo(importVals, true);
            (string, double) minWrErImp = StatUtils.GetMinValueInfo(importVals, true);
            // get WR IR data
            List<(string, double)> wrIrVals = EdnaUtils.GetData(startTime, endTime, EdnaUtils.WrIrPnt);
            exportVals = wrIrVals.Where(x => x.Item2 < 0).ToList();
            importVals = wrIrVals.Where(x => x.Item2 > 0).ToList();
            (string, double) maxWrIrEx = StatUtils.GetMaxValueInfo(exportVals, true);
            (string, double) minWrIrEx = StatUtils.GetMinValueInfo(exportVals, true);
            (string, double) maxWrIrImp = StatUtils.GetMaxValueInfo(importVals, true);
            (string, double) minWrIrImp = StatUtils.GetMinValueInfo(importVals, true);

            DataTable logBookDt = new DataTable();
            int numColumns = 8;
            for (var c = 0; c < numColumns; c++)
            {
                logBookDt.Columns.Add(new DataColumn(c.ToString()));
            }
            DataRow newRow = logBookDt.NewRow();
            newRow.ItemArray = new string[] { "Max Freq", "Max Freq Time", "Min Freq", "Min Freq Time", "Avg Freq", "", "", "" };
            logBookDt.Rows.Add(newRow);
            newRow = logBookDt.NewRow();
            newRow.ItemArray = new string[] { maxFreq.Item2.ToString("0.###"), TillMinsOnly(maxFreq.Item1), minFreq.Item2.ToString("0.###"), TillMinsOnly(minFreq.Item1), avgFreq.ToString("0.###"), "", "", "" };
            logBookDt.Rows.Add(newRow);
            newRow = logBookDt.NewRow();
            newRow.ItemArray = new string[] { "Max Demand", "Max Demand Time", "Min Demand", "Min Demand Time", "", "", "", "" };
            logBookDt.Rows.Add(newRow);
            newRow = logBookDt.NewRow();
            newRow.ItemArray = new string[] { maxDem.Item2.ToString("F0"), TillMinsOnly(maxDem.Item1), minDem.Item2.ToString("F0"), TillMinsOnly(minDem.Item1), "", "", "", "" };
            logBookDt.Rows.Add(newRow);
            newRow = logBookDt.NewRow();
            newRow.ItemArray = new string[] { "Export values", "", "", "", "", "", "", "" };
            logBookDt.Rows.Add(newRow);
            newRow = logBookDt.NewRow();
            newRow.ItemArray = new string[] { "Min WR-ER", "Max WR-ER", "Min WR-SR", "Max WR-SR", "Min WR-NR", "Max WR-NR", "Min IR", "Max IR" };
            logBookDt.Rows.Add(newRow);
            newRow = logBookDt.NewRow();
            newRow.ItemArray = new string[] { maxWrErEx.Item2.ToString("F0"), minWrErEx.Item2.ToString("F0"), maxWrSrEx.Item2.ToString("F0"), minWrSrEx.Item2.ToString("F0"), maxWrNrEx.Item2.ToString("F0"), minWrNrEx.Item2.ToString("F0"), maxWrIrEx.Item2.ToString("F0"), minWrIrEx.Item2.ToString("F0") };
            logBookDt.Rows.Add(newRow);
            newRow = logBookDt.NewRow();
            newRow.ItemArray = new string[] { "Import values", "", "", "", "", "", "", "" };
            logBookDt.Rows.Add(newRow);
            newRow = logBookDt.NewRow();
            newRow.ItemArray = new string[] { "Min WR-ER", "Max WR-ER", "Min WR-SR", "Max WR-SR", "Min WR-NR", "Max WR-NR", "Min IR", "Max IR" };
            logBookDt.Rows.Add(newRow);
            newRow = logBookDt.NewRow();
            newRow.ItemArray = new string[] { minWrErImp.Item2.ToString("F0"), maxWrErImp.Item2.ToString("F0"), minWrSrImp.Item2.ToString("F0"), maxWrSrImp.Item2.ToString("F0"), minWrNrImp.Item2.ToString("F0"), maxWrNrImp.Item2.ToString("F0"), minWrIrImp.Item2.ToString("F0"), maxWrIrImp.Item2.ToString("F0") };
            logBookDt.Rows.Add(newRow);
            LogBookDataView.ItemsSource = logBookDt.DefaultView;
        }

        private void GuessTimes_Click(object sender, RoutedEventArgs e)
        {
            SetInitialStartEndTimes();
        }

        private string TillMinsOnly(string timeStr)
        {
            string subStr =
            !String.IsNullOrWhiteSpace(timeStr) && timeStr.Length >= 5
            ? timeStr.Substring(0, 5)
            : timeStr;

            return subStr;
        }
    }
}
