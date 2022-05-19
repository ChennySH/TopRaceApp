﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopRaceApp.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TopRaceApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : ContentPage
    {
        public StartPage()
        {
            ((App)App.Current).SetBackgrounds(this);

            this.BindingContext = new StartPageViewModel();
            InitializeComponent();
        }
    }
}