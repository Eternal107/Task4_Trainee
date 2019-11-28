using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Task4.Conrols
{
    class CustomStackLayout:StackLayout
    {
       

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
            nameof(ItemsSource),
            typeof(List<View>),
            typeof(CustomStackLayout),
            null,
            propertyChanged: ItemAdded);


        public List<View> ItemsSource
        {
            get => (List<View>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

       public CustomStackLayout()
        {
            InnitDigitalClock();
            Device.StartTimer(TimeSpan.FromSeconds(1.0 / 60), InnitDigitalClock);
        }


        bool InnitDigitalClock()
        {
            foreach (var n in Children)
            {
                if (n is RegionClock)
                {
                    string TimeZoneId;
                    var RegionClock = (RegionClock)n;
                    if (RegionClock.TimeOffset <= 0)
                        TimeZoneId = "Etc/GMT+" + -RegionClock.TimeOffset;
                    else TimeZoneId = "Etc/GMT-" + RegionClock.TimeOffset;
                    DateTime timeUtc = DateTime.UtcNow;
                    try
                    {
                        TimeZoneInfo Zone = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId);

                        RegionClock.digitalClock.Text = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, Zone).ToString("HH:mm:ss");

                    }
                    catch (TimeZoneNotFoundException)
                    {
                        RegionClock.digitalClock.Text = "Wrong Time Zone";
                    }
                }
            }
            return true;
        }


        private static void ItemAdded(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as CustomStackLayout;    
            if (newValue != null)
            {             
                control.Children.Clear();
                foreach (var n in (List<View>)newValue)
                    control.Children.Add(n);
            }
        }
    
}
}
