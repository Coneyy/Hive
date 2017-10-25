using Hive.Assets.Scripts.Network.PlayerInfrastructure.Models;

namespace Hive.Assets.Scripts.Network.PlayerInfrastructure
{
    public interface IPlayerService
    {    
       
		HivePlayer LogIn(string emailOrUsername, string password);

        void LogOut();

		HivePlayer Register(string email, string username, string password );

    }
}