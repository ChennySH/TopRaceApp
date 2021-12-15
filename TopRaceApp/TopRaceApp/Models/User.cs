using System;
using System.Collections.Generic;


namespace TopRaceApp.Models
{
    public partial class User
    {
        public User()
        {
            Players = new List<Player>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }

        public virtual List<Player> Players { get; set; }
    }
}
