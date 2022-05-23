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
    class SignUpPageViewModel:BaseViewModel
    {
        public SignUpPageViewModel()
        {
            UserName = "";
            Email = "";
            Password = "";
            RepeatPassword = "";
            PhoneNumber = "";
            RegisterCommand = new Command(Register);
        }
        private async void Register()
        {
            TopRaceAPIProxy proxy = TopRaceAPIProxy.CreateProxy();
            try
            {
                bool allValuesVliadted = ValidationAllValues();
                if (!allValuesVliadted)
                {
                    await App.Current.MainPage.DisplayAlert("Registeration Failed", "Please check all your values are validated", "Okay");
                }
                else
                {

                    bool userNameExist = await proxy.IsUserNameExistAsync(UserName);
                    bool emailExist = await proxy.IsEmailExistAsync(Email);
                    if (userNameExist || emailExist)
                    {
                        string error = "";
                        if (userNameExist)
                        {
                            error += "Username ";
                            if (emailExist)
                            {
                                error += "and email are already in use";
                            }
                            else
                            {
                                error += "is already in use";
                            }
                        }
                        else
                            error += "Email is already in use";
                        await App.Current.MainPage.DisplayAlert("Registeration Failed", error, "Okay");
                    }
                    else
                    {
                        User newUser = new User
                        {
                            UserName = this.UserName,
                            Email = this.Email,
                            Password = this.Password,
                            PhoneNumber = this.PhoneNumber,
                            WinsNumber = 0,
                            LosesNumber = 0,
                            WinsStreak = 0,
                            ProfilePic = "",
                        };
                        bool registered = await proxy.SignUpAsync(newUser);
                        if (registered)
                        {
                            if (this.imageFileResult != null)
                            {
                                bool success = await proxy.UploadImage(new FileInfo()
                                {
                                    Name = this.imageFileResult.FullPath
                                }, $"{newUser.Email}.jpg");
                            }
                            Login(); 
                        }
                        else
                            await App.Current.MainPage.DisplayAlert("Registeration Failed", "Something went wrong", "Okay");

                    }
                }
            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert("Registeration Failed", "Something went wrong", "Okay");
            }
        }
        public void Login()
        {
            LoginPage loginPage = new LoginPage();
            App.Current.MainPage.Navigation.PushAsync(loginPage);
        }

        FileResult imageFileResult;
        public event Action<ImageSource> SetImageSourceEvent;
        public ICommand PickImageCommand => new Command(OnPickImage);
        public async void OnPickImage()
        {
            FileResult result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions()
            {
                Title = "Pick an Image"
            });
            if (result != null)
            {
                this.imageFileResult = result;

                var stream = await result.OpenReadAsync();
                ImageSource imgSource = ImageSource.FromStream(() => stream);
                if (SetImageSourceEvent != null)
                    SetImageSourceEvent(imgSource);
            }
        }
        ///The following command handle the take photo button
        public ICommand CameraImageCommand => new Command(OnCameraImage);
        public async void OnCameraImage()
        {
            if (MediaPicker.IsCaptureSupported)
            {
                var result = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions()
                {
                    Title = "Take an Image"
                });
                if (result != null)
                {
                    this.imageFileResult = result;
                    var stream = await result.OpenReadAsync();
                    ImageSource imgSource = ImageSource.FromStream(() => stream);
                    if (SetImageSourceEvent != null)
                        SetImageSourceEvent(imgSource);
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Can not take an image", "Your device canwt take images", "Okay");
            }
        }
        public ICommand SetDefaultImageCommand => new Command(SetDefualtImage);

        public void SetDefualtImage(object obj)
        {
            TopRaceAPIProxy proxy = TopRaceAPIProxy.CreateProxy();
            this.imageFileResult = null;
            if (SetImageSourceEvent != null)
                SetImageSourceEvent(proxy.GetBasePhotoUri()+"DefaultProfilePic.jpg");
        }
        #region properties
        #region UserName
        private string userName;
        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                if (userName != value)
                {
                    userName = value;
                    OnPropertyChanged();
                    UserNameValidation();
                }
            }
        }
        private string userNameErrorMessege;
        public string UserNameErrorMessege
        {
            get
            {
                return userNameErrorMessege;
            }
            set
            {
                if (userNameErrorMessege != value)
                {
                    userNameErrorMessege = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool userNameErrorMessegeIsVisible;
        public bool UserNameErrorMessegeIsVisible
        {
            get
            {
                return userNameErrorMessegeIsVisible;
            }
            set
            {
                if (userNameErrorMessegeIsVisible != value)
                {
                    userNameErrorMessegeIsVisible = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion
        #region FirstName
        //private string firstName;
        //public string FirstName
        //{
        //    get
        //    {
        //        return firstName;
        //    }
        //    set
        //    {
        //        if (firstName != value)
        //        {
        //            firstName = value;
        //            OnPropertyChanged();
        //            FirstNameValidation();
        //        }
        //    }
        //}
        //private string firstNameErrorMessege;
        //public string FirstNameErrorMessege
        //{
        //    get
        //    {
        //        return firstNameErrorMessege;
        //    }
        //    set
        //    {
        //        if (firstNameErrorMessege != value)
        //        {
        //            firstNameErrorMessege = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}
        //private bool firstNameErrorMessegeIsVisible;
        //public bool FirstNameErrorMessegeIsVisible
        //{
        //    get
        //    {
        //        return firstNameErrorMessegeIsVisible;
        //    }
        //    set
        //    {
        //        if (firstNameErrorMessegeIsVisible != value)
        //        {
        //            firstNameErrorMessegeIsVisible = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}
        #endregion
        #region LastName
        //private string lastName;
        //public string LastName
        //{
        //    get
        //    {
        //        return lastName;
        //    }
        //    set
        //    {
        //        if (lastName != value)
        //        {
        //            lastName = value;
        //            OnPropertyChanged();
        //            LastNameValidation();
        //        }
        //    }
        //}
        //private string lastNameErrorMessege;
        //public string LastNameErrorMessege
        //{
        //    get
        //    {
        //        return lastNameErrorMessege;
        //    }
        //    set
        //    {
        //        if (lastNameErrorMessege != value)
        //        {
        //            lastNameErrorMessege = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}
        //private bool lastNameErrorMessegeIsVisible;
        //public bool LastNameErrorMessegeIsVisible
        //{
        //    get
        //    {
        //        return lastNameErrorMessegeIsVisible;
        //    }
        //    set
        //    {
        //        if (lastNameErrorMessegeIsVisible != value)
        //        {
        //            lastNameErrorMessegeIsVisible = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}
        #endregion
        #region Email
        private string email;
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                if (email != value)
                {
                    email = value;
                    EmailValidation();
                    OnPropertyChanged();
                }
            }
        }
        private string emailErrorMessege;
        public string EmailErrorMessege
        {
            get
            {
                return emailErrorMessege;
            }
            set
            {
                if (emailErrorMessege != value)
                {
                    emailErrorMessege = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool emailErrorMessegeIsVisible;
        public bool EmailErrorMessegeIsVisible
        {
            get
            {
                return emailErrorMessegeIsVisible;
            }
            set
            {
                if (emailErrorMessegeIsVisible != value)
                {
                    emailErrorMessegeIsVisible = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion
        #region Password
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
                    RepeatPasswordValidation();
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
        #region RepeatPassword
        private string repeatPassword;
        public string RepeatPassword
        {
            get
            {
                return repeatPassword;
            }
            set
            {
                if (repeatPassword != value)
                {
                    repeatPassword = value;
                    OnPropertyChanged();
                    RepeatPasswordValidation();
                }
            }
        }
        private string repeatPasswordErrorMessege;
        public string RepeatPasswordErrorMessege
        {
            get
            {
                return repeatPasswordErrorMessege;
            }
            set
            {
                if (repeatPasswordErrorMessege != value)
                {
                    repeatPasswordErrorMessege = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool repeatPasswordErrorMessegeIsVisible;
        public bool RepeatPasswordErrorMessegeIsVisible
        {
            get
            {
                return repeatPasswordErrorMessegeIsVisible;
            }
            set
            {
                if (repeatPasswordErrorMessegeIsVisible != value)
                {
                    repeatPasswordErrorMessegeIsVisible = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion
        #region PhoneNumber
        private string phoneNumber;
        public string PhoneNumber
        {
            get
            {
                return phoneNumber;
            }
            set
            {
                if (phoneNumber != value)
                {
                    phoneNumber = value;
                    OnPropertyChanged();
                    PhoneNumberValidation();
                }
            }
        }
        private string phoneNumberErrorMessege;
        public string PhoneNumberErrorMessege
        {
            get
            {
                return phoneNumberErrorMessege;
            }
            set
            {
                if (phoneNumberErrorMessege != value)
                {
                    phoneNumberErrorMessege = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool phoneNumberErrorMessegeIsVisible;
        public bool PhoneNumberErrorMessegeIsVisible
        {
            get
            {
                return phoneNumberErrorMessegeIsVisible;
            }
            set
            {
                if (phoneNumberErrorMessegeIsVisible != value)
                {
                    phoneNumberErrorMessegeIsVisible = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion
        #region ProfilePicture 
        private string profilePic;
        public string ProfilePic
        {
            get
            {
                return profilePic;
            }
            set
            {
                if(profilePic != value)
                {
                    profilePic = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion
        #endregion
        #region ValdationMethods
        public bool ValidationAllValues()
        {
            UserNameValidation();
            //FirstNameValidation();
       //     LastNameValidation();
            EmailValidation();
            PhoneNumberValidation();
            PasswordValidation();
            return (!(UserNameErrorMessegeIsVisible || EmailErrorMessegeIsVisible ||
                PhoneNumberErrorMessegeIsVisible || PasswordErrorMessegeIsVisible || RepeatPasswordErrorMessegeIsVisible));
        }
        public void UserNameValidation()
        {
            if (string.IsNullOrEmpty(UserName) || UserName.Length < 2)
            {
                UserNameErrorMessege = "UserName must have at least 2 letters";
                UserNameErrorMessegeIsVisible = true;
            }
            else
            {
                UserNameErrorMessege = "";
                UserNameErrorMessegeIsVisible = false;
            }
        }
        //public void FirstNameValidation()
        //{
        //    if (string.IsNullOrEmpty(FirstName) || FirstName.Length < 2)
        //    {
        //        FirstNameErrorMessege = "Please enter a valid name";
        //        FirstNameErrorMessegeIsVisible = true;
        //    }
        //    else
        //    {
        //        FirstNameErrorMessege = "";
        //        FirstNameErrorMessegeIsVisible = false;
        //    }
        //}
        //public void LastNameValidation()
        //{
        //    if (string.IsNullOrEmpty(LastName) || LastName.Length < 2)
        //    {
        //        LastNameErrorMessege = "Please enter a valid name";
        //        LastNameErrorMessegeIsVisible = true;
        //    }
        //    else
        //    {
        //        LastNameErrorMessege = "";
        //        LastNameErrorMessegeIsVisible = false;
        //    }
        //}
        public void PhoneNumberValidation()
        {
            if (string.IsNullOrEmpty(PhoneNumber))
            {
                PhoneNumberErrorMessege = "Please enter your phone number";
                PhoneNumberErrorMessegeIsVisible = true;
                return;
            }
            for (int i = 2; i < PhoneNumber.Length; i++)
            {
                char c = PhoneNumber[i];
                if (c < '0' || c > '9')
                {
                    PhoneNumberErrorMessege = "Phone number must contain valid digits only";
                    PhoneNumberErrorMessegeIsVisible = true;
                    return;
                }
            }
            if (!PhoneNumber.StartsWith("05"))
            {
                PhoneNumberErrorMessege = "Phone number must start with the digits 05";
                PhoneNumberErrorMessegeIsVisible = true;
                return;
            }
            if (PhoneNumber.Length != 10)
            {
                PhoneNumberErrorMessege = "Phone number must contain 10 digits";
                PhoneNumberErrorMessegeIsVisible = true;
                return;
            }

            PhoneNumberErrorMessege = "";
            PhoneNumberErrorMessegeIsVisible = false;
        }
        public void EmailValidation()
        {
            if (string.IsNullOrEmpty(Email))
            {
                EmailErrorMessege = "Please enter your email";
                EmailErrorMessegeIsVisible = true;
                return;
            }
            if (!(Email.Contains("@") && !Email.StartsWith("@") && !Email.EndsWith("@")))
            {
                EmailErrorMessege = "Please enter a valid email";
                EmailErrorMessegeIsVisible = true;
                return;
            }
            EmailErrorMessege = "";
            EmailErrorMessegeIsVisible = false;
        }
        public void PasswordValidation()
        {
            if (string.IsNullOrEmpty(Password) || Password.Length < 8)
            {
                PasswordErrorMessege = "The Password must have at least 8 letters";
                PasswordErrorMessegeIsVisible = true;
            }
            else
            {
                PasswordErrorMessege = "";
                PasswordErrorMessegeIsVisible = false;
            }
        }
        public void RepeatPasswordValidation()
        {
            if (RepeatPassword != Password)
            {
                RepeatPasswordErrorMessegeIsVisible = true;
                RepeatPasswordErrorMessege = "The repeated password don't match to the password";
            }
            else
            {
                RepeatPasswordErrorMessegeIsVisible = false;
                RepeatPasswordErrorMessege = "";
            }
        }
        #endregion
        public ICommand RegisterCommand { get; set; }
    }
}
