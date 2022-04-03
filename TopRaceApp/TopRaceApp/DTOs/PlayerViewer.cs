using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopRaceApp.Models;

namespace TopRaceApp.DTOs
{
    class PlayerViewer
    {
        public string UserName { get; set; }
        public string ProfilePic { get; set; }
        public bool IsHost { get; set; }
        public int ColorId { get; set; }
        public virtual Color Color { get; set; }
        public string BackGroundColorCode { get; set; }
        public PlayerViewer(PlayersInGame pl, bool IsYou)
        {
            this.UserName = pl.UserName;
            this.ProfilePic = pl.ProfilePic;
            this.IsHost = pl.IsHost;
            this.ColorId = pl.ColorId;
            this.Color = pl.Color;
            if (IsYou)
            {
                BackGroundColorCode = "#FF808080";
            }
            else
            {
                BackGroundColorCode = "#FFFFFFFF";
            }
        }
    }
}
