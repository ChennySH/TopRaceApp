using System;
using System.Collections.Generic;


namespace TopRaceApp.Models
{
    public partial class Game
    {
        public Game()
        {
            MoversInGames = new List<MoversInGame>();
            PlayersInGames = new List<PlayersInGame>();
        }

        public int Id { get; set; }
        public string GameName { get; set; }
        public bool IsPrivate { get; set; }
        public string PrivateKey { get; set; }
        public int HostUserId { get; set; }
        public int ChatRoomId { get; set; }
        public int StatusId { get; set; }
        public DateTime LastUpdateTime { get; set; }

        public virtual ChatRoom ChatRoom { get; set; }
        public virtual User HostUser { get; set; }
        public virtual GameStatus Status { get; set; }
        public virtual List<MoversInGame> MoversInGames { get; set; }
        public virtual List<PlayersInGame> PlayersInGames { get; set; }
    }
}
