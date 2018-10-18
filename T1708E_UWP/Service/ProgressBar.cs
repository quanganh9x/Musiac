using System;
using System.Collections.Generic;
using System.Linq;
using T1708E_UWP.Entity;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace T1708E_UWP.Service
{
    public class ProgressBar
    {
        public static Progress Bar = new Progress();
        private static bool Debugable = true;

        public static void SetProgress(Double progress, bool Visible)
        {
            Visibility visible = Visibility.Collapsed;
            if (Visible) visible = Visibility.Visible;
            Bar.Set(progress, visible);
        }

        public static void Hide()
        {
            SetProgress(0, false);
        }

        public static void SetDebug(string text)
        {
            if (Debugable) Bar.SetText(text);
        }
    }
}
