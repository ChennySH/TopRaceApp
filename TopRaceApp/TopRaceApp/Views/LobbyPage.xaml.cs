using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopRaceApp.ViewModels;
using TopRaceApp.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TopRaceApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LobbyPage : ContentPage
    {
        public LobbyPage()
        {
            ((App)App.Current).SetBackgrounds(this);

            InitializeComponent();
        }
        public void SetEvent()
        {
            ((LobbyPageViewModel)this.BindingContext).ScrollToButton += ScrollToButton;
        }
        public void ScrollToButton(int index)
        {
            ChatCollectionView.ScrollTo(index, position: ScrollToPosition.MakeVisible);
        }

        
    }
}