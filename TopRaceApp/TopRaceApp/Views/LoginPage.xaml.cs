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
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            this.BindingContext = new LoginPageViewModel();
            InitializeComponent();
        }
    }
}