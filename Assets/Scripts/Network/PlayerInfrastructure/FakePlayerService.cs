using Hive.Assets.Scripts.Network.PlayerInfrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hive.Assets.Scripts.Network.PlayerInfrastructure.Models;

namespace Hive.Assets.Scripts.Network.PlayerInfrastructure
{
    class FakePlayerService : IPlayerService
    {
        public HivePlayer LogIn(string emailOrUsername, string password)
        {
            return new HivePlayer("player", "player@gmail.com");
        }

        public void LogOut()
        {
        }

        public HivePlayer SignUp(string email, string username, string password)
        {
            return new HivePlayer("player", "player@gmail.com");
        }
    }
}
