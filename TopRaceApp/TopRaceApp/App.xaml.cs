﻿using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TopRaceApp.Views;
using TopRaceApp.Models;
using TopRaceApp.ViewModels;
using TopRaceApp.Services;
using TopRaceApp.DTOs;

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
        public GameDTO currentGame { get; set; }
        public List<Models.Color> GameColors { get; set; }
        public List<Position> Positions { get; set; }
        public App()
        {
            InitializeComponent();
            this.currentUser = null;
            this.currentPlayerInGame = null;
            this.currentGame = null;
            MainPage = new NavigationPage(new StartPage());
            

        }
        public void SetBackgrounds(Page page)
        {
            TopRaceAPIProxy proxy = TopRaceAPIProxy.CreateProxy();
            if (Device.RuntimePlatform == Device.UWP)
            {
                page.BackgroundImageSource = proxy.GetBasePhotoUri() + "JungleBackground.jpg";
            }
            else
            {
                page.BackgroundImageSource = proxy.GetBasePhotoUri() + "JungleBackground.jpg";
            }
        }
        protected override async void OnStart()
        {
            TopRaceAPIProxy proxy = TopRaceAPIProxy.CreateProxy();
            this.GameColors = await proxy.GetAllColorsAsync();
            this.Positions = await proxy.GetAllPositionssAsync();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
