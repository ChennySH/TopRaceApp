using System;
using System.Collections.Generic;


namespace TopRaceApp.Models
{
    public partial class Message
    {
        public int Id { get; set; }
        public int FromId { get; set; }
        public string Message1 { get; set; }
        public int GameId { get; set; }
        public DateTime TimeSent { get; set; }

        public virtual PlayersInGame From { get; set; }
        public virtual Game Game { get; set; }
    }
}
