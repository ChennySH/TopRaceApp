﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using TopRaceApp.Services;


namespace TopRaceApp.Models
{
    public partial class PlayersInGame
    {
        public PlayersInGame()
        {
            GameCurrentPlayerInTurns = new List<Game>();
            GamePreviousPlayers = new List<Game>();
            GameWinners = new List<Game>();
            Messages = new List<Message>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string ProfilePic { get; set; }
        public bool IsHost { get; set; }
        public bool IsInGame { get; set; }
        public bool DidPlayInGame { get; set; }
        public int ColorId { get; set; }
        public int ChatRoomId { get; set; }
        public int GameId { get; set; }
        public int CurrentPosId { get; set; }
        public DateTime EnterTime { get; set; }
        public DateTime LastMoveTime { get; set; }
        public string ProfileImageSource
        {
            get
            {
                TopRaceAPIProxy proxy = TopRaceAPIProxy.CreateProxy();
                return proxy.GetBasePhotoUri() + "ProfileImages/" + this.Email + ".jpg";
            }
        }

        public virtual Color Color { get; set; }
        public virtual Position CurrentPos { get; set; }
        public virtual Game Game { get; set; }
        public virtual User User { get; set; }
        [JsonIgnore]
        public virtual List<Game> GameCurrentPlayerInTurns { get; set; }
        [JsonIgnore]
        public virtual List<Game> GamePreviousPlayers { get; set; }
        [JsonIgnore]
        public virtual List<Game> GameWinners { get; set; }
        [JsonIgnore]
        public virtual List<Message> Messages { get; set; }
    }
}
