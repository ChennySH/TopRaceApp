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

namespace TopRaceApp.ViewModels
{
    class LobbyPageViewModel:BaseViewModel
    {
        #region properties 
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
                    IsNotHost = !IsHost;
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
        public ObservableCollection<PlayersInGame> PlayersInGameList;
        public ObservableCollection<Message> ChatMessages;
        #endregion
        public LobbyPageViewModel()
        {
            IsHost = (((App)App.Current).currentGame.HostPlayerId == ((App)App.Current).currentPlayer.Id);
            GameName = ((App)App.Current).currentGame.GameName;
            PrivateKey = ((App)App.Current).currentGame.PrivateKey;
            IsPrivate = ((App)App.Current).currentGame.IsPrivate;
            HostName = ((App)App.Current).currentGame.HostPlayer.PlayerName;
            HostProfilePic = ((App)App.Current).currentGame.HostPlayer.ProfilePic;
            MessageText = "";
            PlayersInGameList = new ObservableCollection<PlayersInGame>();
            foreach (PlayersInGame p in ((App)App.Current).currentGame.PlayersInGames)
            {
                PlayersInGameList.Add(p);
            }
            ChatMessages = new ObservableCollection<Message>();
            foreach(Message m in ((App)App.Current).currentGame.ChatRoom.Messages)
            {
                ChatMessages.Add(m);
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
                    ((App)App.Current).currentGame = await proxy.GetGame(((App)App.Current).currentGame.Id);
                    ((App)App.Current).currentPlayerInGame = ((App)App.Current).currentGame.PlayersInGames.Where(p => p.PlayerId == ((App)App.Current).currentPlayer.Id).FirstOrDefault();
                    UpdatePlayersInGameList();
                    UpdateChatRoom();
                    // interact with UI elements
                });
                return true; // runs again, or false to stop
            });
        }
        public void UpdateChatRoom()
        {
            foreach(Message m in ((App)App.Current).currentGame.ChatRoom.Messages)
            {
                if (!IsInChatMessages(m))
                    ChatMessages.Add(m);
            }
            ChatMessages.OrderByDescending(m => m.TimeSent);
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
            PlayersInGameList.Clear();
            foreach (PlayersInGame p in ((App)App.Current).currentGame.PlayersInGames)
            {
                PlayersInGameList.Add(p);
            }
            PlayersInGameList.OrderBy(p => p.Number);
        }
    }
}
