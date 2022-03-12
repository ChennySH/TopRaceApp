using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TopRaceApp.Views;
using TopRaceApp.Models;
using TopRaceApp.ViewModels;
using TopRaceApp.Services;

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
        public PlayersInGame currentPlayerInGame { get; set; }
        public Game currentGame { get; set; }
        public List<Models.Color> GameColors { get; set; }
        public App()
        {
            InitializeComponent();
            this.currentUser = null;
            this.currentPlayerInGame = null;
            this.currentGame = null;
            MainPage = new NavigationPage(new GamePage());
        }

        protected override async void OnStart()
        {
            TopRaceAPIProxy proxy = TopRaceAPIProxy.CreateProxy();
            this.GameColors = await proxy.GetAllColorsAsync();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
