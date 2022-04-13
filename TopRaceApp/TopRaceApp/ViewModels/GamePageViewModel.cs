using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TopRaceApp.Models;
using TopRaceApp.DTOs;
using TopRaceApp.Services;

namespace TopRaceApp.ViewModels
{
    class GamePageViewModel : BaseViewModel
    {
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
                if(timer != value)
                {
                    timer = value;
                    OnPropertyChanged();
                }
            }
        }
        public Mover [,] Board { get; set; }
        public List<PlayersInGame> Players { get; set; }

        public GamePageViewModel()
        {
            Timer = 15;
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
        }

    }
}
