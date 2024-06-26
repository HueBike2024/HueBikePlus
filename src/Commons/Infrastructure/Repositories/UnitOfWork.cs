﻿using Core.Interfaces.Database;
using Core.SeedWork.Repository;
using Infrastructure.EF;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BaseDbContext _context;
        private bool disposed = false;

        public UnitOfWork(BaseDbContext context)
        {
            _context = context;
        }

        public int SaveChanges()
        {
            _context.ChangeTracker.AutoDetectChangesEnabled = false;
            return _context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public IRepository<T> AsyncRepository<T>() where T : class
        {
            return new Repository<T>(_context);
        }

        private async Task Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    await _context.DisposeAsync();
                }
            }
            disposed = true;
        }

        public async Task Dispose()
        {
            await Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}