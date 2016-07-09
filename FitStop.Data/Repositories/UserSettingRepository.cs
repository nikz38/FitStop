using FitStop.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitStop.Data.Repositories
{
    public class UserSettingRepository : BaseRepository<UserSetting>
    {
        public UserSettingRepository(DbContext context) : base(context) { }

    }
}