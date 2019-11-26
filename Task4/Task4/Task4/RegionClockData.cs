using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Task4
{
    public class RegionClockData
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public double HandColorRed { get; set; }
        public double HandColorGreen { get; set; }
        public double HandColorBlue { get; set; }

        public double TickMarksColorRed { get; set; }
        public double TickMarksColorGreen { get; set; }
        public double TickMarksColorBlue { get; set; }

        public string CountryText { get; set; }
        public int TimeOffset { get; set; }

        

    }
}
