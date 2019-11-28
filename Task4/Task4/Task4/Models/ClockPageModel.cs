using Prism.Commands;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Task4.Repository;
using Xamarin.Forms;

namespace Task4.Models
{
   public class ClockPageModel
    {
        public Repository<RegionClockData> ClockRepo { get; private set; }

        public EventHandler tapped;

        public List<View> children = new List<View>();

        public ClockPageModel(string FilePath)
        {
            var connection = new SQLiteAsyncConnection(FilePath);

            ClockRepo = new Repository<RegionClockData>(connection);

        }
    }
}
