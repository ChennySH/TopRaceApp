using System;
using System.Collections.Generic;
using TopRaceApp.DTOs;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TopRaceApp.Models
{
    public partial class GameStatus
    {
        public GameStatus()
        {
            Games = new List<GameDTO>();
        }

        public int Id { get; set; }
        public string StatusName { get; set; }

        [JsonIgnore]
        public virtual List<GameDTO> Games { get; set; }
    }
}
