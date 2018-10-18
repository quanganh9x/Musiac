using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using Windows.Foundation;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using T1708E_UWP.Entity;
using T1708E_UWP.Service;
using System.Net;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace T1708E_UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegisterForm : Page
    {
        private Member currentMember;
        private static string UploadUrl;
        public RegisterForm()
        {
            this.currentMember = new Member();
            this.InitializeComponent();
        }

        private async void Handle_Signup(object sender, RoutedEventArgs e)
        {
            
            this.currentMember.firstName = this.FirstName.Text;
            this.currentMember.lastName = this.LastName.Text;
            this.currentMember.email = this.Email.Text;
            this.currentMember.password = this.Password.Password.ToString();
            this.currentMember.avatar = this.ImageUrl.Text;
            this.currentMember.phone = this.Phone.Text;
            this.currentMember.address = this.Address.Text;
            this.currentMember.introduction = this.Introduction.Text;
            await ApiHandle<Member>.Call(APITypes.SignUp, this.currentMember);
        }

        private async void Capture_Photo(object sender, RoutedEventArgs e)
        {
            StorageFile file = null;
            CameraCaptureUI captureUI = new CameraCaptureUI();
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Png;
            captureUI.PhotoSettings.CroppedSizeInPixels = new Size(200, 200);
            file = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);
            if (file == null)
            {
                return;
            }
            Uri URL = await ApiHandle<string>.Upload(file, await ApiHandle<string>.Call(APITypes.GetUpload, null), "quanganh9x", "image/png");
            ImageUrl.Text = URL.AbsoluteUri;
            MyAvatar.Source = new BitmapImage(URL);
        }

        private void Select_Gender(object sender, RoutedEventArgs e)
        {
            RadioButton radioGender = sender as RadioButton;
            this.currentMember.gender = Int32.Parse(radioGender.Tag.ToString());
        }

        private void Change_Birthday(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            this.currentMember.birthday = sender.Date.Value.ToString("yyyy-MM-dd");
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            FrameSwitcher.Switch(typeof(Views.RegisterForm));
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            FrameSwitcher.Switch(typeof(Views.LoginForm));
        }

        private void ImageUrl_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                MyAvatar.Source = new BitmapImage(new Uri(ImageUrl.Text));
            } catch (System.Exception)
            {
                MyAvatar.Source = null;
            }
        }
    }
}
