using System.Collections.Generic;

namespace SHWDTech.IOT.Storage.ChargingPile.Repository
{
    public class ChargingPileResult
    {
        public bool Succeeded { get; }

        public IEnumerable<string> Errors { get; }

        public static ChargingPileResult Success { get; } = new ChargingPileResult(true);

        public ChargingPileResult(params string[] errors) : this((IEnumerable<string>) errors)
        {
            
        }

        public ChargingPileResult(IEnumerable<string> errors)
        {
            if (errors == null)
            {
                errors = new[]
                {
                    "Operate Failed"
                };
            }

            Succeeded = false;
            Errors = errors;
        }

        protected ChargingPileResult(bool success)
        {
            Succeeded = success;
            Errors = new string[0];
        }

        public static ChargingPileResult Failed(params string[] errors) => new ChargingPileResult(errors);
    }
}
