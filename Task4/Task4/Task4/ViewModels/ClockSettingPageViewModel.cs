using Prism.Commands;
using Prism.Navigation;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task4.Conrols;
using Task4.Models;
using Task4.Repository;
using Xamarin.Forms;

namespace Task4.ViewModels
{
    public class ClockSettingPageViewModel : ViewModelBase, INavigationAware
    {

        ClockSettingPageModel Model;

        private bool AddItem=false;


        private DelegateCommand sliderValueChanged;

        public DelegateCommand SliderValueChanged =>
            sliderValueChanged ?? (sliderValueChanged = new DelegateCommand(OnSliderValueChanged));

        public DelegateCommand Savevalue=> Model.savevalue??(Model.savevalue=new DelegateCommand(Model.SaveToDataBase));


        private bool isVisible;

        public bool IsVisible
        {
            get { return isVisible; }
            set { SetProperty(ref isVisible, value); }
        }

        public string Country
        {
            get { return Model.country; }
            set { SetProperty(ref Model.country, value); }
        }

        public int TimeOffset
        {
            get { return Model.timeOffset; }
            set { SetProperty(ref Model.timeOffset, value); }
        }


        public Color HandColor
        {
            get { return Model._HandColor; }
            set { SetProperty(ref Model._HandColor, value); }
        }


        public Color TickMarksColor
        {
            get { return Model._TickMarksColor; }
            set { SetProperty(ref Model._TickMarksColor, value); }
        }

        
        public double RedHandChannel
        {
            get { return Model.redHandChannel; }
            set { SetProperty(ref Model.redHandChannel, value); }
        }

      
        public double GreenHandChannel
        {
            get { return Model.greenHandChannel; }
            set { SetProperty(ref Model.greenHandChannel, value); }
        }

        
        public double BlueHandChannel
        {
            get { return Model.blueHandChannel; }
            set { SetProperty(ref Model.blueHandChannel, value); }
        }

        
        public double RedTickChannel
        {
            get { return Model.redTickChannel; }
            set { SetProperty(ref Model.redTickChannel, value); }
        }

        
        public double GreenTickChannel
        {
            get { return Model.greenTickChannel; }
            set { SetProperty(ref Model.greenTickChannel, value); }
        }

       
        public double BlueTickChannel
        {
            get { return Model.blueTickChannel; }
            set { SetProperty(ref Model.blueTickChannel, value); }
        }


        public ClockSettingPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "clock Setting Page";
            Model=new ClockSettingPageModel(App.FilePath);

        }


        protected void OnSliderValueChanged()
        {       
            HandColor = new Color(RedHandChannel, GreenHandChannel, BlueHandChannel);
            TickMarksColor = new Color(RedTickChannel, GreenTickChannel, BlueTickChannel);

        }

        void InnitSliders()
        {

            RedHandChannel = Model.RegionClock.HandColor.R;
            GreenHandChannel = Model.RegionClock.HandColor.G;
            BlueHandChannel = Model.RegionClock.HandColor.B;

            RedTickChannel = Model.RegionClock.TickMarksColor.R;
            GreenTickChannel = Model.RegionClock.TickMarksColor.G;
            BlueTickChannel = Model.RegionClock.TickMarksColor.B;

        }

        public override async void OnNavigatedFrom(INavigationParameters parameters)
        {
            if (AddItem && Model.IsSaved)
            {
                var CurrentUserId = App.Current.Properties.Keys.LastOrDefault();
                var RegionClockData =  (await Model.ClockRepo.Get<RegionClockData>(x => x.UserID == CurrentUserId)).LastOrDefault();

                Model.RegionClock = new RegionClock(RegionClockData);

                parameters.Add("AddItem", true);
                parameters.Add("NewItem", Model.RegionClock);
            }
            else
            {
                
                Model.RegionClock.HandColor = new Color(RedHandChannel, GreenHandChannel, BlueHandChannel);
                Model.RegionClock.TickMarksColor = new Color(RedTickChannel, GreenTickChannel, BlueTickChannel);
                Model.RegionClock.CountryText = Country;
                Model.RegionClock.TimeOffset = TimeOffset;
                parameters.Add("UpdateItemDataBase", true);
                parameters.Add("OldItem", Model.RegionClock);
            }

        }


        public override void OnNavigatedTo(INavigationParameters parameters)
        {

            if (parameters.GetValue<bool>("Addclock")==true)
            {
                HandColor = Color.Black;
                TickMarksColor = Color.Black;
                Country = "Country";
                AddItem = true;
                IsVisible = true;
                InnitSliders();
            }
            else
            {
                IsVisible = false;
                
                Model.RegionClock = parameters.GetValue<RegionClock>("RegionClock");
                Country = Model.RegionClock.CountryText;
                TimeOffset = Model.RegionClock.TimeOffset;
                HandColor = Model.RegionClock.HandColor;
                TickMarksColor = Model.RegionClock.TickMarksColor;
                InnitSliders();
            }

        }
    }
}