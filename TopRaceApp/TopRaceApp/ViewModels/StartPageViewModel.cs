using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using TopRaceApp.Services;
using TopRaceApp.Models;
using TopRaceApp.Views;
using Xamarin.Essentials;
using System.Linq;


namespace TopRaceApp.ViewModels
{
    class StartPageViewModel:BaseViewModel
    {
        public StartPageViewModel()
        {
            LoginCommand = new Command(Login);
            SignUpCommand = new Command(SignUp);
        }
        public ICommand LoginCommand { get; set; }
        public ICommand SignUpCommand { get; set; }
        public void Login()
        {
            TopRaceAPIProxy proxy = TopRaceAPIProxy.CreateProxy();
            try
            {
                LoginPage loginPage = new LoginPage();
                App.Current.MainPage.Navigation.PushAsync(loginPage);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void SignUp()
        {
            TopRaceAPIProxy proxy = TopRaceAPIProxy.CreateProxy();
            try
            {
                SignUpPage signUp = new SignUpPage();
                App.Current.MainPage.Navigation.PushAsync(signUp);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
