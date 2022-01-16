using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TopRaceApp.Views;
using TopRaceApp.Models;

namespace TopRaceApp
{
    public partial class App : Application
    {
        public static bool IsDevEnv
        {
            get
            {
                return true; //change this before release!
            }
        }

        //The current logged in user
        public User currentUser { get; set; }
        public Player currentPlayer { get; set; }
        public Game currentGame { get; set; }
        public App()
        {
            InitializeComponent();
            this.currentUser = null;
            this.currentPlayer = null;
            this.currentGame = null;
            MainPage = new StartPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
