using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TopRaceApp.Models;
using TopRaceApp.DTOs;
using TopRaceApp.Services;
using System.Threading.Tasks;
using Xamarin.Forms;
using TopRaceApp.Views;
using System.Windows.Input;

namespace TopRaceApp.ViewModels
{
    class GamePageViewModel : BaseViewModel
    {
        #region Properties
        private string crewmatePic1;
        public string CrewmatePic1
        {
            get
            {
                return crewmatePic1;
            }
            set
            {
                if (value != crewmatePic1)
                {
                    crewmatePic1 = value;
                    OnPropertyChanged();
                }
            }
        }
        private string crewmatePic2;
        public string CrewmatePic2
        {
            get
            {
                return crewmatePic2;
            }
            set
            {
                if (value != crewmatePic2)
                {
                    crewmatePic2 = value;
                    OnPropertyChanged();
                }
            }
        }
        private string crewmatePic3;
        public string CrewmatePic3
        {
            get
            {
                return crewmatePic3;
            }
            set
            {
                if (value != crewmatePic3)
                {
                    crewmatePic3 = value;
                    OnPropertyChanged();
                }
            }
        }
        private string crewmatePic4;
        public string CrewmatePic4
        {
            get
            {
                return crewmatePic4;
            }
            set
            {
                if (value != crewmatePic4)
                {
                    crewmatePic4 = value;
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
        public ObservableCollection<Message> ChatMessages { get; set; }
        private int timer;
        public int Timer
        {
            get
            {
                return timer;
            }
            set
            {
                if (timer != value)
                {
                    timer = value;
                    OnPropertyChanged();
                }
            }
        }
        public PlayersInGame CurrentPlayerInTurn { get; set; }
        public PlayersInGame Winner { get; set; }
        public PlayersInGame PreviousPlayer { get; set; }
        private DateTime lastUpdateTime;
        public DateTime LastUpdateTime
        {
            get
            {
                return lastUpdateTime;
            }
            set
            {
                if (value != lastUpdateTime)
                {
                    lastUpdateTime = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool isMyTurn;
        public bool IsMyTurn
        {
            get
            {
                return isMyTurn;
            }
            set
            {
                if (isMyTurn != value)
                {
                    isMyTurn = value;
                    OnPropertyChanged();
                }
            }
        }
        private int lastRoll;
        public int LastRoll
        {
            get
            {
                return lastRoll;
            }
            set
            {
                if (lastRoll != value)
                {
                    lastRoll = value;
                    OnPropertyChanged();
                }
            }
        }
        public int UpdatesCounter{ get; set; }
        public GamePage GamePage { get; set; }
        public Mover [,] Board { get; set; }
        public List<PlayersInGame> Players { get; set; }
        public List<bool> IsMovingList { get; set; }
        public ICommand SendMessageCommand { get; set; }
        public Position CurrentPos { get; set; }
        public Position PreviousPos { get; set; }
        public ICommand RollCommand { get; set; }
        #endregion
        public GamePageViewModel()
        {
            UpdatesCounter = ((App)App.Current).currentGame.UpdatesCounter;
            LastUpdateTime = ((App)App.Current).currentGame.LastUpdateTime;
            LastRoll = 0;
            Timer = 15;
            SendMessageCommand = new Command(SendMessage);
            RollCommand = new Command(Roll);
            ChatMessages = new ObservableCollection<Message>();
            List<Message> messages = ((App)App.Current).currentGame.Messages.ToList();
            for (int i = messages.Count - 1 ; i > -1; i--)
            {
                ChatMessages.Add(messages[i]);
            }
            Players = new List<PlayersInGame>();
            foreach (PlayersInGame p in ((App)App.Current).currentGame.PlayersInGames)
            {
                TopRaceAPIProxy proxy = TopRaceAPIProxy.CreateProxy();
                if (p.IsInGame)
                {
                    if (p.Color.PicLink.StartsWith(proxy.GetBasePhotoUri()) == false)
                    { 
                        p.Color.PicLink = proxy.GetBasePhotoUri() + p.Color.PicLink; 
                    }
                    Players.Add(p);
                }
            }
            IsMovingList = new List<bool>();
            for (int i = 0; i < Players.Count; i++)
            {
                IsMovingList.Add(false);
            }
            CrewmatePic1 = Players[0].Color.PicLink;
            CrewmatePic2 = Players[1].Color.PicLink;
            if (Players.Count > 2)
            { 
                CrewmatePic3 = Players[2].Color.PicLink;
            }
            if (Players.Count > 3)
            { 
                CrewmatePic4 = Players[3].Color.PicLink;
            }
            Mover[][] jaggedArry = ((App)App.Current).currentGame.Board;
            Board = GameDTO.ToMatrix(jaggedArry);
            CurrentPlayerInTurn = ((App)App.Current).currentGame.CurrentPlayerInTurn;
            IsMyTurn = CurrentPlayerInTurn.Id == ((App)App.Current).currentPlayerInGame.Id;
        }
        public async Task Run()
        {
            TopRaceAPIProxy proxy = TopRaceAPIProxy.CreateProxy();

            Device.StartTimer(new TimeSpan(0, 0, 2), () =>
            {
                // do something every 2 seconds
                Device.BeginInvokeOnMainThread(async () =>
                {
                    ((App)App.Current).currentGame = await proxy.GetGameAsync(((App)App.Current).currentGame.Id);

                    // interact with UI elements
                    TimeSpan minimum = new TimeSpan(0, 0, 0, 0, 500);
                    TimeSpan diff = ((App)App.Current).currentGame.LastUpdateTime - LastUpdateTime;
                if (UpdatesCounter < ((App)App.Current).currentGame.UpdatesCounter)
                    {
                        DateTime t = LastUpdateTime;
                        ((App)App.Current).currentPlayerInGame = ((App)App.Current).currentGame.PlayersInGames.Where(p => p.UserId == ((App)App.Current).currentUser.Id).FirstOrDefault();
                        CurrentPlayerInTurn = ((App)App.Current).currentGame.CurrentPlayerInTurn;
                        PreviousPlayer = ((App)App.Current).currentGame.PreviousPlayer;
                        LastUpdateTime = ((App)App.Current).currentGame.LastUpdateTime;
                        UpdatesCounter = ((App)App.Current).currentGame.UpdatesCounter;
                        int prevoiusID = PreviousPlayer.Id;
                        PlayersInGame unUpdatedPlayer = null;
                        int index = 0;
                        for (int i = 0; i < Players.Count; i++)
                        {
                            PlayersInGame pl = Players[i];
                            if(pl.Id == prevoiusID)
                            {
                                unUpdatedPlayer = pl;
                                Players[i] = PreviousPlayer;
                                index = i;
                            }
                        }
                        IsMovingList[index] = true;
                        LastRoll = ((App)App.Current).currentGame.LastRollResult;
                        GamePage.MoveCrewmate(index, unUpdatedPlayer.CurrentPos, ((App)App.Current).currentGame.LastRollResult, PreviousPlayer.CurrentPos);
                        IsMovingList[index] = false;
                        Winner = ((App)App.Current).currentGame.Winner;
                        if (Winner != null)
                        {
                            // pops up winner! back to lobby or homePage
                        }
                        IsMyTurn = CurrentPlayerInTurn.Id == ((App)App.Current).currentPlayerInGame.Id;
                        if (IsMyTurn)
                        {
                            //start timer
                        }
                    }
                    UpdateChatRoom();
                });
                return Winner == null; // runs again, or false to stop
            });
        }
        public void UpdateChatRoom()
        {
            foreach (Message m in ((App)App.Current).currentGame.Messages)
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
        private async void Roll()
        {
            if(IsMyTurn)
            {
                TopRaceAPIProxy proxy = TopRaceAPIProxy.CreateProxy();
                ((App)App.Current).currentGame = await proxy.PlayAsync(((App)App.Current).currentGame.Id);
                ((App)App.Current).currentPlayerInGame = ((App)App.Current).currentGame.PlayersInGames.Where(p => p.UserId == ((App)App.Current).currentUser.Id).FirstOrDefault();
                LastUpdateTime = ((App)App.Current).currentGame.LastUpdateTime;
                CurrentPlayerInTurn = ((App)App.Current).currentGame.CurrentPlayerInTurn;
                PreviousPlayer = ((App)App.Current).currentGame.PreviousPlayer;
                LastUpdateTime = ((App)App.Current).currentGame.LastUpdateTime;
                PlayersInGame unUpdatedPlayer = null;
                int index = 0;
                List<PlayersInGame> players = Players.ToList();
                foreach (PlayersInGame pl in players)
                {
                    if (pl.Id == ((App)App.Current).currentPlayerInGame.Id)
                    {
                        unUpdatedPlayer = pl;
                        index = Players.IndexOf(pl);
                        Players.Remove(pl);
                        Players.Insert(index, ((App)App.Current).currentPlayerInGame);
                    }
                }
                LastRoll = ((App)App.Current).currentGame.LastRollResult;
                UpdatesCounter = ((App)App.Current).currentGame.UpdatesCounter;
                GamePage.MoveCrewmate(index, unUpdatedPlayer.CurrentPos, ((App)App.Current).currentGame.LastRollResult, PreviousPlayer.CurrentPos);
                IsMyTurn = CurrentPlayerInTurn.Id == ((App)App.Current).currentPlayerInGame.Id;
                Winner = ((App)App.Current).currentGame.Winner;
                if (Winner != null)
                {
                    IsMyTurn = false;
                    // pops up winner! back to lobby or homePage
                }
            }
            
        }
    }
}
