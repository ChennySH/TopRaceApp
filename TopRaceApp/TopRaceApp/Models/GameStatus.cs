using System;
using System.Collections.Generic;


namespace TopRaceApp.Models
{
    public partial class GameStatus
    {
        public GameStatus()
        {
            Games = new List<Game>();
        }

        public int Id { get; set; }
        public string StatusName { get; set; }

        public virtual List<Game> Games { get; set; }
    }
}
