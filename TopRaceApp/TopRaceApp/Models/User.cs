using System;
using System.Collections.Generic;
using Xamarin.Forms;
using TopRaceApp.Services;

namespace TopRaceApp.Models
{
    public partial class User
    {
        public User()
        {
            PlayersInGames = new List<PlayersInGame>();
        }
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public int WinsNumber { get; set; }
        public int LosesNumber { get; set; }
        public int WinsStreak { get; set; }
        public string ProfilePic { get; set; }
        public string ProfileImageSource
        {
            get
            {
                TopRaceAPIProxy proxy = TopRaceAPIProxy.CreateProxy();
                return proxy.GetBasePhotoUri() + "ProfileImages/" + this.Email + ".jpg";
            }
        }
        public virtual List<PlayersInGame> PlayersInGames { get; set; }
    }
}
