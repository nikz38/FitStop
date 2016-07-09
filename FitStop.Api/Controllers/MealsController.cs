using FitStop.Api.Helpers;
using FitStop.Api.Models;
using FitStop.Common;
using FitStop.Common.Exceptions;
using FitStop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Results;

namespace FitStop.Api.Controllers
{
    /// <summary>
    /// Meals API controller
    /// </summary>
    /// <seealso cref="FitStop.Api.Controllers.BaseController" />
    [TokenAuthorize]
    public class MealsController : BaseController
    {
        /// <summary>
        /// Gets the meal by specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="RuntimeException">You can only get your own meals!</exception>
        [HttpGet]
        public Meal Get(int id)
        {
            Meal meal = MealManager.Get(id);

            if (meal.UserId != CurrentUser.Id && CurrentUser.Role == UserRole.AppUser.ToString())
            {
                throw new ValidationException("You can only get your own meals!");
            }

            return meal;
        }

        /// <summary>
        /// Gets the meals for user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        /// <exception cref="RuntimeException">You can only get your own meals!</exception>
        [HttpGet]
        public List<Meal> GetForUser(int userId)
        {
            List<Meal> meals = MealManager.GetForUser(userId);

            if (userId != CurrentUser.Id && CurrentUser.Role == UserRole.AppUser.ToString())
            {
                throw new ValidationException("You can only get your own meals!");
            }

            return meals.OrderByDescending(m => m.DateTimeFor).ToList();
        }

        /// <summary>
        /// Adds the specified meal.
        /// </summary>
        /// <param name="meal">The meal.</param>
        /// <returns></returns>
        /// <exception cref="RuntimeException">You can only add meals to your own profile!</exception>
        [HttpPost]
        public Meal Add(Meal meal)
        {
            if (CurrentUser.Role == UserRole.AppUser.ToString())
            {
                if (meal.UserId != CurrentUser.Id) throw new ValidationException("You can only add meals to your own profile!");
                meal.UserId = CurrentUser.Id;
            }

            Meal mealFromDb = MealManager.Add(meal);
            return mealFromDb;
        }

        /// <summary>
        /// Updates the specified meal.
        /// </summary>
        /// <param name="meal">The meal.</param>
        /// <returns></returns>
        /// <exception cref="RuntimeException">You can only update your own meals!</exception>
        [HttpPut]
        public Meal Update(Meal meal)
        {
            if (meal.UserId != CurrentUser.Id && CurrentUser.Role == UserRole.AppUser.ToString())
            {
                throw new ValidationException("You can only update your own meals!");
            }

            MealManager.Update(meal);
            return meal;
        }

        /// <summary>
        /// Deletes the meal by specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="ValidationException">You can only delete your own meals!</exception>
        [HttpDelete]
        public Meal Delete(int id)
        {
            Meal meal = MealManager.Get(id);

            if (meal.UserId != CurrentUser.Id && CurrentUser.Role == UserRole.AppUser.ToString())
            {
                throw new ValidationException("You can only delete your own meals!");
            }

            MealManager.Delete(meal);
            return meal;
        }
    }
}
