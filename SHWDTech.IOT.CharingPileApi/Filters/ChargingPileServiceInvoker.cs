using WebServerComponent.Filter;

namespace SHWDTech.IOT.CharingPileApi.Filters
{
    public class ChargingPileServiceInvoker : IServiceInvoker
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}