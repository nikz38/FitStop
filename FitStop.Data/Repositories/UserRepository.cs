using FitStop.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitStop.Data.Repositories
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(DbContext context) : base(context) { }

        public List<User> Search(string query, bool getManagers = false)
        {
            IQueryable<User> usersQuery = dbSet.Include("UserSetting");

            if (!string.IsNullOrWhiteSpace(query))
            {
                usersQuery = usersQuery.Where(u => u.FirstName.ToLower().Contains(query) || u.LastName.ToLower().Contains(query) || u.EMail.ToLower().Contains(query));
            }

            if (!getManagers)
            {
                usersQuery = usersQuery.Where(u => u.Role != Common.UserRole.UserManager && u.Role != Common.UserRole.Administrator);
            }

            var users = usersQuery.ToList();

            return users;
        }
    }
}