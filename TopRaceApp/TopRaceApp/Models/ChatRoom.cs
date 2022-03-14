using System;
using System.Collections.Generic;
using TopRaceApp.DTOs;

namespace TopRaceApp.Models
{
    public partial class ChatRoom
    {
        public ChatRoom()
        {

            Games = new List<GameDTO>();
            Messages = new List<Message>();
            PlayersInGames = new List<PlayersInGame>();
        }

        public int Id { get; set; }

        public virtual List<GameDTO> Games { get; set; }
        public virtual List<Message> Messages { get; set; }
        public virtual List<PlayersInGame> PlayersInGames { get; set; }
    }
}
