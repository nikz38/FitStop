using FitStop.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitStop.Data.Repositories
{
    public class MealRepository : BaseRepository<Meal>
    {
        public MealRepository(DbContext context) : base(context) { }

    }
}