using System.Collections.Generic;
using Hive.Assets.Scripts.Network.PlayerInfrastructure.Models;

namespace Hive.Assets.Scripts.Network.PlayerInfrastructure
{

    public interface ISessionService
    {        
		void UpdateCurrentSession(HivePlayer player);
        void UpdateSessionOpponent(HivePlayer opponent);

        Session GetCurrentSession();

        void ClearCurrentSession();
    }
}