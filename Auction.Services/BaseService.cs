using System;
using Auction.Services.Interfaces;
using Auction.UoW.Interfaces;
using Auction.UoW.Repositories;

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

        private void Dispose(bool disposing)
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
