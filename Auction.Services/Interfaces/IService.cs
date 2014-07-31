using System;

namespace Auction.Services.Interfaces
{
    public interface IService : IDisposable
    {
        void Save();

        void DisableValidationOnSave();

    }
}
