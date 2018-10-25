using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace T1708E_UWP.Service
{
    public class Dialog
    {
        public static async Task Show(string message)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "T1708E-UWP",
                Content = message,
                CloseButtonText = ":)"
            };
            await dialog.ShowAsync();
        }
    }
}
