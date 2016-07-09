using FitStop.Data.Repositories;
using FitStop.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitStop.Data.UnitOfWork
{
    /// <summary>
    /// Unit of Work pattern implementation
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class UnitOfWork : IDisposable
    {
        #region Fields

        /// <summary>
        /// Data context
        /// </summary>
        private DbContext _context;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Data context
        /// </summary>
        private DbContext DataContext { get { return _context ?? (_context = new FitStopEntities()); } }

        #region Repositories

        private UserRepository userRepository;
        public UserRepository UserRepository { get { return userRepository ?? (userRepository = new UserRepository(DataContext)); } }

        private UserSettingRepository userSettingRepository;
        public UserSettingRepository UserSettingRepository { get { return userSettingRepository ?? (userSettingRepository = new UserSettingRepository(DataContext)); } }

        private MealRepository mealRepository;
        public MealRepository MealRepository { get { return mealRepository ?? (mealRepository = new MealRepository(DataContext)); } }

        #endregion Repositories

        #endregion Properties

        #region Methods

        /// <summary>
        /// Save changes for unit of work async
        /// </summary>
        public async Task SaveAsync()
        {
            _context.ChangeTracker.DetectChanges();
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Save changes for unit of work
        /// </summary>
        public void Save()
        {
            _context.ChangeTracker.DetectChanges();
            _context.SaveChanges();
        }

        #endregion Methods

        #region IDisposable Members

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing && _context != null)
            {
                _context.Dispose();
            }
            _disposed = true;
        }

        /// <summary>
        /// Dispose objects
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Members
    }
}
