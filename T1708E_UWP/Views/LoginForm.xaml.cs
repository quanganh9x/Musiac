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
        private bool isChecked = true;
        public LoginForm()
        {
            this.InitializeComponent();
        }

        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<String, String> LoginInfo = new Dictionary<string, string>();
            LoginInfo.Add("email", this.Email.Text);
            LoginInfo.Add("password", this.Password.Password);
            string responseContent = await ApiHandle<Dictionary<string, string>>.Call(APITypes.SignIn, LoginInfo);
            try
            {
                Response resp = JsonConvert.DeserializeObject<Response>(responseContent);
                if (resp.token != null)
                {
                    if (isChecked) await FileHandle.Save("token.ini", resp.token);
                    Debug.WriteLine(resp.token);
                    FrameSwitcher.Switch(typeof(Views.NavigationView));
                }
            }
            catch
            {
                ApiHandle<string>.ThrowException(responseContent);
            }
        }

        private async void BtnSignUp_Click(object sender, RoutedEventArgs e)
        {
            FrameSwitcher.Switch(typeof(Views.RegisterForm));
        }

        private void CheckBox_Change(object sender, RoutedEventArgs e)
        {
            isChecked = !isChecked;
        }
    }
}
