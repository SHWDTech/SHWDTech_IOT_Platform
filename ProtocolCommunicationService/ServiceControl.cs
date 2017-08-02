using System;

namespace ProtocolCommunicationService
{
    public class ServiceControl
    {
        public static string DbConnString { get; private set; }

        private static bool _isInited;

        public static void Init(string dbConnString)
        {
            DbConnString = dbConnString;
            _isInited = true;
        }

        private static void CheckInit()
        {
            if(_isInited) throw new InvalidOperationException("Must init first");
        }

        public static void StartBusiness(Guid bussnesssId)
        {
            CheckInit();
            var business = BusinessLoader.LoadBusiness(bussnesssId);
            if (business == null) return;
            var manager = new BusinessManager(business);
            manager.Start();
        }
    }
}
