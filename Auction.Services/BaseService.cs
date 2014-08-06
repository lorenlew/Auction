using System;
using Auction.DAL.UnitOfWork;
using Auction.Services.Interfaces;

namespace Auction.Services
{
    public class BaseService : IService
    {
        private bool _disposed;

        protected readonly IUnitOfWork Uow;

        public BaseService(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");
            Uow = unitOfWork;
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
