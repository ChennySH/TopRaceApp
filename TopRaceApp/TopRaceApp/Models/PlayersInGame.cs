using System;
using System.Collections.Generic;


namespace TopRaceApp.Models
{
    public partial class PlayersInGame
    {
        public PlayersInGame()
        {
            Messages = new List<Message>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string ProfilePic { get; set; }
        public bool IsHost { get; set; }
        public int Number { get; set; }
        public int ColorId { get; set; }
        public int ChatRoomId { get; set; }
        public int GameId { get; set; }
        public int CurrentPosId { get; set; }
        public DateTime LastMoveTime { get; set; }

        public virtual ChatRoom ChatRoom { get; set; }
        public virtual Color Color { get; set; }
        public virtual Position CurrentPos { get; set; }
        public virtual Game Game { get; set; }
        public virtual User User { get; set; }
        public virtual List<Message> Messages { get; set; }
    }
}
