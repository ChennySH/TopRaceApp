using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopRaceApp.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TopRaceApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            ((App)App.Current).SetBackgrounds(this);
            
            this.BindingContext = new MainPageViewModel();
            InitializeComponent();
            if (Device.RuntimePlatform == Device.UWP)
            {
                ProfilePic.WidthRequest = 200;
                ProfilePic.HeightRequest = 200;
            }
            else
            {
                ProfilePic.WidthRequest = 150;
                ProfilePic.HeightRequest = 150;
            }
        }
    }
}