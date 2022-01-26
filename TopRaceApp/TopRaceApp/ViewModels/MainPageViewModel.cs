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

        #endregion
        public MainPageViewModel()
        {
            this.UserName = ((App)App.Current).currentUser.UserName;
            this.ProfilePic = ((App)App.Current).currentUser.ProfilePic;
            this.WinsCount = ((App)App.Current).currentUser.WinsNumber;
            this.LosesCount = ((App)App.Current).currentUser.LosesNumber;
            this.WinStreak = ((App)App.Current).currentUser.WinsStreak;
            this.Status = $"Wins: {this.WinsCount} Loses: {this.LosesCount}\n Current Streak: {this.WinStreak}";
            HostGameCommand = new Command(HostGame);
        }
        public ICommand HostGameCommand { get; set; }
        public async void HostGame()
        {
            TopRaceAPIProxy proxy = TopRaceAPIProxy.CreateProxy();
            try
            {
                Game newGame = new Game
                {
                    GameName = $"{((App)App.Current).currentUser.UserName}'s Game",
                    IsPrivate = true,
                    LastUpdateTime = DateTime.Now
                };
                newGame.HostUser = ((App)App.Current).currentUser;
                Game fullGame = await proxy.HostGameAsync(newGame);
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
            await ((LobbyPageViewModel)(lobbyPage.BindingContext)).Run();
            App.Current.MainPage = lobbyPage;
        }

    }
}
