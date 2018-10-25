using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using T1708E_UWP.Entity;
using T1708E_UWP.Service;
using T1708E_UWP.Views;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace T1708E_UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ListSongs : Page
    {
        
        private ObservableCollection<Song> _songs = new ObservableCollection<Song>();
        public ObservableCollection<Song> Songs { get => _songs; set => _songs = value; }
        private int selected = 0;
        private Tooltip box = new Tooltip();

        public ListSongs()
        {
            this.InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs args)
        {
            await Load();

        }

        private async Task Load()
        {
            Service.ProgressBar.SetProgress(60, true);
            if (selected == 0) this._songs = await SongHandle.Get(false);
            else this._songs = await SongHandle.GetMine(false);
            itemListView.MaxHeight = ((Frame)Window.Current.Content).ActualHeight - listTool.ActualHeight - mediaPlayer.ActualHeight - 60; // dirty fix
            itemListView.ItemsSource = null;
            itemListView.ItemsSource = Songs; // native way to bind an observablecollection in uwp
            GC.Collect();
            Service.ProgressBar.Hide();
        } 
        

        private void itemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (itemListView.SelectedItem != null)
            {
                try
                {
                    Service.ProgressBar.SetProgress(50, true);
                    Song item = (Song)itemListView.SelectedItem;
                    mediaPlayer.Source = MediaSource.CreateFromUri(new Uri(item.link));
                    mediaPlayer.AutoPlay = true;
                    Service.ProgressBar.Hide();
                } catch
                {
                    ExceptionHandle.ThrowDebug("link error");
                }
            }
            
        }
        
        
        private async void AppBarButton_Click_Custom(object sender, RoutedEventArgs e)
        {
            // may be affected by *sandbox-san*. please let me pass through, sandbox-san!
            if (selected == 0)
            await FileHandle.SaveToFile(await ApiHandle<string>.Call(APITypes.GetSongs, null));
            else await FileHandle.SaveToFile(await ApiHandle<string>.Call(APITypes.GetSavedSongs, null));

        }
        
        private async void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (selected != Tooltip.SelectedIndex) await Load();
            selected = Tooltip.SelectedIndex;
        }
        
    }

    public class Tooltip: INotifyPropertyChanged
    {
        private Visibility _visible = Visibility.Visible;
        public Visibility Visible { get => _visible; set { _visible = value; this.NotifyPropertyChanged("Visible"); } }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] string propName = "")
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        #endregion

        public void ChangeProperty()
        {
            if (Visible == Visibility.Visible) Visible = Visibility.Collapsed;
            else Visible = Visibility.Visible;
        }
    }
}
