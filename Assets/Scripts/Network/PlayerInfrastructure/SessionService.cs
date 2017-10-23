using Hive.Assets.Scripts.Network.PlayerInfrastructure.Models;
using Hive.Assets.Scripts.Network.PlayerInfrastructure;

namespace Hive.Assets.Scripts.Network.PlayerInfrastructure
{
    public class SessionService : ISessionService
    {
        public SessionService()
        {
            SessionSingleton.Session = new Session();
        }

        public void ClearCurrentSession()
        {
            SessionSingleton.Session = new Session();
        }

		public void UpdateCurrentSession(HivePlayer player)
        {
            SessionSingleton.Session.Player = player;
        }

        public  Session GetCurrentSession()
        {
           return SessionSingleton.Session;
        }
    }
}