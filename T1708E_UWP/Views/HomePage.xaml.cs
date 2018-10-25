using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class HomePage : Page
    {
        private Frame f;
        private ObservableCollection<HomeItem> _list = new ObservableCollection<HomeItem>();
        
        public ObservableCollection<HomeItem> List { get => _list; set => _list = value; }

        public HomePage()
        {
            LoadProfile();
            Add();
            Service.ProgressBar.SetProgress(70, true);
            this.InitializeComponent();
            f = Window.Current.Content as Frame;
            Service.ProgressBar.Hide();
        }

        private void Add()
        {
            _list.Add(new HomeItem("List", "List of songs. My favourite", typeof(ListSongs)));
            _list.Add(new HomeItem("Add", "Add new", typeof(SongForm)));
        }

        private async void LoadProfile()
        {
            try
            {
                Entity.Member user = JsonConvert.DeserializeObject<Entity.Member>(await ApiHandle<string>.Call(APITypes.GetInfo, null));
                thumb.Source = new BitmapImage(new Uri(user.avatar));
                header.Text = user.firstName + " " + user.lastName;
                lName.Text = "Last name: " + user.lastName;
                fName.Text = "First name: " + user.firstName;
                email.Text = "Email: " + user.email;
                phone.Text = "Phone number: " + user.phone;
                introduction.Text = "Intro: " + user.introduction;
                address.Text = "Address: " + user.address;
                birthday.Text = "Birthday: " + user.birthday;
            } catch
            {
                ExceptionHandle.ThrowDebug("LOL");
            }
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            f.Navigate(typeof(LoginForm));
        }

        private void GridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (gridList.SelectedItem != null)
            {
                HomeItem item = (HomeItem)gridList.SelectedItem;
                f.Navigate(item.Navi);
            }
        }
    }

    public class HomeItem
    {
        private string _name;
        private string _description;
        private Type _navi;

        public Type Navi { get => _navi; set => _navi = value; }
        public string Name { get => _name; set => _name = value; }
        public string Description { get => _description; set => _description = value; }

        public HomeItem(string name, string description, Type navi)
        {
            _name = name;
            _navi = navi;
            _description = description;
        }
    }
}
