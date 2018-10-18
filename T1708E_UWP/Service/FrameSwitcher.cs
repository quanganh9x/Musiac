using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace T1708E_UWP.Service
{
    enum Frames
    {
        SplitView, LoginForm, SignupForm
    }

    public static class FrameSwitcher
    {
        public static void Switch(Type dest)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(dest);
            GC.Collect(); // garbage collector: to clear remaining elements of last frame
        }
        /*
        public static Frame Get()
        {
            return Window.Current.Content as Frame;
        }
        */
    }
}
