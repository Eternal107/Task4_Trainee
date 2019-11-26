using Prism.Commands;
using Prism.Navigation;
using SQLite;
using System;
using System.Collections.Generic;

using System.Text;
using Task4.Conrols;
using Xamarin.Forms;

namespace Task4.ViewModels
{
    public class ClockSettingPageViewModel : ViewModelBase, INavigationAware
    {

        private bool AddItem=false;

        private bool IsSaved = false;

        

        RegionClock RegionClock=new RegionClock();
        
        private DelegateCommand sliderValueChanged;

        public DelegateCommand SliderValueChanged =>
            sliderValueChanged ?? (sliderValueChanged = new DelegateCommand(OnSliderValueChanged));

        private DelegateCommand savevalue;

        public DelegateCommand Savevalue =>
            savevalue ?? (savevalue = new DelegateCommand(()=> { IsSaved = true; }));


        private bool isVisible;

        public bool IsVisible
        {
            get { return isVisible; }
            set { SetProperty(ref isVisible, value); }
        }


        private string country;

        public string Country
        {
            get { return country; }
            set { SetProperty(ref country, value); }
        }


        private int timeOffset;

        public int TimeOffset
        {
            get { return timeOffset; }
            set { SetProperty(ref timeOffset, value); }
        }



        private Color _HandColor;
        private Color _TickMarksColor;

        public Color HandColor
        {
            get { return _HandColor; }
            set { SetProperty(ref _HandColor, value); }
        }


        public Color TickMarksColor
        {
            get { return _TickMarksColor; }
            set { SetProperty(ref _TickMarksColor, value); }
        }

        private double redHandChannel;
        public double RedHandChannel
        {
            get { return redHandChannel; }
            set { SetProperty(ref redHandChannel, value); }
        }

        private double greenHandChannel;
        public double GreenHandChannel
        {
            get { return greenHandChannel; }
            set { SetProperty(ref greenHandChannel, value); }
        }

        private double blueHandChannel;
        public double BlueHandChannel
        {
            get { return blueHandChannel; }
            set { SetProperty(ref blueHandChannel, value); }
        }

        private double redTickChannel;
        public double RedTickChannel
        {
            get { return redTickChannel; }
            set { SetProperty(ref redTickChannel, value); }
        }

        private double greenTickChannel;
        public double GreenTickChannel
        {
            get { return greenTickChannel; }
            set { SetProperty(ref greenTickChannel, value); }
        }

        private double blueTickChannel;
        public double BlueTickChannel
        {
            get { return blueTickChannel; }
            set { SetProperty(ref blueTickChannel, value); }
        }


        public ClockSettingPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "clock Setting Page";

        }


        protected void OnSliderValueChanged()
        {
          
            HandColor = new Color(RedHandChannel, GreenHandChannel, BlueHandChannel);
            TickMarksColor = new Color(RedTickChannel, GreenTickChannel, BlueTickChannel);


        }

        private void SaveToDataBase()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<RegionClockData>();

                RegionClockData Data = new RegionClockData();
                Data.CountryText = Country;
                Data.TimeOffset = TimeOffset;
                Data.HandColorRed = HandColor.R;
                Data.HandColorGreen = HandColor.G;
                Data.HandColorBlue = HandColor.B;

                Data.TickMarksColorRed = TickMarksColor.R;
                Data.TickMarksColorGreen = TickMarksColor.G;
                Data.TickMarksColorBlue = TickMarksColor.B;
                conn.Insert(Data);
               
            }
        }

        void InnitSliders()
        {

            RedHandChannel = RegionClock.HandColor.R;
            GreenHandChannel = RegionClock.HandColor.G;
            BlueHandChannel = RegionClock.HandColor.B;

            RedTickChannel = RegionClock.TickMarksColor.R;
            GreenTickChannel = RegionClock.TickMarksColor.G;
            BlueTickChannel = RegionClock.TickMarksColor.B;

        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            if (AddItem && IsSaved)
            {
                SaveToDataBase();
                RegionClock.HandColor = new Color(RedHandChannel, GreenHandChannel, BlueHandChannel);
                RegionClock.TickMarksColor = new Color(RedTickChannel, GreenTickChannel, BlueTickChannel);
                RegionClock.CountryText = Country;
                RegionClock.TimeOffset = TimeOffset;
                
                parameters.Add("AddItem", true);
                parameters.Add("NewItem", RegionClock);
            }
            else
            {
                RegionClock.HandColor = new Color(RedHandChannel, GreenHandChannel, BlueHandChannel);
                RegionClock.TickMarksColor = new Color(RedTickChannel, GreenTickChannel, BlueTickChannel);
                RegionClock.CountryText = Country;
                RegionClock.TimeOffset = TimeOffset;
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
                RegionClock = parameters.GetValue<RegionClock>("RegionClock");
                Country = RegionClock.CountryText;
                TimeOffset = RegionClock.TimeOffset;
                HandColor = RegionClock.HandColor;
                TickMarksColor = RegionClock.TickMarksColor;
                InnitSliders();
            }

        }
    }
}