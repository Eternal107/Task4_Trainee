using Prism;
using Prism.Ioc;
using Prism.Navigation;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Task4.Conrols;
using Task4.ViewModels;
using Task4.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Task4
{
    public partial class App
    {
        public static Dictionary<string, string> User = new Dictionary<string, string>();
        public static string FilePath;
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            InnitUsers();

            SetUpNavigation();

        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<ClockPage, ClockPageViewModel>();
            containerRegistry.RegisterForNavigation<ClockSettingPage, ClockSettingPageViewModel>();
        }

        protected async void SetUpNavigation()
        {
            if (Current.Properties.Count != 0)
            {
                var p = new NavigationParameters();
                App.FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"{Current.Properties.Keys.LastOrDefault()}" + ".db");
                p.Add("LoadFromDataBase", true);
                await NavigationService.NavigateAsync("NavigationPage/ClockPage", p);

            }
            else await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected void InnitUsers()
        {
            User.Add("admin", "admin");
            User.Add("user1", "user1");
            User.Add("user2", "user2");
        }
    }
}