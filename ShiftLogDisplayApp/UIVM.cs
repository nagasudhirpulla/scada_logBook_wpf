using System;
using System.ComponentModel;

namespace ShiftLogDisplayApp
{
    public class UIVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
