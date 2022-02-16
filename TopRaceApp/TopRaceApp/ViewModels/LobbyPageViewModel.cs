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
        //private Models.Color selectedColor;
        //public Models.Color SelectedColor
        //{
        //    get
        //    {
        //        return selectedColor;
        //    }
        //    set
        //    {
        //        if(selectedColor != value)
        //        {
        //            selectedColor = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}
        public ObservableCollection<PlayersInGame> PlayersInGameList { get; set; }
        public ObservableCollection<Message> ChatMessages { get; set; }
        public ObservableCollection<Models.Color> ColorsCollection { get; set; }
        #endregion
        public LobbyPageViewModel()
        {
            IsGameActive = ((App)App.Current).currentGame.StatusId == 1;
            RoomStatus = string.Empty;
            IsHost = (((App)App.Current).currentGame.HostUserId == ((App)App.Current).currentUser.Id);
            IsNotHost = (((App)App.Current).currentGame.HostUserId != ((App)App.Current).currentUser.Id);
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
                {
                    p.Color.PicLink = proxy.GetBasePhotoUri() + p.Color.PicLink;
                }
                PlayersInGameList.Add(p);
            }
            ChatMessages = new ObservableCollection<Message>();
            foreach(Message m in ((App)App.Current).currentGame.ChatRoom.Messages)
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
        }


        public ICommand SendMessageCommand { get; set; }
        public ICommand OpenColorChangeViewCommand { get; set; }
        public ICommand CloseColorChangeViewCommand { get; set; }
        public ICommand ChangeColorCommand{ get; set; }
        public ICommand CloseGameCommand { get; set; }

        private async void SendMessage()
        {
            TopRaceAPIProxy proxy = TopRaceAPIProxy.CreateProxy();
            Message newMessage = new Message
            {
                Message1 = this.MessageText,
                From = ((App)App.Current).currentPlayerInGame,
                ChatRoom = ((App)App.Current).currentGame.ChatRoom,
                TimeSent = DateTime.Now
            };
            bool success=await proxy.SendMessageAsync(newMessage);
            if (success)
                this.MessageText = string.Empty;           
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
                ((App)App.Current).currentGame = null;
                ((App)App.Current).currentPlayerInGame = null;
                ((App)App.Current).MainPage = new MainPage();
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
                await ((App)App.Current).MainPage.DisplayToastAsync(closedToastOptions);
            }
            else
            {
                var notClosedToastOptions = new ToastOptions
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
                await ((App)App.Current).MainPage.DisplayToastAsync(notClosedToastOptions);
            }
        }
        public async void ChangeColor(Models.Color color)
        {
            TopRaceAPIProxy proxy = TopRaceAPIProxy.CreateProxy();
            ((App)App.Current).currentPlayerInGame.ColorId = color.Id;
            bool isUpdated = await proxy.UpdatePlayerAsync(((App)App.Current).currentPlayerInGame);
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
                    ((App)App.Current).currentGame = await proxy.GetGameAsync(((App)App.Current).currentGame.Id);
                    IsGameActive = ((App)App.Current).currentGame.StatusId == 1;
                    if (IsGameActive)
                    {
                        ((App)App.Current).currentPlayerInGame = ((App)App.Current).currentGame.PlayersInGames.Where(p => p.UserId == ((App)App.Current).currentUser.Id).FirstOrDefault();
                        UpdateChatRoom();
                        UpdatePlayersInGameList();
                        AddNewPlayers();
                        PlayersInGameList.OrderBy(p => p.Id);
                    }
                    //interact with UI elements
                });
                if (!IsGameActive)
                {
                    return false;
                }
                return true; // runs again, or false to stop
            });
            if(((App)App.Current).currentGame.StatusId == 3)
            {
                ((App)App.Current).currentGame = null;
                ((App)App.Current).currentPlayerInGame = null;
                ((App)App.Current).MainPage.Navigation.PushAsync(new MainPage());
                var closedToastOptions = new ToastOptions
                {
                    BackgroundColor = Xamarin.Forms.Color.Black,
                    MessageOptions = new MessageOptions
                    {
                        Message = "The Game Was Closed by the host.",
                        Foreground = Xamarin.Forms.Color.White,
                    },
                    CornerRadius = 5,
                    Duration = System.TimeSpan.FromSeconds(3),
                };
                await ((App)App.Current).MainPage.DisplayToastAsync(closedToastOptions);
            }
        }
        public void UpdateChatRoom()
        {
            foreach(Message m in ((App)App.Current).currentGame.ChatRoom.Messages)
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
            foreach (PlayersInGame p in this.PlayersInGameList)
            {
                if (!IsInGamesPlayerList(p))
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
                        
                        this.PlayersInGameList.Remove(p);
                        this.PlayersInGameList.Add(player);
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
            foreach(PlayersInGame p in ((App)App.Current).currentGame.PlayersInGames)
            {
                if (!IsInPlayersInGameList(p))
                    this.PlayersInGameList.Add(p);
            }
        }
        public bool IsInPlayersInGameList(PlayersInGame p)
        {
            foreach(PlayersInGame pl in this.PlayersInGameList)
            {
                if (p.Id == pl.Id)
                    return true;
            }
            return false;
        }
        public bool IsInGamesPlayerList(PlayersInGame p)
        {
            foreach (PlayersInGame pl in ((App)App.Current).currentGame.PlayersInGames)
            {
                if (p.Id == pl.Id)
                    return true;
            }
            return false;
        }
        
    }
}
