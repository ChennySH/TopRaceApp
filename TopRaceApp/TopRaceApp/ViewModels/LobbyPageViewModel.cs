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
using System.Collections.ObjectModel;
using Xamarin.CommunityToolkit;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views.Options;
using System.Collections.Concurrent;
using TopRaceApp.DTOs;

namespace TopRaceApp.ViewModels
{
    class LobbyPageViewModel:BaseViewModel
    {
        #region properties 
        private bool isGameActive;
        public bool IsGameActive
        {
            get
            {
                return isGameActive;
            }
            set
            {
                if(isGameActive != value)
                {
                    isGameActive = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool isGameOn;
        public bool IsGameOn
        {
            get
            {
                return isGameOn;
            }
            set
            {
                if (isGameOn != value)
                {
                    isGameOn = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool isInGamePage;
        public bool IsInGamePage
        {
            get
            {
                return isInGamePage;
            }
            set
            {
                if (isInGamePage != value)
                {
                    isInGamePage = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool areYouInGame;
        public bool AreYouInGame
        {
            get
            {
                return areYouInGame;
            }
            set
            {
                if (areYouInGame != value)
                {
                    areYouInGame = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool didLeaveTheGame;
        public bool DidLeaveTheGame
        {
            get
            {
                return didLeaveTheGame;
            }
            set
            {
                if(didLeaveTheGame != value)
                {
                    didLeaveTheGame = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool isHost;
        public bool IsHost
        {
            get
            {
                return isHost;
            }
            set
            {
                if(isHost != value)
                {
                    isHost = value;
                    OnPropertyChanged();
                    IsNotHost = !isHost;
                }
            }
        }
        private bool isNotHost;
        public bool IsNotHost
        {
            get
            {
                return isNotHost;
            }
            set
            {
                if (isNotHost != value)
                {
                    isNotHost = value;
                    OnPropertyChanged();
                }
            }
        }
        private string gameName;
        public string GameName
        {
            get
            {
                return gameName;
            }
            set
            {
                if(gameName != value)
                {
                    gameName = value;
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
                if (privateKey != value)
                {
                    privateKey = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool isPrivate;
        public bool IsPrivate
        {
            get
            {
                return isPrivate;
            }
            set
            {
                if (isPrivate != value)
                {
                    isPrivate = value;
                    OnPropertyChanged();
                    if (isPrivate)
                    {
                        RoomStatus = "Private";
                    }
                    else
                    { 
                        RoomStatus = "Public"; 
                    }
                        
                }
            }
        }
        private string roomStatus;
        public string RoomStatus
        {
            get
            {
                return roomStatus;
            }
            set
            {
                if (roomStatus != value)
                {
                    roomStatus = value;
                    OnPropertyChanged();
                }
            }
        }
        private string hostName;
        public string HostName
        {
            get
            {
                return hostName;
            }
            set
            {
                if (hostName != value)
                {
                    hostName = value;
                    OnPropertyChanged();
                }
            }
        }
        private string hostProfilePic;
        public string HostProfilePic
        {
            get
            {
                return hostProfilePic;
            }
            set
            {
                if (hostProfilePic != value)
                {
                    hostProfilePic = value;
                    OnPropertyChanged();
                }
            }
        }
        private string messageText;
        public string MessageText
        {
            get
            {
                return messageText;
            }
            set
            {
                if (messageText != value)
                {
                    messageText = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public ObservableCollection<PlayersInGame> PlayersInGameList { get; set; }
        public ObservableCollection<Message> ChatMessages { get; set; }
        public ObservableCollection<Models.Color> ColorsCollection { get; set; }
        #endregion
        public LobbyPageViewModel()
        {
            IsInGamePage = false;
            IsGameActive = ((App)App.Current).currentGame.StatusId == 1;
            AreYouInGame = ((App)App.Current).currentPlayerInGame.IsInGame;
            DidLeaveTheGame = false;
            RoomStatus = string.Empty;
            IsHost = (((App)App.Current).currentPlayerInGame.IsHost);
            IsNotHost = !(((App)App.Current).currentPlayerInGame.IsHost);
            GameName = ((App)App.Current).currentGame.GameName;
            PrivateKey = ((App)App.Current).currentGame.PrivateKey;
            IsPrivate = ((App)App.Current).currentGame.IsPrivate;
            MessageText = "";
            //SelectedColor = ((App)App.Current).currentPlayerInGame.Color;
            //SelectedColor = null;
            PlayersInGameList = new ObservableCollection<PlayersInGame>();
            foreach (PlayersInGame p in ((App)App.Current).currentGame.PlayersInGames)
            {
                TopRaceAPIProxy proxy = TopRaceAPIProxy.CreateProxy();
                if (p.IsInGame)
                {
                    p.Color.PicLink = proxy.GetBasePhotoUri() + p.Color.PicLink;
                    PlayersInGameList.Add(p);
                }
            }
            
            ChatMessages = new ObservableCollection<Message>();
            foreach(Message m in ((App)App.Current).currentGame.Messages)
            {
                ChatMessages.Add(m);
            }
            SendMessageCommand = new Command(SendMessage);
            this.ColorsCollection = new ObservableCollection<Models.Color>();
            foreach(Models.Color color in ((App)Application.Current).GameColors)
            {
                this.ColorsCollection.Add(color);
            }
            OpenColorChangeViewCommand = new Command(OpenColorChangeView);
            CloseColorChangeViewCommand = new Command(CloseColorChangeView);
            ChangeColorCommand = new Command<Models.Color>(ChangeColor);
            CloseGameCommand = new Command(CloseGame);
            KickOutPlayerCommand = new Command<PlayersInGame>(KickOutPlayer);
            LeaveGameCommand = new Command(LeaveGame);
            StartGameCommand = new Command(StartGame);
        }
        public ICommand SendMessageCommand { get; set; }
        public ICommand OpenColorChangeViewCommand { get; set; }
        public ICommand CloseColorChangeViewCommand { get; set; }
        public ICommand ChangeColorCommand{ get; set; }
        public ICommand CloseGameCommand { get; set; }
        public ICommand KickOutPlayerCommand { get; set; }
        public ICommand LeaveGameCommand { get; set; }
        public ICommand StartGameCommand { get; set; }
        private async void SendMessage()
        {
            if (messageText != string.Empty)
            {
                TopRaceAPIProxy proxy = TopRaceAPIProxy.CreateProxy();
                Message newMessage = new Message
                {
                    Message1 = this.MessageText,
                    From = ((App)App.Current).currentPlayerInGame,
                    GameId = ((App)App.Current).currentGame.Id,
                    TimeSent = DateTime.Now
                };
                bool success = await proxy.SendMessageAsync(newMessage);
                if (success)
                    this.MessageText = string.Empty;
            }        
        }
        public void CloseColorChangeView()
        {
            App.Current.MainPage.Navigation.PopModalAsync();
        }
        public void OpenColorChangeView()
        {
            ChangeColorPopUp changeColorPage = new ChangeColorPopUp();
            changeColorPage.BindingContext = this;
            App.Current.MainPage.Navigation.PushModalAsync(changeColorPage);
        }
        public async void CloseGame()
        {
            if (!IsHost)
                return;
            TopRaceAPIProxy proxy = TopRaceAPIProxy.CreateProxy();
            bool isClosed = await proxy.CloseGameAsync(((App)App.Current).currentGame.Id);
            if (isClosed)
            {
                this.IsGameActive = false;
                this.AreYouInGame = false;
                ((App)App.Current).currentGame = null;
                ((App)App.Current).currentPlayerInGame = null;
                await ((App)App.Current).MainPage.Navigation.PopAsync();
                var closedToastOptions = new ToastOptions
                {
                    BackgroundColor = Xamarin.Forms.Color.Black,
                    MessageOptions = new MessageOptions
                    {
                        Message = "The Game Was Closed Succesfully!",
                        Foreground = Xamarin.Forms.Color.White,
                    },
                    CornerRadius = 5,
                    Duration = System.TimeSpan.FromSeconds(3),
                };
                try
                {
                    await ((App)App.Current).MainPage.DisplayToastAsync(closedToastOptions);
                }
                catch (Exception e)
                {

                }
            }
            else
            {
                var notClosedToastOptions = new ToastOptions
                {
                    BackgroundColor = Xamarin.Forms.Color.Black,
                    MessageOptions = new MessageOptions
                    {
                        Message = "Something went wrong",
                        Foreground = Xamarin.Forms.Color.White,
                    },
                    CornerRadius = 5,
                    Duration = System.TimeSpan.FromSeconds(3),
                };
                await ((App)App.Current).MainPage.DisplayToastAsync(notClosedToastOptions);
            }
        }
        public async void ChangeColor(Models.Color color)
        {
            TopRaceAPIProxy proxy = TopRaceAPIProxy.CreateProxy();
            ((App)App.Current).currentPlayerInGame.ColorId = color.Id;
            bool isUpdated = await proxy.UpdatePlayerAsync(((App)App.Current).currentPlayerInGame);
            ((App)App.Current).currentPlayerInGame.Color = color;
            PlayersInGame pl = this.PlayersInGameList.Where(p => p.Id == ((App)App.Current).currentPlayerInGame.Id).FirstOrDefault();
            int index = this.PlayersInGameList.IndexOf(pl);
            this.PlayersInGameList.Remove(pl);
            this.PlayersInGameList.Insert(index, ((App)App.Current).currentPlayerInGame);
            CloseColorChangeView();
            if (!isUpdated)
            {
                var toastOptions = new ToastOptions
                {
                    BackgroundColor = Xamarin.Forms.Color.Black,
                    MessageOptions = new MessageOptions
                    {
                        Message = "The Color is Already Taken",
                        Foreground = Xamarin.Forms.Color.White,
                    },
                    CornerRadius = 5,
                    Duration = System.TimeSpan.FromSeconds(3),
                };
                try
                {
                    await ((App)App.Current).MainPage.DisplayToastAsync(toastOptions);
                }
                catch(Exception e)
                {

                }
            }
        }
        public async void KickOutPlayer(PlayersInGame playerInGame)
        {
            try
            {
                if (IsHost)
                {
                    if (playerInGame.IsHost)
                    {
                        var toastOptions = new ToastOptions
                        {
                            BackgroundColor = Xamarin.Forms.Color.Black,
                            MessageOptions = new MessageOptions
                            {
                                Message = "You can not kick out yourself",
                                Foreground = Xamarin.Forms.Color.White,
                            },
                            CornerRadius = 5,
                            Duration = System.TimeSpan.FromSeconds(3),
                        };
                        try
                        { 
                            await ((App)App.Current).MainPage.DisplayToastAsync(toastOptions); 
                        }
                        catch(Exception e) { }
                        return;
                    }
                    TopRaceAPIProxy proxy = TopRaceAPIProxy.CreateProxy();
                    bool isKicked = await proxy.KickOutAsync(((App)App.Current).currentGame.Id, playerInGame.Id);
                    if (isKicked)
                    {
                        var toastOptions = new ToastOptions
                        {
                            BackgroundColor = Xamarin.Forms.Color.Black,
                            MessageOptions = new MessageOptions
                            {
                                Message = "The player was kicked out succesfully",
                                Foreground = Xamarin.Forms.Color.White,
                            },
                            CornerRadius = 5,
                            Duration = System.TimeSpan.FromSeconds(3),
                        };
                        try
                        {
                            await ((App)App.Current).MainPage.DisplayToastAsync(toastOptions);
                        }
                        catch(Exception e) { }
                    }
                    else
                    {
                        var toastOptions = new ToastOptions
                        {
                            BackgroundColor = Xamarin.Forms.Color.Black,
                            MessageOptions = new MessageOptions
                            {
                                Message = "Something went wrong",
                                Foreground = Xamarin.Forms.Color.White,
                            },
                            CornerRadius = 5,
                            Duration = System.TimeSpan.FromSeconds(3),
                        };
                        try
                        { await ((App)App.Current).MainPage.DisplayToastAsync(toastOptions); }
                        catch(Exception e) { }
                    }
                }
            }
            catch(Exception e)
            {
                var toastOptions = new ToastOptions
                {
                    BackgroundColor = Xamarin.Forms.Color.Black,
                    MessageOptions = new MessageOptions
                    {
                        Message = e.Message,
                        Foreground = Xamarin.Forms.Color.White,
                    },
                    CornerRadius = 5,
                    Duration = System.TimeSpan.FromSeconds(3),
                };
                await ((App)App.Current).MainPage.DisplayToastAsync(toastOptions);
            }
        }
        public async void MoveToGamePage()
        {
            GamePage gamePage = new GamePage();
            gamePage.BindingContext = new GamePageViewModel();
            gamePage.SetBorder();
            await gamePage.SetBitMaps();
            ((GamePageViewModel)gamePage.BindingContext).GamePage = gamePage;
            await ((GamePageViewModel)gamePage.BindingContext).Run();
            await ((App)App.Current).MainPage.Navigation.PushAsync(gamePage);
        }
        public async void StartGame()
        {
            try
            {
                if (isHost)
                {
                    IsInGamePage = true;
                    IsGameOn = true;
                    TopRaceAPIProxy proxy = TopRaceAPIProxy.CreateProxy();
                    GameDTO game = await proxy.StartGameAsync(((App)App.Current).currentGame.Id);
                    ((App)App.Current).currentGame = game;
                    ((App)App.Current).currentPlayerInGame = game.PlayersInGames.Where(p => p.Id == ((App)App.Current).currentPlayerInGame.Id).FirstOrDefault();
                    MoveToGamePage();
                }
            }
            catch(Exception e)
            {
                var toastOptions = new ToastOptions
                {
                    BackgroundColor = Xamarin.Forms.Color.Black,
                    MessageOptions = new MessageOptions
                    {
                        Message = e.Message,
                        Foreground = Xamarin.Forms.Color.White,
                    },
                    CornerRadius = 5,
                    Duration = System.TimeSpan.FromSeconds(3),
                };
                await ((App)App.Current).MainPage.DisplayToastAsync(toastOptions);
            }
        }
        public async void LeaveGame()
        {
            try
            {
                if (!IsHost)
                {
                    TopRaceAPIProxy proxy = TopRaceAPIProxy.CreateProxy();
                    bool isKicked = await proxy.KickOutAsync(((App)App.Current).currentGame.Id, ((App)App.Current).currentPlayerInGame.Id);
                    if (isKicked)
                    {
                        DidLeaveTheGame = true;
                        ((App)App.Current).currentGame = null;
                        ((App)App.Current).currentPlayerInGame = null;
                        await ((App)App.Current).MainPage.Navigation.PopAsync();
                    }
                    else
                    {
                        var toastOptions = new ToastOptions
                        {
                            BackgroundColor = Xamarin.Forms.Color.Black,
                            MessageOptions = new MessageOptions
                            {
                                Message = "Something went wrong",
                                Foreground = Xamarin.Forms.Color.White,
                            },
                            CornerRadius = 5,
                            Duration = System.TimeSpan.FromSeconds(3),
                        };
                        await ((App)App.Current).MainPage.DisplayToastAsync(toastOptions);
                    }
                }
            }
            catch (Exception e)
            {
                var toastOptions = new ToastOptions
                {
                    BackgroundColor = Xamarin.Forms.Color.Black,
                    MessageOptions = new MessageOptions
                    {
                        Message = e.Message,
                        Foreground = Xamarin.Forms.Color.White,
                    },
                    CornerRadius = 5,
                    Duration = System.TimeSpan.FromSeconds(3),
                };
                await ((App)App.Current).MainPage.DisplayToastAsync(toastOptions);
            }
        }
        public async Task Run()
        {
            TopRaceAPIProxy proxy = TopRaceAPIProxy.CreateProxy();
            Device.StartTimer(new TimeSpan(0, 0, 3), () =>
            {

                // do something every 3 seconds
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (IsGameActive && AreYouInGame && !DidLeaveTheGame)
                    {
                        ((App)App.Current).currentGame = await proxy.GetGameAsync(((App)App.Current).currentGame.Id);
                        IsGameActive = ((App)App.Current).currentGame.StatusId != 3;
                        IsGameOn = ((App)App.Current).currentGame.StatusId == 2;
                        ((App)App.Current).currentPlayerInGame = ((App)App.Current).currentGame.PlayersInGames.Where(p => p.UserId == ((App)App.Current).currentUser.Id).FirstOrDefault();
                        AreYouInGame = ((App)App.Current).currentPlayerInGame.IsInGame;
                        if (IsGameActive && AreYouInGame)
                        {
                            UpdateChatRoom();
                            UpdatePlayersInGameList();
                            AddNewPlayers();
                            PlayersInGameList.OrderBy(p => p.Id);
                        }
                        if (IsGameOn && (IsInGamePage == false))
                        {
                            IsInGamePage = true;
                            MoveToGamePage();
                        }
                        if(IsGameOn && IsInGamePage)
                        {
                            if (((App)App.Current).MainPage.BindingContext != null)
                            {
                                if (((App)App.Current).MainPage.BindingContext.Equals(this))
                                {
                                    IsInGamePage = false;
                                    IsGameOn = false;
                                }
                            }
                        }
                        if (!IsGameOn)
                        {
                            if (!IsGameActive && !IsHost)
                            {
                                ((App)App.Current).currentGame = null;
                                ((App)App.Current).currentPlayerInGame = null;
                                await ((App)App.Current).MainPage.Navigation.PopAsync();
                                var closedToastOptions = new ToastOptions
                                {
                                    BackgroundColor = Xamarin.Forms.Color.Black,
                                    MessageOptions = new MessageOptions
                                    {
                                        Message = "The Game Was Closed by the host",
                                        Foreground = Xamarin.Forms.Color.White,
                                    },
                                    CornerRadius = 5,
                                    Duration = System.TimeSpan.FromSeconds(3),
                                };
                                await ((App)App.Current).MainPage.DisplayToastAsync(closedToastOptions);
                            }
                            if (!AreYouInGame && IsGameActive && !IsHost)
                            {
                                ((App)App.Current).currentGame = null;
                                ((App)App.Current).currentPlayerInGame = null;
                                await ((App)App.Current).MainPage.Navigation.PopAsync();
                                var kickedToastOptions = new ToastOptions
                                {
                                    BackgroundColor = Xamarin.Forms.Color.Black,
                                    MessageOptions = new MessageOptions
                                    {
                                        Message = "You were kicked out by the host",
                                        Foreground = Xamarin.Forms.Color.White,
                                    },
                                    CornerRadius = 5,
                                    Duration = System.TimeSpan.FromSeconds(3),
                                };
                                await ((App)App.Current).MainPage.DisplayToastAsync(kickedToastOptions);
                            }
                        }
                    }
                    //interact with UI elements
                });
                if (!IsGameActive || !AreYouInGame || DidLeaveTheGame) 
                {
                    return false;
                }
                return true; // runs again, or false to stop
            });
            
        }
        public void UpdateChatRoom()
        {
            foreach(Message m in ((App)App.Current).currentGame.Messages)
            {
                //if (!ChatMessages.Contains(m))
                if (!IsInChatMessages(m))
                { 
                    ChatMessages.Insert(0, m);
                }
            }
           // ChatMessages.OrderByDescending(m => m.TimeSent);
        }
        public bool IsInChatMessages(Message message)
        {
            foreach (Message m in ChatMessages)
            {
                if (m.Id == message.Id)
                    return true;
            }
            return false;
        }
        public void UpdatePlayersInGameList()
        {
            // deleting players that are no longer in game
            foreach (PlayersInGame p in this.PlayersInGameList.ToList())
            {
                if (!IsInSourceGamesPlayerList(p))
                {
                    this.PlayersInGameList.Remove(p);
                }
            }
            // Updating colors
            if (this.PlayersInGameList.Count > 0)
            {
               
                foreach (PlayersInGame p in this.PlayersInGameList.ToList())
                {
                    PlayersInGame player = ((App)App.Current).currentGame.PlayersInGames.Where(pl => pl.Id == p.Id).FirstOrDefault();
                   
                    if (p.ColorId != player.ColorId)
                    {
                        int index = PlayersInGameList.IndexOf(p);
                        this.PlayersInGameList.Remove(p);
                        this.PlayersInGameList.Insert(index, player);
                        if (((App)App.Current).currentPlayerInGame.Id == p.Id)
                        {
                            ((App)App.Current).currentPlayerInGame = player;
                        }
                    }
                }
            }
        }
        public void AddNewPlayers()
        {
            foreach(PlayersInGame p in ((App)App.Current).currentGame.PlayersInGames.ToList())
            {
                if (!IsInPlayersInGameList(p) && p.IsInGame)
                    this.PlayersInGameList.Add(p);
            }
        }
        public bool IsInPlayersInGameList(PlayersInGame p)
        {
            foreach(PlayersInGame pl in this.PlayersInGameList.ToList())
            {
                if (p.Id == pl.Id)
                    return true;
            }
            return false;
        }
        public bool IsInSourceGamesPlayerList(PlayersInGame p)
        {
            //ConcurrentBag<PlayersInGame> PlayersInGames = new ConcurrentBag<PlayersInGame>(((App)App.Current).currentGame.PlayersInGames);
            foreach (PlayersInGame pl in ((App)App.Current).currentGame.PlayersInGames.ToList())
            {
                if (p.Id == pl.Id && pl.IsInGame)
                    return true;
            }
            return false;
        }
        
    }
}
