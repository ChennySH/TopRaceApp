using System;
using System.Collections.Generic;


namespace TopRaceApp.Models
{
    public partial class Player
    {
        public Player()
        {
            PlayersInGames = new List<PlayersInGame>();
            Users = new List<User>();
        }

        public int Id { get; set; }
        public string PlayerName { get; set; }
        public int WinsNumber { get; set; }
        public int LosesNumber { get; set; }
        public int WinStreak { get; set; }
        public string ProfilePic { get; set; }

        public virtual List<PlayersInGame> PlayersInGames { get; set; }
        public virtual List<User> Users { get; set; }
    }
}
