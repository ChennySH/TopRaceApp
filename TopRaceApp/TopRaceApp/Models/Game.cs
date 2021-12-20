using System;
using System.Collections.Generic;


namespace TopRaceApp.Models
{
    public partial class Game
    {
        public Game()
        {
            PlayersInGames = new List<PlayersInGame>();
        }

        public int Id { get; set; }
        public bool IsPrivate { get; set; }
        public int PrivateKey { get; set; }
        public int HostPlayerId { get; set; }
        public int CurrentTurn { get; set; }
        public int Players { get; set; }
        public int ChatRoomId { get; set; }
        public int StatusId { get; set; }

        public virtual ChatRoom ChatRoom { get; set; }
        public virtual GameStatus Status { get; set; }
        public virtual List<PlayersInGame> PlayersInGames { get; set; }
    }
}
