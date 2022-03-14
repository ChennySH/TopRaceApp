using System;
using System.Collections.Generic;
using TopRaceApp.DTOs;

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

        public virtual List<GameDTO> Games { get; set; }
    }
}
