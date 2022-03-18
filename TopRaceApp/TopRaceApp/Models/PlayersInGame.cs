using System;
using System.Collections.Generic;
using TopRaceApp.DTOs;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace TopRaceApp.Models
{
    public partial class PlayersInGame
    {
        public PlayersInGame()
        {
            GameCurrentPlayerInTurns = new List<GameDTO>();
            GamePreviousPlayers = new List<GameDTO>();
            GameWinners = new List<GameDTO>();
            Messages = new List<Message>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
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

        public virtual Color Color { get; set; }
        public virtual Position CurrentPos { get; set; }
        public virtual Game Game { get; set; }
        public virtual User User { get; set; }
        [JsonIgnore]
        public virtual List<GameDTO> GameCurrentPlayerInTurns { get; set; }
        [JsonIgnore]
        public virtual List<GameDTO> GamePreviousPlayers { get; set; }
        [JsonIgnore]
        public virtual List<GameDTO> GameWinners { get; set; }
        public virtual List<Message> Messages { get; set; }
    }
}
