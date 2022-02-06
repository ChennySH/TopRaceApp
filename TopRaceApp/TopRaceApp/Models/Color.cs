using System;
using System.Collections.Generic;
using TopRaceApp.Services;



namespace TopRaceApp.Models
{
    public partial class Color
    {
        public Color()
        {
            PlayersInGames = new List<PlayersInGame>();
            
        }

        public int Id { get; set; }
        public string ColorName { get; set; }
        public string ColorCode { get; set; }
        public string PicLink { get; set; }

        public virtual List<PlayersInGame> PlayersInGames { get; set; }
    }
}
