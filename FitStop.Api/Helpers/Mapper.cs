using AutoMapper;
using FitStop.Api.Models;
using FitStop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitStop.Api.Helpers
{
    /// <summary>
    /// Map helper for mapping compatible object
    /// </summary>
    public static class Mapper
    {
        /// <summary>
        /// Automatically maps one object to another using AutoMapper.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TDestination">The type of the destination.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static TDestination AutoMap<TSource, TDestination>(TSource source)
            where TDestination : class
            where TSource : class
        {
            var config = new MapperConfiguration(c => c.CreateMap<TSource, TDestination>());
            var mapper = config.CreateMapper();
            return mapper.Map<TDestination>(source);
        }

        /// <summary>
        /// Maps the specified user to UserModel.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public static UserModel Map(User user)
        {
            var model = AutoMap<User, UserModel>(user);
            model.Role = user.Role.ToString();
            if (user.UserSetting != null)
            {
                model.DailyCalorieIntake = user.UserSetting.DailyCalorieIntake;
            }
            return model;
        }

        /// <summary>
        /// Maps the specified user to JWT model.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public static UserJwtModel MapJwt(User user)
        {
            var model = AutoMap<User, UserJwtModel>(user);
            model.Role = user.Role.ToString();
            return model;
        }

    }

}