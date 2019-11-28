using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using Task4.Conrols;
using Task4.Views;
using Xamarin.Forms;
using System.Collections;
using SQLite;
using System.IO;
using System.Linq;
using Task4.DataBase;
using Task4.Repository;
using Task4.Models;

namespace Task4.ViewModels
{
    public class ClockPageViewModel : ViewModelBase, INavigatedAware
    {
        private ClockPageModel Model;

        private DelegateCommand logout;
        private DelegateCommand addClock;

        public DelegateCommand AddClock =>
            addClock ?? (addClock = new DelegateCommand(ToAddClockPage));

        public DelegateCommand Logout =>
            logout ?? (logout = new DelegateCommand(ToMainpage));

        public EventHandler Tapped
        {
            get { return Model.tapped; }
            set { SetProperty(ref Model.tapped, value); }
        }

        public List<View> Children 
        {
            get { return Model.children; }
            set { SetProperty(ref Model.children, value); }
        }

        public ClockPageViewModel(INavigationService navigationService) : base(navigationService)
        {
             Title = "clock Page";
            Model = new ClockPageModel(App.FilePath);
            Tapped += OnRegionClockTapped;      
        }

        private async void OnRegionClockTapped(object sender, EventArgs args)
        {

            var p = new NavigationParameters();

            p.Add("RegionClock", (RegionClock)sender);
            
            await NavigationService.NavigateAsync("ClockSettingPage", p);
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.GetValue<bool>("AddItem"))
            {
                var temp = parameters.GetValue<RegionClock>("NewItem");
                temp.Tapped = Tapped;
                List<View> NewChildren = new List<View>();
                foreach (var regionClock in Children)
                    NewChildren.Add(regionClock);
                NewChildren.Add(temp);
                Children = NewChildren;

            }
            else if(parameters.GetValue<bool>("LoadFromDataBase"))
            {
                
                List<View> DataBaseClocks = new List<View>();
                var CurrentUserId = App.Current.Properties.Keys.LastOrDefault();
                var Data = await Model.ClockRepo.Get<RegionClockData>(x => x.UserID== CurrentUserId);

                   foreach(var RegionClockData in Data)
                      DataBaseClocks.Add(new RegionClock(RegionClockData){ Tapped=Tapped});
                  
                     Children = DataBaseClocks;
            }
            else if(parameters.GetValue<bool>("UpdateItemDataBase"))
            {
                var temp = parameters.GetValue<RegionClock>("OldItem");
                if (temp != null)
                {
                    var Index = Children.FindIndex(p => p == temp) + 1;
                    RegionClockData Data = new RegionClockData()
                    {
                        ID = Index,
                        UserID = App.Current.Properties.Keys.LastOrDefault(),

                        HandColorRed = temp.HandColor.R,
                        HandColorGreen = temp.HandColor.G,
                        HandColorBlue = temp.HandColor.B,

                        TickMarksColorRed = temp.TickMarksColor.R,
                        TickMarksColorGreen = temp.TickMarksColor.G,
                        TickMarksColorBlue = temp.TickMarksColor.B,
                        CountryText = temp.CountryText,
                        TimeOffset = temp.TimeOffset
                    };

                    await Model.ClockRepo.Update(Data);
                }
            }
                
            }
            
        private async void ToMainpage()
        {
            App.Current.Properties.Clear();
            await App.Current.SavePropertiesAsync();
            await NavigationService.NavigateAsync("/NavigationPage/MainPage");

        }

        private async void ToAddClockPage()
        { 
            var p = new NavigationParameters();
            p.Add("Addclock", true);
            
            await NavigationService.NavigateAsync("ClockSettingPage",p);

        }
    }
}

