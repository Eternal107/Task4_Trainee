using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Task4.DataBase;
using Task4.Models;
using Task4.Repository;

namespace Task4.ViewModels
{
    public class MainPageViewModel : ViewModelBase, INavigatedAware
    {
        private MainPageModel Model;
        
        public DelegateCommand ToClockPage=> Model.clockPage ?? (Model.clockPage = new DelegateCommand(PushClockPage));

        public string Login
        {
            get { return Model.login; }
            set { SetProperty(ref Model.login, value); }
        }

        public string Password
        {
            get { return Model.password; }
            set { SetProperty(ref Model.password, value); }
        }


        public MainPageViewModel(INavigationService navigationService)
          : base(navigationService)
        {
            Title = "Login Page";
            Model = new MainPageModel(App.FilePath);
            
        }

        private async void PushClockPage()
        {
            if (Login != null)
            {
                User user = new User() { Login = Login, Password=Password};
                if(await Model.UserRepo.LoginValidate(user))
                {
                    string CurrentLogin = (await Model.UserRepo.Get<User>(x => x.Login == Login)).LastOrDefault().Login;
                    App.Current.Properties.Add(CurrentLogin, Password);
                    await App.Current.SavePropertiesAsync();
                    var p = new NavigationParameters();
                    p.Add("LoadFromDataBase", true);
                    await NavigationService.NavigateAsync("/NavigationPage/ClockPage", p);
                }
            }
        }
    }
}