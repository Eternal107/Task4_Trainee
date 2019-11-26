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

namespace Task4.ViewModels
{
    public class ClockPageViewModel : ViewModelBase, INavigatedAware
    {
       
        private DelegateCommand addClock;

        public DelegateCommand AddClock =>
       addClock ?? (addClock = new DelegateCommand(ToAddClockPage));

        private DelegateCommand logout;

        public DelegateCommand Logout =>
       logout ?? (logout = new DelegateCommand(ToMainpage));

        private EventHandler tapped;

        public EventHandler Tapped
        {
            get { return tapped; }
            set { SetProperty(ref tapped, value); }
        }

        private List<View> children=new List<View>();

        public List<View> Children
        {
            get { return children; }
            set { SetProperty(ref children, value); }
        }

        public ClockPageViewModel(INavigationService navigationService) : base(navigationService)
        {
             Title = "clock Page";
             Tapped += OnRegionClockTapped;
        }

        private async void OnRegionClockTapped(object sender, EventArgs args)
        {
            var p = new NavigationParameters();

            p.Add("RegionClock", (RegionClock)sender);
            
            await NavigationService.NavigateAsync("ClockSettingPage", p);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
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
                using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                {
                    conn.CreateTable<RegionClockData>();

                    List<View> DataBaseClocks = new List<View>();

                    var Data = conn.Table<RegionClockData>().ToList();
                   foreach(var RegionClockData in Data)
                      DataBaseClocks.Add(new RegionClock(RegionClockData){ Tapped=Tapped});
                  
                     Children = DataBaseClocks;
                }
                
            }
            
        }

        private async void ToMainpage()
        {
            App.Current.Properties.Clear();
            
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

