using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace TopRaceApp.Models
{
    public partial class Position
    {
        public Position()
        {
            MoverEndPos = new List<Mover>();
            MoverNextPos = new List<Mover>();
            MoverStartPos = new List<Mover>();
            PlayersInGames = new List<PlayersInGame>();
        }

        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        [JsonIgnore]
        public virtual List<Mover> MoverEndPos { get; set; }
        [JsonIgnore]
        public virtual List<Mover> MoverNextPos { get; set; }
        [JsonIgnore]
        public virtual List<Mover> MoverStartPos { get; set; }
        [JsonIgnore]
        public virtual List<PlayersInGame> PlayersInGames { get; set; }
    }
}
