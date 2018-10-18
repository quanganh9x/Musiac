using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using T1708E_UWP.Service;
using T1708E_UWP.Entity;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using MUXC = Microsoft.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace T1708E_UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NavigationView : Page
    {
       
        public NavigationView()
        {
            this.InitializeComponent();
        }
        
        private Type currentPage;
        private Entity.Progress Bar = Service.ProgressBar.Bar; // for xaml binding. uwp's x:Bind is too confusing and not efficient!

        // List of ValueTuple holding the Navigation Tag and the relative Navigation Page 
        private readonly IList<(string Tag, Type Page, string Title)> _pages = new List<(string Tag, Type Page, string Title)>
        {
            
            ("info", typeof(Views.AccountInfo), "Account Info"),
            ("listsongs", typeof(Views.ListSongs), "List Songs"),
            ("addsongs", typeof(Views.SongForm), "Add Songs"),
        };


        public void SetHeader(string Title)
        {
            NavView.Header = Title;
        }
        
        

        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigated += On_Navigated;
            NavView_Navigate("info");
        }

        private void NavView_Navigate(string navItemTag)
        {
            var item = _pages.First(p => p.Tag.Equals(navItemTag));
            if (currentPage == item.Page)
                return;
            this.SetHeader(item.Title);
            ContentFrame.Navigate(item.Page);
            currentPage = item.Page;
        }

        private void On_Navigated(object sender, NavigationEventArgs e)
        {
            var item = _pages.First(p => p.Page == e.SourcePageType);

                NavView.SelectedItem = NavView.MenuItems
                    .OfType<NavigationViewItem>()
                    .First(n => n.Tag.Equals(item.Tag));
        }

        private void NavView_ItemInvoked(Windows.UI.Xaml.Controls.NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.InvokedItem == null)
                return;
            var navItemTag = NavView.MenuItems
            .OfType<NavigationViewItem>()
            .First(i => args.InvokedItem.Equals(i.Content))
            .Tag.ToString();

            Service.ProgressBar.SetProgress(30, true);
            NavView_Navigate(navItemTag);
        }
    }

    
}
