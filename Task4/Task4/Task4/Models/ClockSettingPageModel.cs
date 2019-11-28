using Prism.Commands;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task4.Conrols;
using Task4.Repository;
using Xamarin.Forms;

namespace Task4.Models
{
   public class ClockSettingPageModel
    {
        public Repository<RegionClockData> ClockRepo { get; private set; }

        public bool AddItem = false;
        public bool IsSaved = false;

        public DelegateCommand sliderValueChanged;
        public DelegateCommand savevalue;

        public bool isVisible;

        public string country;
        public int timeOffset;

        public Color _HandColor;
        public Color _TickMarksColor;

        public double redHandChannel;
        public double greenHandChannel;
        public double blueHandChannel;

        public double redTickChannel;
        public double greenTickChannel;
        public double blueTickChannel;

        public RegionClock RegionClock = new RegionClock();

        public ClockSettingPageModel(string FilePath)
        {
            var connection = new SQLiteAsyncConnection(FilePath);

            ClockRepo = new Repository<RegionClockData>(connection);

        }

        public async void SaveToDataBase()
        {

            RegionClockData Data = new RegionClockData();
            Data.CountryText = country;
            Data.TimeOffset = timeOffset;
            Data.HandColorRed = _HandColor.R;
            Data.HandColorGreen = _HandColor.G;
            Data.HandColorBlue = _HandColor.B;

            Data.TickMarksColorRed = _TickMarksColor.R;
            Data.TickMarksColorGreen = _TickMarksColor.G;
            Data.TickMarksColorBlue = _TickMarksColor.B;
            Data.UserID = App.Current.Properties.Keys.LastOrDefault();
            await ClockRepo.Insert(Data);

            IsSaved = true;

        }

       

    }
}
