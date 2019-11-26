using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Task4.ViewModels
{
    public class MainPageViewModel : ViewModelBase, INavigatedAware
    {
        private DelegateCommand clockPage;

        public DelegateCommand ToClockPage =>
       clockPage ?? (clockPage = new DelegateCommand(PushClockPage));

        private string login;
        private string password;

        public string Login
        {
            get { return login; }
            set { SetProperty(ref login, value); }
        }


        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value); }
        }


        public MainPageViewModel(INavigationService navigationService)
          : base(navigationService)
        {
            Title = "Login Page";
        }

        private void PushClockPage()
        {
            if (Login != null)
                if (App.User.ContainsKey(Login))
                    if (App.User[Login] == Password)
                    {
                        App.FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"{Login}" + ".db");
                        App.Current.Properties.Add(Login, Password);
                        App.Current.SavePropertiesAsync();
                        var p = new NavigationParameters();
                        p.Add("LoadFromDataBase", true);
                        NavigationService.NavigateAsync("/NavigationPage/ClockPage", p);
                    }
        }
    }
}