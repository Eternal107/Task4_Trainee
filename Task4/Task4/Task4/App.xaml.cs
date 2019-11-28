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
using Task4.DataBase;
using Task4.Repository;
using Task4.ViewModels;
using Task4.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Task4
{
    public partial class App
    {
        
        public static string FilePath;
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
           
            InitializeComponent();
           
             SetDataBase();
             InnitUsers();
           
            await SetUpNavigation();

        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<ClockPage, ClockPageViewModel>();
            containerRegistry.RegisterForNavigation<ClockSettingPage, ClockSettingPageViewModel>();
        }

        private async void SetDataBase()
        {
           
            FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DataBase0" + ".db3");

            var connection = new SQLiteAsyncConnection(FilePath);
             await connection.CreateTablesAsync<User, RegionClockData>();

        }
        protected async Task SetUpNavigation()
        {
            if (Current.Properties.Count != 0)
            {

                var p = new NavigationParameters();
                p.Add("LoadFromDataBase", true);
                await NavigationService.NavigateAsync("NavigationPage/ClockPage", p);

            }
            else await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected async void InnitUsers()
        {
            var connection = new SQLiteAsyncConnection(FilePath);

            UserRepository userRepo = new UserRepository(connection);
            
            var list = (await userRepo.Get<User>()).ToList();

            var temp = new User()
            {
                Login = "admin",
                Password = "admin"
            };
            if (list.LastOrDefault(p => p.Login == temp.Login)==null)
                await userRepo.Insert(temp);
             temp = new User()
            {
                Login = "user1",
                Password = "user1"
            };
            if (list.LastOrDefault(p => p.Login == temp.Login) == null)
                await userRepo.Insert(temp);
             temp = new User()
            {
                Login = "user2",
                Password = "user2"
            };
            if (list.LastOrDefault(p => p.Login == temp.Login) == null)
               await userRepo.Insert(temp);
             list = (await userRepo.Get<User>()).ToList();
        }
    }
}