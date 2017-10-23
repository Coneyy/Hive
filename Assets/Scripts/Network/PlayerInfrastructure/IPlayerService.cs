using Hive.Assets.Scripts.Network.PlayerInfrastructure.Models;

namespace Hive.Assets.Scripts.Network.PlayerInfrastructure
{
    public interface IPlayerService
    {    
       
		HivePlayer LogIn(string emailOrUsername, string password);

        void LogOut();

		HivePlayer SignUp(string email, string username, string password );

    }
}