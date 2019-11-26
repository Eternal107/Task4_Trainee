using System;
using Task4.Controls;
using Xamarin.Forms;

namespace Task4.Conrols
{
    public class RegionClock : Grid
    {
       
        public static readonly BindableProperty HandColorProperty =
            BindableProperty.Create(nameof(HandColor), typeof(Color), typeof(RegionClock), Color.Black, propertyChanged: (bindable, oldValue, newValue) =>
            {
                var regionClock = (RegionClock)bindable;
                regionClock.clock.SetColors((Color)newValue, regionClock.TickMarksColor);
            });

        public Color HandColor
        {
            set { SetValue(HandColorProperty, value); }
            get { return (Color)GetValue(HandColorProperty); }
        }

        public static readonly BindableProperty TickMarksColorProperty =
            BindableProperty.Create(nameof(TickMarksColor), typeof(Color), typeof(RegionClock), Color.Black, propertyChanged: (bindable, oldValue, newValue) =>
            {
                var regionClock = (RegionClock)bindable;
                regionClock.clock.SetColors(regionClock.HandColor, (Color)newValue);
            });

        public Color TickMarksColor
        {
            set { SetValue(TickMarksColorProperty, value); }
            get { return (Color)GetValue(TickMarksColorProperty); }
        }


        public static readonly BindableProperty CountryTextProperty =
           BindableProperty.Create(nameof(CountryText), typeof(string), typeof(RegionClock), "Country", propertyChanged: (bindable, oldValue, newValue) =>
           {
               var regionClock = (RegionClock)bindable;
               regionClock.country.Text = (string)newValue;
           });

        public string CountryText
        {
            set { SetValue(CountryTextProperty, value); }
            get { return (string)GetValue(CountryTextProperty); }
        }


        public static readonly BindableProperty TimeOffsetProperty =
            BindableProperty.Create(nameof(TimeOffset), typeof(int), typeof(RegionClock), 0, propertyChanged: (bindable, oldValue, newValue) =>
            {
                var regionClock = (RegionClock)bindable;
                
                regionClock.clock.TimeOffset = (int)newValue;

            });

        public int TimeOffset
        {
            set { SetValue(TimeOffsetProperty, value); }
            get { return (int)GetValue(TimeOffsetProperty); }
        }

       
        public static readonly BindableProperty TappedProperty =
             BindableProperty.Create(nameof(Tapped), typeof(EventHandler), typeof(RegionClock), null, propertyChanged: (bindable, oldValue, newValue) =>
             {
                 var regionClock = (RegionClock)bindable;
                 
                 regionClock.Tap.Tapped += (sender, e) =>
                 {
                    (newValue as EventHandler)?.Invoke(sender, e);
                 };
             });


        public EventHandler Tapped
        {
            set { SetValue(TappedProperty, value); }
            get { return (EventHandler)GetValue(TappedProperty); }
        }

        

        private TapGestureRecognizer Tap=new TapGestureRecognizer();
        public Clock clock = new Clock();
        public Label country = new Label();
        public Label digitalClock = new Label();

        void InnitGrid()
        {

            ColumnSpacing = 3;
            RowSpacing = 3;
            
            RowDefinitions.Add(new RowDefinition { Height = new GridLength(25) });
            RowDefinitions.Add(new RowDefinition { Height = new GridLength(25) });

            ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

        }

        public RegionClock()
        {            
            GestureRecognizers.Add(Tap);
            country.Text = CountryText;
            Innit();
        }

        public RegionClock(RegionClockData data)
        {
            GestureRecognizers.Add(Tap);

            Innit();
            CountryText = data.CountryText;
            country.Text = CountryText;
            TimeOffset = data.TimeOffset;
            HandColor = new Color(data.HandColorRed, data.HandColorGreen, data.HandColorBlue);
            TickMarksColor = new Color(data.TickMarksColorRed, data.TickMarksColorGreen, data.TickMarksColorBlue);
        }
        void Innit()
        {
            InnitGrid();
            
            Children.Add(clock, 0, 0);
            SetRowSpan(clock, 2);
            Children.Add(digitalClock, 1, 1);
            Children.Add(country, 1, 0);    
           
        }
     
    }
}
