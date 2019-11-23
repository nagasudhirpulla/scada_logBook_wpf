﻿using System;
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
            (string, double) maxWrSr = StatUtils.GetMaxValueInfo(wrSrVals, true);
            (string, double) minWrSr = StatUtils.GetMinValueInfo(wrSrVals, true);
            // get WR-NR data
            List<(string, double)> wrNrVals = EdnaUtils.GetData(startTime, endTime, EdnaUtils.WrNrPnt);
            (string, double) maxWrNr = StatUtils.GetMaxValueInfo(wrNrVals, true);
            (string, double) minWrNr = StatUtils.GetMinValueInfo(wrNrVals, true);
            // get WR-ER data
            List<(string, double)> wrErVals = EdnaUtils.GetData(startTime, endTime, EdnaUtils.WrErPnt);
            (string, double) maxWrEr = StatUtils.GetMaxValueInfo(wrErVals, true);
            (string, double) minWrEr = StatUtils.GetMinValueInfo(wrErVals, true);
            // get WR IR data
            List<(string, double)> wrIrVals = EdnaUtils.GetData(startTime, endTime, EdnaUtils.WrIrPnt);
            (string, double) maxWrIr = StatUtils.GetMaxValueInfo(wrIrVals, true);
            (string, double) minWrIr = StatUtils.GetMinValueInfo(wrIrVals, true);

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
            newRow.ItemArray = new string[] { maxFreq.Item2.ToString("0.###"), maxFreq.Item1, minFreq.Item2.ToString("0.###"), minFreq.Item1, avgFreq.ToString("0.###"), "", "", "" };
            logBookDt.Rows.Add(newRow);
            newRow = logBookDt.NewRow();
            newRow.ItemArray = new string[] { "Max Demand", "Max Demand Time", "Min Demand", "Min Demand Time", "", "", "", "" };
            logBookDt.Rows.Add(newRow);
            newRow = logBookDt.NewRow();
            newRow.ItemArray = new string[] { maxDem.Item2.ToString("F0"), maxDem.Item1, minDem.Item2.ToString("F0"), minDem.Item1, "", "", "", "" };
            logBookDt.Rows.Add(newRow);
            newRow = logBookDt.NewRow();
            newRow.ItemArray = new string[] { "Max WR-ER", "Min WR-ER", "Max WR-SR", "Min WR-SR", "Max WR-NR", "Min WR-NR", "Max IR", "Min IR" };
            logBookDt.Rows.Add(newRow);
            newRow = logBookDt.NewRow();
            newRow.ItemArray = new string[] { maxWrEr.Item2.ToString("F0"), minWrEr.Item2.ToString("F0"), maxWrSr.Item2.ToString("F0"), minWrSr.Item2.ToString("F0"), maxWrNr.Item2.ToString("F0"), minWrNr.Item2.ToString("F0"), maxWrIr.Item2.ToString("F0"), minWrIr.Item2.ToString("F0") };
            logBookDt.Rows.Add(newRow);
            LogBookDataView.ItemsSource = logBookDt.DefaultView;
        }

        private void GuessTimes_Click(object sender, RoutedEventArgs e)
        {
            SetInitialStartEndTimes();
        }
    }
}
