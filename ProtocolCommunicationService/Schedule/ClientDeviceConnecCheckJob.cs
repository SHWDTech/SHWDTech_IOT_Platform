using ProtocolCommunicationService.Core;
using Quartz;

namespace ProtocolCommunicationService.Schedule
{
    public class ClientDeviceConnecCheckJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var control = (BusinessControl)context.JobDetail.JobDataMap["control"];
            control.CheckDeviceConnection();
        }
    }
}
