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
    class LoginPageViewModel:BaseViewModel
    {
        public LoginPageViewModel()
        {
            UserNameOrEmail = "";
            Password = "";
            SubmitCommand = new Command(Submit);
        }

        private async void Submit()
        {
            TopRaceAPIProxy proxy = TopRaceAPIProxy.CreateProxy();
            try
            {
                bool allValuesVliadted = ValidationAllValues();
                if (!allValuesVliadted)
                {
                    await App.Current.MainPage.DisplayAlert("Login Failed", "Please check all your values are validated", "Okay");
                }
                else
                {
                    User u = await proxy.LoginAsync(UserNameOrEmail, Password);
                    if (u != null)
                    {
                        ((App)App.Current).currentUser = u;
                        ((App)App.Current).currentPlayer = u.Player;
                        MoveToHomePage();
                    }
                    else
                        await App.Current.MainPage.DisplayAlert("Login Failed", "Something went wrong", "Okay");
                }
            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert("Login Failed", "Something went wrong", "Okay");
            }
        }
        public async void MoveToHomePage()
        {
            MainPage mainPage = new MainPage();
            await App.Current.MainPage.Navigation.PushAsync(mainPage);
        }
        #region Properties

        private string userNameOrEmail;
        public string UserNameOrEmail
        {
            get
            {
                return userNameOrEmail;
            }
            set
            {
                if (userNameOrEmail != value)
                {
                    userNameOrEmail = value;
                    OnPropertyChanged();
                    UsernameOrEmailValidation();
                }
            }
        }
        private string userNameOrEmailErrorMessege;
        public string UserNameOrEmailErrorMessege
        {
            get
            {
                return userNameOrEmailErrorMessege;
            }
            set
            {
                if (userNameOrEmailErrorMessege != value)
                {
                    userNameOrEmailErrorMessege = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool userNameOrEmailErrorMessegeIsVisible;
        public bool UserNameOrEmailErrorMessegeIsVisible
        {
            get
            {
                return userNameOrEmailErrorMessegeIsVisible;
            }
            set
            {
                if (userNameOrEmailErrorMessegeIsVisible != value)
                {
                    userNameOrEmailErrorMessegeIsVisible = value;
                    OnPropertyChanged();
                }
            }
        }
        private string password;
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                if (password != value)
                {
                    password = value;
                    OnPropertyChanged();
                    PasswordValidation();
                }
            }
        }
        private string passwordErrorMessege;
        public string PasswordErrorMessege
        {
            get
            {
                return passwordErrorMessege;
            }
            set
            {
                if (passwordErrorMessege != value)
                {
                    passwordErrorMessege = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool passwordErrorMessegeIsVisible;
        public bool PasswordErrorMessegeIsVisible
        {
            get
            {
                return passwordErrorMessegeIsVisible;
            }
            set
            {
                if (passwordErrorMessegeIsVisible != value)
                {
                    passwordErrorMessegeIsVisible = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion
        #region ValidationMethods
        public bool ValidationAllValues()
        {
            UsernameOrEmailValidation();
            PasswordValidation();
            return (!(UserNameOrEmailErrorMessegeIsVisible || PasswordErrorMessegeIsVisible));
        }
        public void UsernameOrEmailValidation()
        {
            if (string.IsNullOrEmpty(UserNameOrEmail))
            {
                UserNameOrEmailErrorMessege = "Please enter your username or email";
                UserNameOrEmailErrorMessegeIsVisible = true;
            }
            else
            {
                UserNameOrEmailErrorMessege = "";
                UserNameOrEmailErrorMessegeIsVisible = false;
            }
        }
        public void PasswordValidation()
        {
            if (string.IsNullOrEmpty(Password) || Password.Length < 8)
            {
                PasswordErrorMessege = "Password must have at least 8 letters";
                PasswordErrorMessegeIsVisible = true;
            }
            else
            {
                PasswordErrorMessege = "";
                PasswordErrorMessegeIsVisible = false;
            }
        }
        #endregion
        public ICommand SubmitCommand { get; set; }

    }
}
