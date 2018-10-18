using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace T1708E_UWP.Entity
{
    public class Progress: INotifyPropertyChanged
    {
        private Double _barProgress = 0;
        private Visibility _visible = Visibility.Collapsed;
        private String _barText = "0 %";
        public Double Value { get => _barProgress; set { _barProgress = value; this.NotifyPropertyChanged("Value"); } }
        public Visibility Visible { get => _visible; set { _visible = value; this.NotifyPropertyChanged("Visible"); } }
        public string ValueText { get => _barText; set { _barText = value; this.NotifyPropertyChanged("ValueText"); } }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] string propName = "")
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        #endregion

        public void Set(Double barProgress, Visibility visible)
        {
            Value = barProgress;
            Visible = visible;
            ValueText = barProgress.ToString() + " %";
        }

        public void SetText(string dbg)
        {
            Value = 0;
            Visible = Visibility.Visible;
            ValueText = dbg;
        }
    }
}
