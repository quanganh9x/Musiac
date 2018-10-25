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
using Newtonsoft.Json;

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
        private bool isImageValid = false;
        private bool isBirthdayValid = false;
        public RegisterForm()
        {
            this.currentMember = new Member();
            this.InitializeComponent();
        }

        private async void Handle_Signup(object sender, RoutedEventArgs e)
        {
            if (CheckRegister(this.Email.Text, this.Phone.Text))
            {
                Revert();
                this.currentMember.firstName = this.FirstName.Text;
                this.currentMember.lastName = this.LastName.Text;
                this.currentMember.email = this.Email.Text;
                this.currentMember.password = this.Password.Password;
                this.currentMember.avatar = this.ImageUrl.Text;
                this.currentMember.phone = this.Phone.Text;
                this.currentMember.address = this.Address.Text;
                this.currentMember.introduction = this.Introduction.Text;
                string responseContent = await ApiHandle<Member>.Call(APITypes.SignUp, this.currentMember);
                try
                {
                    Member resp = JsonConvert.DeserializeObject<Member>(responseContent);
                    if (resp.email != null && resp.email == this.currentMember.email)
                    {
                        await Dialog.Show("Register success!");
                        FrameSwitcher.Switch(typeof(Views.LoginForm));
                    }
                    else
                    {
                        Entity.Exception err = JsonConvert.DeserializeObject<Entity.Exception>(responseContent);
                        register.Text = err.message;
                        register.Visibility = Visibility.Visible;
                    }
                }
                catch
                {
                    ApiHandle<string>.ThrowException(responseContent);
                }
            }
        }

        private void Revert()
        {
            if (register.Visibility == Visibility.Visible) register.Visibility = Visibility.Collapsed;
            if (email.Visibility == Visibility.Visible) email.Visibility = Visibility.Collapsed;
            if (phone.Visibility == Visibility.Visible) phone.Visibility = Visibility.Collapsed;
        }
        private bool CheckRegister(string email, string phone)
        {
            bool isPassed = false;
            if (email.Contains("@") != true || email.Contains(".") != true)
            {
                this.email.Visibility = Visibility.Visible;
                isPassed = false;
            }
            else isPassed = true;
            if (phone.Length != 11 && phone.Length != 10)
            {
                isPassed = false;
                this.phone.Visibility = Visibility.Visible;
            } else isPassed = true;
            if (isImageValid && isBirthdayValid) isPassed = true;
            else isPassed = false;
            return isPassed;
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
            try
            {
                MyAvatar.Source = new BitmapImage(URL);
                isImageValid = true;
                avatar.Visibility = Visibility.Collapsed;
            } catch
            {
                ExceptionHandle.ThrowDebug("cant upload image");
                MyAvatar.Source = null;
                isImageValid = false;
                avatar.Visibility = Visibility.Visible;
            }
        }

        private void Select_Gender(object sender, RoutedEventArgs e)
        {
            RadioButton radioGender = sender as RadioButton;
            this.currentMember.gender = Int32.Parse(radioGender.Tag.ToString());
        }

        private void Change_Birthday(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            this.currentMember.birthday = sender.Date.Value.ToString("yyyy-MM-dd");
            if (2018 - int.Parse(sender.Date.Value.ToString("yyyy-MM-dd").Substring(0, 4)) < 5)
            {
                birthday.Visibility = Visibility.Visible;
                isBirthdayValid = false;
            }
            else
            {
                birthday.Visibility = Visibility.Collapsed;
                isBirthdayValid = true;
            }
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
                isImageValid = true;
                avatar.Visibility = Visibility.Collapsed;
            } catch (System.Exception)
            {
                ExceptionHandle.ThrowDebug("wrong image");
                MyAvatar.Source = null;
                isImageValid = false;
                avatar.Visibility = Visibility.Visible;
            }
        }
    }
}
