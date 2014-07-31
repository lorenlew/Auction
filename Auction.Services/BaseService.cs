using System;
using Auction.Repositories;
using Auction.Repositories.Interfaces;
using Auction.Services.Interfaces;

namespace Auction.Services
{
    public class BaseService : IService
    {
        private bool _disposed;

        protected readonly IUnitOfWork Uow;

        protected BaseService()
        {
            Uow = new UnitOfWork();
        }
        public void Save()
        {
            Uow.Save();
        }

        public void DisableValidationOnSave()
        {
            Uow.DisableValidationOnSave(); 
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
                Uow.Dispose();
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
