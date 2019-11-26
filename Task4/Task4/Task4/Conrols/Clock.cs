using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Task4.Controls
{
    public class Clock : AbsoluteLayout
    {



        public static readonly BindableProperty HandColorProperty =
             BindableProperty.Create(nameof(HandColor), typeof(Color), typeof(Clock), Color.Black, propertyChanged: (bindable, oldValue, newValue) =>
             {
                 var Clock = (Clock)bindable;
                 Clock.SetColors((Color)newValue, Clock.TickMarksColor);
             });

        public Color HandColor
        {
            set { SetValue(HandColorProperty, value); }
            get { return (Color)GetValue(HandColorProperty); }
        }

        public static readonly BindableProperty TickMarksColorProperty =
            BindableProperty.Create(nameof(TickMarksColor), typeof(Color), typeof(Clock), Color.Black, propertyChanged: (bindable, oldValue, newValue) =>
            {
                var Clock = (Clock)bindable;
                Clock.SetColors(Clock.HandColor, (Color)newValue);
            });

        public Color TickMarksColor
        {
            set { SetValue(TickMarksColorProperty, value); }
            get { return (Color)GetValue(TickMarksColorProperty); }
        }

        public static readonly BindableProperty TimeOffsetProperty =
           BindableProperty.Create(nameof(TimeOffset), typeof(int), typeof(Clock), 0, propertyChanged: (bindable, oldValue, newValue) =>
           {
               var Clock = (Clock)bindable;
               Clock.OnTimerTick();
              

           });

        public int TimeOffset
        {
            set { SetValue(TimeOffsetProperty, value); }
            get { return (int)GetValue(TimeOffsetProperty); }
        }

        struct HandParams
        {
            public HandParams(double width, double height, double offset) : this()
            {
                Width = width;
                Height = height;
                Offset = offset;
            }

            public double Width { private set; get; }   
            public double Height { private set; get; }  
            public double Offset { private set; get; }  
        }

        static readonly HandParams secondParams = new HandParams(0.02, 1.1, 0.85);
        static readonly HandParams minuteParams = new HandParams(0.05, 0.8, 0.9);
        static readonly HandParams hourParams = new HandParams(0.125, 0.65, 0.9);

        BoxView[] tickMarks = new BoxView[60];
        BoxView secondHand;
        BoxView minuteHand;
        BoxView hourHand;


        public void SetColors(Color HandColor, Color TickMarksColor)
        {
            secondHand.Color = HandColor;
            minuteHand.Color = HandColor;
            hourHand.Color = HandColor;
            for (int i = 0; i < tickMarks.Length; i++)
                tickMarks[i].Color = TickMarksColor;
        }





        public Clock()
        {

            this.secondHand = new BoxView() { Color = Color.Black };
            this.minuteHand = new BoxView() { Color = Color.Black };
            this.hourHand = new BoxView() { Color = Color.Black };

            Children.Add(secondHand);
            Children.Add(minuteHand);
            Children.Add(hourHand);
            this.SizeChanged += OnthisSizeChanged;
         
            for (int i = 0; i < tickMarks.Length; i++)
            {
                tickMarks[i] = new BoxView() { Color = Color.Black };
                this.Children.Add(tickMarks[i]);
            }

            Device.StartTimer(TimeSpan.FromSeconds(1.0 / 60), OnTimerTick);

        }

        public void OnthisSizeChanged(object sender, EventArgs args)
        {
           
            Point center = new Point(this.Width / 2, this.Height / 2);
            double radius = 0.45 * Math.Min(this.Width, this.Height);

            
            for (int index = 0; index < tickMarks.Length; index++)
            {
                double size = radius / (index % 5 == 0 ? 15 : 30);
                double radians = index * 2 * Math.PI / tickMarks.Length;
                double x = center.X + radius * Math.Sin(radians) - size / 2;
                double y = center.Y - radius * Math.Cos(radians) - size / 2;
                AbsoluteLayout.SetLayoutBounds(tickMarks[index], new Rectangle(x, y, size, size));
                tickMarks[index].Rotation = 180 * radians / Math.PI;
            }

            
            LayoutHand(secondHand, secondParams, center, radius);
            LayoutHand(minuteHand, minuteParams, center, radius);
            LayoutHand(hourHand, hourParams, center, radius);
        }

        void LayoutHand(BoxView boxView, HandParams handParams, Point center, double radius)
        {
            double width = handParams.Width * radius;
            double height = handParams.Height * radius;
            double offset = handParams.Offset;

            AbsoluteLayout.SetLayoutBounds(boxView,
                new Rectangle(center.X - 0.5 * width,
                              center.Y - offset * height,
                              width, height));

            // Set the AnchorY property for rotations.
            boxView.AnchorY = handParams.Offset;
        }

        bool OnTimerTick()
        {

            string TimeZoneId;
            if (TimeOffset <= 0)
                TimeZoneId = "Etc/GMT+" + -TimeOffset;
            else TimeZoneId = "Etc/GMT-" + TimeOffset;
            DateTime dateTime = DateTime.UtcNow;
            try
            {
                TimeZoneInfo Zone = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId);

                dateTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime, Zone);

            }
            catch (TimeZoneNotFoundException)
            {
                dateTime = DateTime.UtcNow;
            }


            hourHand.Rotation = 30 * (dateTime.Hour % 12) + 0.5 * dateTime.Minute;
            minuteHand.Rotation = 6 * dateTime.Minute + 0.1 * dateTime.Second;

            // Do an animation for the second hand.
            double t = dateTime.Millisecond / 1000.0;

            if (t < 0.5)
            {
                t = 0.5 * Easing.SpringIn.Ease(t / 0.5);
            }
            else
            {
                t = 0.5 * (1 + Easing.SpringOut.Ease((t - 0.5) / 0.5));
            }

            secondHand.Rotation = 6 * (dateTime.Second + t);
            return true;
        }
    }
}