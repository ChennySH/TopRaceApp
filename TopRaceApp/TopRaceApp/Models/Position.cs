using System;
using System.Collections.Generic;


namespace TopRaceApp.Models
{
    public partial class Position
    {
        public Position()
        {
            MoversInGameEndPos = new List<MoversInGame>();
            MoversInGameStartPos = new List<MoversInGame>();
            PlayersInGames = new List<PlayersInGame>();
        }

        public int Id { get; set; }
        public string String { get; set; }

        public virtual List<MoversInGame> MoversInGameEndPos { get; set; }
        public virtual List<MoversInGame> MoversInGameStartPos { get; set; }
        public virtual List<PlayersInGame> PlayersInGames { get; set; }
    }
}
