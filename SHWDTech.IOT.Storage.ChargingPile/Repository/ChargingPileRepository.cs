using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using SHWDTech.IOT.Storage.ChargingPile.Entities;
using SHWDTech.IOT.Storage.ChargingPile.ViewModels;

namespace SHWDTech.IOT.Storage.ChargingPile.Repository
{
    public class ChargingPileRepository : IDisposable
    {
        private readonly ChargingPileDbContext _ctx;

        public ChargingPileRepository()
        {
            _ctx = new ChargingPileDbContext();
        }

        public void Dispose()
        {
            _ctx?.Dispose();
        }
    }
}
