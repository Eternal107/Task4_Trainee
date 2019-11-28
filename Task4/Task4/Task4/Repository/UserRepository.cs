using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Task4.DataBase;

namespace Task4.Repository
{
    public class UserRepository
    {
        private SQLiteAsyncConnection db;

        public UserRepository(SQLiteAsyncConnection db)
        {
            this.db = db;
        }

        public async Task<List<User>> Get() =>
            await db.Table<User>().ToListAsync();

        public async Task<List<User>> Get<TValue>(Expression<Func<User, bool>> predicate = null, Expression<Func<User, TValue>> orderBy = null)
        {
            var query = db.Table<User>();

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
                query = query.OrderBy<TValue>(orderBy);

            return await query.ToListAsync();


        }

        public async Task<User> Get(int id) =>
             await db.FindAsync<User>(id);

        public async Task<User> Get(Expression<Func<User, bool>> predicate) =>
            await db.FindAsync<User>(predicate);

        public async Task<int> Insert(User entity) =>
             await db.InsertAsync(entity);

        public async Task<int> Update(User entity) =>
             await db.UpdateAsync(entity);

        public async Task<int> Delete(User entity) =>
             await db.DeleteAsync(entity);




        public async Task<bool> LoginValidate(User user)
        {

            var data = await db.Table<User>().ToListAsync();
            if (data != null)
            {
                var d1 = data.LastOrDefault(p => p.Login == user.Login && p.Password == user.Password);

                if (d1 != null)
                    return true;
                else
                    return false;

            }
            else
                return false;
        }
    }
}
