using FitStop.Common.Exceptions;
using FitStop.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitStop.Common.Helpers
{
    public static class ValidationHelper
    {
        public static void ValidateNotNull<T>(T entity) where T : class
        {
            string entityName = typeof(T).Name.ToSentenceCase();

            if (entity == null)
            {
                throw new ValidationException($"{entityName} does not exist!");
            }
        }

        public static void ValidateEntityExist<T>(T entity) where T : class
        {
            string entityName = typeof(T).Name.ToSentenceCase();

            if (entity != null)
            {
                throw new ValidationException($"{entityName} already exist!");
            }
        }

    }

}
