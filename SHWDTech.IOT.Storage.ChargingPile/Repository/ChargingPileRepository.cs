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

        public async Task<ChargingPileResult> RegisterChargingPile(ChargingPileViewModel model)
        {
            if (_ctx.ChargingPiles.Any(c => c.IdentityCode == model.IdentityCode))
            {
                return ChargingPileResult.Failed("IdentityCode Exists");
            }

            if (_ctx.ChargingPiles.Any(c => c.NodeId == model.NodeId))
            {
                return ChargingPileResult.Failed("NodeId Exists");
            }
            var pile = new Entities.ChargingPile
            {
                IdentityCode = model.IdentityCode,
                NodeId = model.NodeId
            };
            _ctx.ChargingPiles.Add(pile);
            foreach (var rechargeShot in model.RechargeShots)
            {
                var shot = new RechargeShot
                {
                    ChargingPile = pile,
                    IdentityCode = rechargeShot.IdentityCode,
                    PortNumber = rechargeShot.PortNumber
                };
                _ctx.RechargeShots.Add(shot);
            }
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var result = await _ctx.SaveChangesAsync();
                if (result > 0)
                {
                    scope.Complete();
                }
            }

            return ChargingPileResult.Success;
        }

        public void Dispose()
        {
            _ctx?.Dispose();
        }
    }
}
