﻿using System;
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
using Xamarin.CommunityToolkit;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views.Options;
using TopRaceApp.DTOs;
namespace TopRaceApp.ViewModels
{
    class MainPageViewModel:BaseViewModel
    {
        #region Properties
        private string userName;
        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                if (this.userName != value)
                {
                    userName = value;
                    OnPropertyChanged();
                }
            }
        }
        private string profilePic;
        public string ProfilePic
        {
            get
            {
                return profilePic;
            }
            set
            {
                if (this.profilePic != value)
                {
                    profilePic = value;
                    OnPropertyChanged();
                }
            }
        }
        private int winsCount;
        public int WinsCount
        {
            get
            {
                return winsCount;
            }
            set
            {
                if (this.winsCount != value)
                {
                    winsCount = value;
                    OnPropertyChanged();
                }
            }
        }
        private int losesCount;
        public int LosesCount
        {
            get
            {
                return losesCount;
            }
            set
            {
                if (this.losesCount != value)
                {
                    losesCount = value;
                    OnPropertyChanged();
                }
            }
        }
        private int winStreak;
        public int WinStreak
        {
            get
            {
                return winStreak;
            }
            set
            {
                if (this.winStreak != value)
                {
                    winStreak = value;
                    OnPropertyChanged();
                }
            }
        }
        private string status;
        public string Status
        {
            get
            {
                return status;
            }
            set
            {
                if(this.status != value)
                {
                    this.status = value;
                    OnPropertyChanged();
                }
            }
        }
        private string privateKey;
        public string PrivateKey
        {
            get
            {
                return privateKey;
            }
            set
            {
                if(this.privateKey != value)
                {
                    this.privateKey = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion
        public MainPageViewModel()
        {
            this.PrivateKey = string.Empty;
            this.UserName = ((App)App.Current).currentUser.UserName;
            this.ProfilePic = ((App)App.Current).currentUser.ProfileImageSource;
            this.WinsCount = ((App)App.Current).currentUser.WinsNumber;
            this.LosesCount = ((App)App.Current).currentUser.LosesNumber;
            this.WinStreak = ((App)App.Current).currentUser.WinsStreak;
            this.Status = $"Wins: {this.WinsCount} Loses: {this.LosesCount}\n Current Streak: {this.WinStreak}";
            HostGameCommand = new Command(HostGame);
            JoinGameWithPrivateKeyCommand = new Command(JoinGameWithPrivateKey);
            LogOutCommand = new Command(LogOut);
        }
        public ICommand HostGameCommand { get; set; }
        public async void HostGame()
        {
            TopRaceAPIProxy proxy = TopRaceAPIProxy.CreateProxy();
            try
            {
                GameDTO newGame = new GameDTO
                {
                    GameName = $"{((App)App.Current).currentUser.UserName}'s Game",
                    IsPrivate = true,
                    LastUpdateTime = DateTime.Now
                };
                GameDTO fullGame = await proxy.HostGameAsync(newGame);
                if (fullGame == null)
                    await App.Current.MainPage.DisplayAlert("Registeration Failed", "Something went wrong", "Okay");
                else
                {
                    ((App)App.Current).currentGame = fullGame;
                    ((App)App.Current).currentPlayerInGame = fullGame.PlayersInGames.Where(p => p.UserId == ((App)App.Current).currentUser.Id).FirstOrDefault();
                    MoveToLobbyPage();
                }
            }
            catch
            {
                await App.Current.MainPage.DisplayAlert("Registeration Failed", "Something went wrong", "Okay");
            }
        }
        public async void MoveToLobbyPage()
        {
            LobbyPage lobbyPage = new LobbyPage();
            lobbyPage.BindingContext = new LobbyPageViewModel();
            lobbyPage.SetEvent();
            await ((LobbyPageViewModel)(lobbyPage.BindingContext)).Run();
            await App.Current.MainPage.Navigation.PushAsync(lobbyPage);
        }
        public ICommand JoinGameWithPrivateKeyCommand { get; set; }
        public async void JoinGameWithPrivateKey()
        {
            TopRaceAPIProxy proxy = TopRaceAPIProxy.CreateProxy();
            try
            {
                privateKey = privateKey.ToUpper();
                GameDTO game = await proxy.JoinGameWithPrivateCodeAsync(PrivateKey);
                if (game != null)
                {
                    ((App)App.Current).currentGame = game;
                    ((App)App.Current).currentPlayerInGame = game.PlayersInGames.Where(p => p.UserId == ((App)App.Current).currentUser.Id).FirstOrDefault();
                    MoveToLobbyPage();
                }
                else
                {
                    if (Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.iOS)
                    {
                        var toastOptions = new ToastOptions
                        {
                            BackgroundColor = Xamarin.Forms.Color.Black,
                            MessageOptions = new MessageOptions
                            {
                                Message = "Could not find an active game with the inserted key",
                                Foreground = Xamarin.Forms.Color.White,
                            },
                            CornerRadius = 5,
                            Duration = System.TimeSpan.FromSeconds(3),
                        };
                        await ((App)App.Current).MainPage.DisplayToastAsync(toastOptions);
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Game could not be found", "Could not find an active game with the inserted key", "OK");
                    }

                }

            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("Registeration Failed", "Something went wrong", "Okay");
            }
        }
        public ICommand LogOutCommand { get; set; }
        private async void LogOut()
        {
            //StartPage startPage = new StartPage();
            //App.Current.MainPage = startPage;
            TopRaceAPIProxy proxy = TopRaceAPIProxy.CreateProxy();
            bool isLoggedOut = await proxy.LogOutAsync();
            if (isLoggedOut)
            {
                ((App)App.Current).currentUser = null;
                App.Current.MainPage.Navigation.PopToRootAsync();
            }
        }
    }
}
