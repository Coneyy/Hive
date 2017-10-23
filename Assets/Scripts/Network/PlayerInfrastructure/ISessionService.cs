using System.Collections.Generic;
using Hive.Assets.Scripts.Network.PlayerInfrastructure.Models;

namespace Hive.Assets.Scripts.Network.PlayerInfrastructure
{

    public interface ISessionService
    {        
		void UpdateCurrentSession(HivePlayer player);

        Session GetCurrentSession();

        void ClearCurrentSession();
    }
}