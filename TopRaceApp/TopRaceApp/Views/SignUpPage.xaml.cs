using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopRaceApp.ViewModels;
using TopRaceApp.Services;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TopRaceApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            ((App)App.Current).SetBackgrounds(this);
            this.BindingContext = new SignUpPageViewModel();
            ((SignUpPageViewModel)this.BindingContext).SetImageSourceEvent += OnSetImageSource;
            TopRaceAPIProxy proxy = TopRaceAPIProxy.CreateProxy();
            InitializeComponent();
            ProfileImage.Source = proxy.GetBasePhotoUri() + "DefaultProfilePic.png";

        }
        public void OnSetImageSource(ImageSource imgSource)
        {
            ProfileImage.Source = imgSource;
        }
    }
}