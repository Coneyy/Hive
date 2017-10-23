namespace Hive.Assets.Scripts.Network.PlayerInfrastructure.Models
{
    public static class SessionSingleton
    {
        private static object lockObject = new object();
        private static Session _session;

        public static Session Session
        {  
            get
            {
                return _session;
            }
            set
            {
                lock(lockObject)
                {
                    _session=value;
                }
            }   
        }
    }
}