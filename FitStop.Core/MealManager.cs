using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitStop.Entities;
using FitStop.Data.UnitOfWork;
using FitStop.Common.Helpers;

namespace FitStop.Core
{
    public class MealManager
    {
        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public Meal Get(int id)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var mealFromDb = uow.MealRepository.GetById(id);
                ValidationHelper.ValidateNotNull(mealFromDb);
                return mealFromDb;
            }
        }

        /// <summary>
        /// Gets for user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public List<Meal> GetForUser(int userId)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var mealsFromDb = uow.MealRepository.Find(m => m.UserId == userId);
                return mealsFromDb;
            }
        }

        /// <summary>
        /// Adds the specified meal.
        /// </summary>
        /// <param name="meal">The meal.</param>
        /// <returns></returns>
        public Meal Add(Meal meal)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                uow.MealRepository.Insert(meal);
                uow.Save();
                return meal;
            }
        }

        /// <summary>
        /// Updates the specified meal.
        /// </summary>
        /// <param name="meal">The meal.</param>
        public void Update(Meal meal)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var mealFromDb = uow.MealRepository.GetById(meal.Id);
                ValidationHelper.ValidateNotNull(mealFromDb);

                mealFromDb.Text = meal.Text;
                mealFromDb.DateTimeFor = meal.DateTimeFor;
                mealFromDb.Calories = meal.Calories;

                uow.MealRepository.Update(mealFromDb);
                uow.Save();
            }
        }

        /// <summary>
        /// Deletes the specified meal.
        /// </summary>
        /// <param name="meal">The meal.</param>
        public void Delete(Meal meal)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var mealFromDb = uow.MealRepository.GetById(meal.Id);
                ValidationHelper.ValidateNotNull(mealFromDb);

                uow.MealRepository.Delete(mealFromDb);
                uow.Save();
            }
        }
    }
}
