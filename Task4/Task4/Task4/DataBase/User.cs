using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Task4.DataBase
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Login { get; set; }
        public string Password { get; set; }
    }
}
