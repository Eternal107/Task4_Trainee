using Prism.Commands;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Task4.DataBase;
using Task4.Repository;

namespace Task4.Models
{
    public class MainPageModel
    {
        public UserRepository UserRepo { get; private set; }

        public string login;
        public string password;
         
        public DelegateCommand clockPage;

        public MainPageModel(string FilePath)
        {
             var connection = new SQLiteAsyncConnection(FilePath);

             UserRepo = new UserRepository(connection);
                
        }
    }
}
