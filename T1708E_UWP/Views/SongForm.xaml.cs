using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using T1708E_UWP.Entity;
using T1708E_UWP.Service;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace T1708E_UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SongForm : Page
    {
        private Song currentSong = new Song();
        private bool isImageValid = false;
        public SongForm()
        {
            this.InitializeComponent();
            Service.ProgressBar.Hide();
        }

        private async void BtnSignup_Click(object sender, RoutedEventArgs e)
        {
            
            if (CheckSong(this.Link.Text))
            {
                Service.ProgressBar.SetProgress(40, true);
                Revert();
                this.currentSong.name = this.Name.Text;
                this.currentSong.description = this.Description.Text;
                this.currentSong.singer = this.Singer.Text;
                this.currentSong.author = this.Author.Text;
                this.currentSong.thumbnail = this.Thumbnail.Text;
                this.currentSong.link = this.Link.Text;
                Service.ProgressBar.SetProgress(60, true);
                string responseContent = await ApiHandle<Song>.Call(APITypes.CreateSong, this.currentSong);
                try
                {
                    Song resp = JsonConvert.DeserializeObject<Song>(responseContent);
                    if (resp.link != null && resp.link == this.currentSong.link)
                    {
                        await Dialog.Show("Add song success!");
                    }
                    else song.Visibility = Visibility.Visible;
                }
                catch
                {
                    ApiHandle<string>.ThrowException(responseContent);
                } finally
                {
                    Service.ProgressBar.Hide();
                }
            }
        }

        private void Revert()
        {
            if (link.Visibility == Visibility.Visible) link.Visibility = Visibility.Collapsed;
            if (song.Visibility == Visibility.Visible) song.Visibility = Visibility.Collapsed;
        }
        private bool CheckSong(string link)
        {
            bool isPassed = false;
            if (link.Contains("http") != true || link.Contains("mp3") != true)
            {
                this.link.Visibility = Visibility.Visible;
                isPassed = false;
            }
            else isPassed = true;
            if (isImageValid) isPassed = true;
            else isPassed = false;
            return isPassed;
        }

        private void Thumbnail_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                ThumbSong.Source = new BitmapImage(new Uri(Thumbnail.Text));
                isImageValid = true;
                thumb.Visibility = Visibility.Collapsed;
            } catch(System.Exception)
            {
                ThumbSong.Source = null;
                isImageValid = false;
                thumb.Visibility = Visibility.Visible;
            }
        }
    }
}
