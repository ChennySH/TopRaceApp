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
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views.Options;

namespace TopRaceApp.ViewModels
{
    class GamePageViewModel : BaseViewModel
    {
        #region Properties
        private bool isHost;
        public bool IsHost
        {
            get
            {
                return isHost;
            }
            set
            {
                if (isHost != value)
                {
                    isHost = value;
                    OnPropertyChanged();
                }
            }
        }
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
        private string resultSetter;
        public string ResultSetter
        {
            get
            {
                return resultSetter; 
            }
            set
            {
                if (resultSetter != value)
                {
                    resultSetter = value;
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
        private string winnerCrewmate;
        public string WinnerCrewmate
        {
            get
            {
                return winnerCrewmate;
            }
            set
            {
                if(winnerCrewmate != value)
                {
                    winnerCrewmate = value;
                    OnPropertyChanged();
                }
            }
        }
        private string winnerName;
        public string WinnerName
        {
            get
            {
                return winnerName;
            }
            set
            {
                if(winnerName != value)
                {
                    winnerName = value;
                    OnPropertyChanged();
                        
                }
            }
        }
        private string winnerProfilePic;
        public string WinnerProfilePic
        {
            get
            {
                return winnerProfilePic;
            }
            set
            {
                if (winnerProfilePic != value)
                {
                    winnerProfilePic = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool didShowWinner;
        public bool DidShowWinner
        {
            get
            {
                return didShowWinner;
            }
            set
            {
                if(didShowWinner != value)
                {
                    didShowWinner = value;
                    OnPropertyChanged();
                }
            }
        }
        private string winnerOrLoser;
        public string WinnerOrLoser
        {
            get
            {
                return winnerOrLoser;
            }
            set
            {
                if(winnerOrLoser != value)
                {
                    winnerOrLoser = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool didQuit;
        public bool DidQuit
        {
            get 
            { 
                return didQuit;
            }
            set
            {
                if(didQuit != value)
                {
                    didQuit = value;
                    OnPropertyChanged();
                }
            }
        }
        public int UpdatesCounter{ get; set; }
        public int MovesCounter { get; set; }
        public GamePage GamePage { get; set; }
        public Mover [,] Board { get; set; }
        public List<PlayersInGame> Players { get; set; }
        public List<bool> IsMovingList { get; set; }
        public const int TIMER_TIME = 30;
        public ICommand SendMessageCommand { get; set; }
        public Position CurrentPos { get; set; }
        public Position PreviousPos { get; set; }
        public ICommand RollCommand { get; set; }
        public ICommand RollTestCommand { get; set; }
        public ICommand BackToLobbyPageCommand { get; set; }
        public ICommand QuitGameAfterGameIsOverCommand { get; set; }
        public ICommand QuitDuringGameCommand { get; set; }
        public event Action<int> ScrollToButton;
        #endregion
        public GamePageViewModel()
        {
            DidQuit = false;
            IsHost = ((App)App.Current).currentPlayerInGame.IsHost;
            ResultSetter = "0";
            UpdatesCounter = ((App)App.Current).currentGame.UpdatesCounter;
            MovesCounter = ((App)App.Current).currentGame.MovesCounter;
            LastUpdateTime = ((App)App.Current).currentGame.LastUpdateTime;
            LastRoll = 0;
            Timer = TIMER_TIME;
            MessageText = "";
            SendMessageCommand = new Command(SendMessage);
            RollCommand = new Command(Roll);
            RollTestCommand = new Command(RollTest);
            BackToLobbyPageCommand = new Command(BackToLobbyPage);
            QuitGameAfterGameIsOverCommand = new Command(QuitGameAfterGameIsOver);
            QuitDuringGameCommand = new Command(QuitDuringGame);
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
            if (IsMyTurn)
            {
                //start timer
                StartYourTimer();
            }
            else
            {
                StartRivalTimer();
            }
            Device.StartTimer(TimeSpan.FromSeconds(0.5), () =>
            {
                // do something every 0.1 seconds
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (!DidQuit)
                    {
                        ((App)App.Current).currentGame = await proxy.GetGameAsync(((App)App.Current).currentGame.Id);
                        // interact with UI elements
                        //TimeSpan minimum = new TimeSpan(0, 0, 0, 0, 500);
                        //TimeSpan diff = ((App)App.Current).currentGame.LastUpdateTime - LastUpdateTime;
                        if (UpdatesCounter < ((App)App.Current).currentGame.UpdatesCounter)                        
                        {
                            List<PlayersInGame> playersList = ((App)App.Current).currentGame.PlayersInGames.Where(pl => pl.IsInGame).ToList();
                            if (playersList.Count > 1)
                            {
                                //DateTime t = LastUpdateTime;
                                ((App)App.Current).currentPlayerInGame = ((App)App.Current).currentGame.PlayersInGames.Where(p => p.UserId == ((App)App.Current).currentUser.Id).FirstOrDefault();
                                
                                LastUpdateTime = ((App)App.Current).currentGame.LastUpdateTime;
                                UpdatesCounter = ((App)App.Current).currentGame.UpdatesCounter;
                                while (playersList.Count < Players.Count)
                                {
                                    int removedIndex = 0;
                                    for (int i = 0; i < Players.Count; i++)
                                    {
                                        PlayersInGame player = Players[i];
                                        bool isInGame = false;
                                        foreach (PlayersInGame pl in playersList)
                                        {
                                            if (pl.Id == player.Id)
                                            {
                                                isInGame = true;
                                            }
                                        }
                                        if (!isInGame)
                                        {
                                            removedIndex = i;
                                        }
                                    }
                                    Players.RemoveAt(removedIndex);
                                    this.GamePage.RemoveFromLists(removedIndex);
                                }
                                int oldCurrentPlayerID = CurrentPlayerInTurn.Id;
                                CurrentPlayerInTurn = ((App)App.Current).currentGame.CurrentPlayerInTurn;
                                PreviousPlayer = ((App)App.Current).currentGame.PreviousPlayer;
                                IsMyTurn = (CurrentPlayerInTurn.Id == ((App)App.Current).currentPlayerInGame.Id) && (Winner == null);
                                // restartTimer if the currentPlayerQuited, if there was also a move it would restart only there 
                                if (MovesCounter == ((App)App.Current).currentGame.MovesCounter)
                                {
                                    if (oldCurrentPlayerID != CurrentPlayerInTurn.Id)
                                    {
                                        if (IsMyTurn)
                                        {
                                            StartYourTimer();
                                        }
                                        else
                                        {
                                            StartRivalTimer();
                                        }
                                    }
                                }
                                if (MovesCounter < ((App)App.Current).currentGame.MovesCounter)
                                {
                                    if (PreviousPlayer != null)
                                    {
                                        int prevoiusID = PreviousPlayer.Id;
                                        PlayersInGame unUpdatedPlayer = null;
                                        int index = -1;
                                        for (int i = 0; i < Players.Count; i++)
                                        {
                                            PlayersInGame pl = Players[i];
                                            if (pl.Id == prevoiusID)
                                            {
                                                unUpdatedPlayer = pl;
                                                Players[i] = PreviousPlayer;
                                                index = i;
                                            }
                                        }
                                        if (index != -1)
                                        {
                                            IsMovingList[index] = true;
                                            LastRoll = ((App)App.Current).currentGame.LastRollResult;
                                            GamePage.MoveCrewmate(index, unUpdatedPlayer.CurrentPos, ((App)App.Current).currentGame.LastRollResult, PreviousPlayer.CurrentPos);
                                            IsMovingList[index] = false;
                                        }

                                    }
                                    MovesCounter = ((App)App.Current).currentGame.MovesCounter;
                                    if (IsMyTurn)
                                    {
                                        StartYourTimer();
                                    }
                                    else
                                    {
                                        StartRivalTimer();
                                    }
                                }
                                int currentIndex = GetCurrentIndex();
                                this.GamePage.SetFramesBackground(currentIndex);
                            }
                            Winner = ((App)App.Current).currentGame.Winner;
                            if (Winner != null)
                            {
                                // pops up winner! back to lobby or homePage
                                if (!DidShowWinner)
                                {
                                    ShowWinner();
                                    DidShowWinner = true;
                                }
                            }

                        }
                        UpdateChatRoom();
                    }
                });
                return ((Winner == null) && !DidQuit); // runs again, or false to stop
            });
        }
        public async void QuitDuringGame()
        {           
            TopRaceAPIProxy proxy = TopRaceAPIProxy.CreateProxy();
            if (!isHost)
            {
                bool isKicked = await proxy.KickOutAsync(((App)App.Current).currentGame.Id, ((App)App.Current).currentPlayerInGame.Id);
                if (isKicked)
                {
                    DidQuit = true;
                    await App.Current.MainPage.Navigation.PopAsync();
                    NavigationPage navigationPage = (NavigationPage)((App)App.Current).MainPage;
                    LobbyPageViewModel vm = ((LobbyPageViewModel)navigationPage.CurrentPage.BindingContext);
                    vm.AreYouInGame = false;
                    await App.Current.MainPage.Navigation.PopAsync();
                }
            }
        }
        public void UpdateChatRoom()
        {
            foreach (Message m in ((App)App.Current).currentGame.Messages)
            {
                //if (!ChatMessages.Contains(m))
                if (!IsInChatMessages(m))
                {
                    ChatMessages.Add(m);
                    ScrollToButton(ChatMessages.IndexOf(m));
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
            if (IsMyTurn)
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
                MovesCounter = ((App)App.Current).currentGame.MovesCounter;
                GamePage.MoveCrewmate(index, unUpdatedPlayer.CurrentPos, ((App)App.Current).currentGame.LastRollResult, PreviousPlayer.CurrentPos);
                IsMyTurn = CurrentPlayerInTurn.Id == ((App)App.Current).currentPlayerInGame.Id;
                int currentIndex = GetCurrentIndex();
                this.GamePage.SetFramesBackground(currentIndex);
                Winner = ((App)App.Current).currentGame.Winner;
                if (Winner != null)
                {
                    IsMyTurn = false;
                    ShowWinner();
                    DidShowWinner = true;
                }
                if(IsMyTurn)
                { 
                    //start timer
                    StartYourTimer();
                }
                else
                {
                    StartRivalTimer();
                }
            }
            
        }
        private async void RollTest()
        {
            if (IsMyTurn)
            {
                int result = 0;
                try
                {
                    result = int.Parse(ResultSetter);
                }
                catch
                {
                    Roll();
                    return;
                }               
                TopRaceAPIProxy proxy = TopRaceAPIProxy.CreateProxy();
                ((App)App.Current).currentGame = await proxy.PlayTestAsync(((App)App.Current).currentGame.Id, result);
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
                MovesCounter = ((App)App.Current).currentGame.MovesCounter;
                GamePage.MoveCrewmate(index, unUpdatedPlayer.CurrentPos, ((App)App.Current).currentGame.LastRollResult, PreviousPlayer.CurrentPos);
                IsMyTurn = CurrentPlayerInTurn.Id == ((App)App.Current).currentPlayerInGame.Id;
                int currentIndex = GetCurrentIndex();
                this.GamePage.SetFramesBackground(currentIndex);
                Winner = ((App)App.Current).currentGame.Winner;
                if (Winner != null)
                {
                    IsMyTurn = false;
                    ShowWinner();
                    DidShowWinner = true;
                }
                if (IsMyTurn)
                {
                    //start timer
                    StartYourTimer();
                }
                else
                {
                    StartRivalTimer();
                }

            }

        }
        public void ShowWinner()
        {
            TopRaceAPIProxy proxy = TopRaceAPIProxy.CreateProxy();
            ContentPage winnerPopUp = new WinnerPopUp();
            winnerPopUp.BindingContext = this;
            WinnerCrewmate = Winner.Color.PicLink;
            if (!WinnerCrewmate.StartsWith(proxy.GetBasePhotoUri()))
            {
                WinnerCrewmate = proxy.GetBasePhotoUri() + WinnerCrewmate;
            }
            WinnerName = Winner.UserName;
            WinnerProfilePic = Winner.ProfileImageSource;
            if(Winner.Id == ((App)App.Current).currentPlayerInGame.Id)
            {
                WinnerOrLoser = "Winner!";
            }
            else
            {
                WinnerOrLoser = "You Lose";
            }
            ((App)App.Current).MainPage.Navigation.PushModalAsync(winnerPopUp);
           
        }
        private async Task ResetGame()
        {
            TopRaceAPIProxy proxy = TopRaceAPIProxy.CreateProxy();
            ((App)App.Current).currentGame = await proxy.ResetGameAsync(((App)App.Current).currentGame.Id);
            ((App)App.Current).currentPlayerInGame = ((App)App.Current).currentGame.PlayersInGames.Where(p => p.UserId == ((App)App.Current).currentUser.Id).FirstOrDefault();

        }
        private async void BackToLobbyPage()
        {
            if (IsHost)
            {
                await ResetGame();
            }
            if (((App)App.Current).currentGame != null && ((App)App.Current).currentGame.StatusId == 2)
            {
                return;
            }
            await ((App)App.Current).MainPage.Navigation.PopModalAsync();
            await ((App)App.Current).MainPage.Navigation.PopAsync();
            NavigationPage navigationPage = (NavigationPage)((App)App.Current).MainPage;
            LobbyPageViewModel vm = ((LobbyPageViewModel)navigationPage.CurrentPage.BindingContext);
            if (((App)App.Current).currentGame != null)
            {
                vm.IsGameOn = false;
                vm.IsInGamePage = false;
            }
            else
            {
                ((App)App.Current).currentGame = null;
                ((App)App.Current).currentPlayerInGame = null;
                await ((App)App.Current).MainPage.Navigation.PopAsync();
                ShowMessage("The game was closed", "The game was closed by the host");
            }
        }
        private async void QuitGameAfterGameIsOver()
        {

            if (IsHost)
            {
                await ResetGame();
                await ((App)App.Current).MainPage.Navigation.PopModalAsync();
                await ((App)App.Current).MainPage.Navigation.PopAsync();
                NavigationPage navigationPage = (NavigationPage)((App)App.Current).MainPage;
                LobbyPageViewModel vm = ((LobbyPageViewModel)navigationPage.CurrentPage.BindingContext);
                vm.CloseGame();
            }
            else
            {
                await ((App)App.Current).MainPage.Navigation.PopModalAsync();
                await ((App)App.Current).MainPage.Navigation.PopAsync();
                NavigationPage navigationPage = (NavigationPage)((App)App.Current).MainPage;
                LobbyPageViewModel vm = ((LobbyPageViewModel)navigationPage.CurrentPage.BindingContext);
                if (((App)App.Current).currentGame != null && ((App)App.Current).currentGame.StatusId != 3)
                {
                    vm.LeaveGame();
                }
                else
                {
                    ((App)App.Current).currentGame = null;
                    ((App)App.Current).currentPlayerInGame = null;
                    await ((App)App.Current).MainPage.Navigation.PopAsync();
                }
            }
        }
        public async void ShowMessage(string title, string text)
        {
            if (Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.iOS)
            {
                var kickedToastOptions = new ToastOptions
                {
                    BackgroundColor = Xamarin.Forms.Color.Black,
                    MessageOptions = new MessageOptions
                    {
                        Message = text,
                        Foreground = Xamarin.Forms.Color.White,
                    },
                    CornerRadius = 5,
                    Duration = System.TimeSpan.FromSeconds(3),
                };
                await ((App)App.Current).MainPage.DisplayToastAsync(kickedToastOptions);
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(title, text, "OK");
            }
        }
        public int GetCurrentIndex()
        {
            int index = 0;
            for (int i = 0; i < Players.Count; i++)
            {
                if(Players[i].Id == CurrentPlayerInTurn.Id)
                {
                    index = i;
                }
            }
            return index;
        }
        public void StartYourTimer()
        {
            int currentMovesCounter = this.MovesCounter;
            bool didTimerEnd = false;
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                if (!DidQuit)
                {
                    if (Winner == null)
                    {
                        if (currentMovesCounter == this.MovesCounter)
                        {
                            this.Timer -= 1;
                            if (this.Timer == 0)
                            {
                                didTimerEnd = true;
                                Roll();
                            }
                        }
                    }
                }
                if ((currentMovesCounter == this.MovesCounter && (!didTimerEnd) && (!DidQuit) && Winner == null) == false)
                {
                    this.Timer = TIMER_TIME;
                }
                return currentMovesCounter == this.MovesCounter && (!didTimerEnd) && (!DidQuit) && Winner == null;
            });
        }
        public void StartRivalTimer()
        {
            int currentMovesCounter = this.MovesCounter;
            bool didTimerEnd = false;
            int playerInTurnID = CurrentPlayerInTurn.Id;
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                if (!DidQuit)
                {
                    if (Winner == null)
                    {
                        if (playerInTurnID == CurrentPlayerInTurn.Id)
                        {
                            if (currentMovesCounter == this.MovesCounter)
                            {
                                this.Timer -= 1;
                                if (this.Timer == 0)
                                {
                                    didTimerEnd = true;
                                }
                            }
                        }
                    }
                }
                if ((currentMovesCounter == this.MovesCounter && (!didTimerEnd) && (!DidQuit) && Winner == null && playerInTurnID == CurrentPlayerInTurn.Id) == false)
                {
                    this.Timer = TIMER_TIME;
                }
                return currentMovesCounter == this.MovesCounter && (!didTimerEnd) && (!DidQuit) && Winner == null && playerInTurnID == CurrentPlayerInTurn.Id;
            });
        }
    }
}
