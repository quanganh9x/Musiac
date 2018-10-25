using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using T1708E_UWP.Entity;
using T1708E_UWP.Service;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    public sealed partial class LoginForm : Page
    {
        public LoginForm()
        {
            this.InitializeComponent();
        }

        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (CheckLogin(this.Email.Text))
            {
                Revert();
                Dictionary<String, String> LoginInfo = new Dictionary<string, string>();
                LoginInfo.Add("email", this.Email.Text);
                LoginInfo.Add("password", this.Password.Password);
                string responseContent = await ApiHandle<Dictionary<string, string>>.Call(APITypes.SignIn, LoginInfo);
                try
                {
                    Response resp = JsonConvert.DeserializeObject<Response>(responseContent);
                    if (resp.token != null)
                    {
                        if (rem.IsChecked == true) await FileHandle.Save("token.ini", resp.token);
                        FrameSwitcher.Switch(typeof(Views.NavigationView));
                    }
                    else
                    {
                        Entity.Exception err = JsonConvert.DeserializeObject<Entity.Exception>(responseContent);
                        login.Text = err.message;
                        login.Visibility = Visibility.Visible;
                    }
                }
                catch
                {
                    ApiHandle<string>.ThrowException(responseContent);

                }
            }
            else email.Visibility = Visibility.Visible;
        }

        private bool CheckLogin(string email)
        {
            if (email.Contains("@") && email.Contains(".")) return true;
            return false;
        }

        private void Revert()
        {
            if (email.Visibility == Visibility.Visible)
            email.Visibility = Visibility.Collapsed;
            if (login.Visibility == Visibility.Visible)
                login.Visibility = Visibility.Collapsed;
        }
        private async void BtnSignUp_Click(object sender, RoutedEventArgs e)
        {
            FrameSwitcher.Switch(typeof(Views.RegisterForm));
        }
        
    }
}
