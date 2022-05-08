using System;
using System.Collections.Generic;


namespace TopRaceApp.Models
{
    public partial class Game
    {
        public Game()
        {
            Messages = new List<Message>();
            PlayersInGames = new List<PlayersInGame>();
        }

        public int Id { get; set; }
        public string GameName { get; set; }
        public bool IsPrivate { get; set; }
        public string PrivateKey { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public int UpdatesCounter { get; set; }
        public int MovesCounter { get; set; }
        public string Board { get; set; }
        public int StatusId { get; set; }
        public int? WinnerId { get; set; }
        public int? CurrentPlayerInTurnId { get; set; }
        public int? PreviousPlayerId { get; set; }
        public int LastRollResult { get; set; }

        public virtual PlayersInGame CurrentPlayerInTurn { get; set; }
        public virtual PlayersInGame PreviousPlayer { get; set; }
        public virtual GameStatus Status { get; set; }
        public virtual PlayersInGame Winner { get; set; }
        public virtual List<Message> Messages { get; set; }
        public virtual List<PlayersInGame> PlayersInGames { get; set; }
    }
}
