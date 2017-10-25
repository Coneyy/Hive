
using UnityEngine;

namespace Hive.Assets.Scripts.Network.PlayerInfrastructure
{
    public abstract class IPlayerManager : MonoBehaviour
    {
        public event SignedIn PlayerSignedIn;
        public event SignedUp PlayerSignedUp;

        public delegate void SignedIn();
        public delegate void SignedUp();

        protected ISessionService _sessionService {get; set;}

         protected IPlayerService _playerService {get; set;}

         public abstract void RegisterPlayer(string email, string username, string password, string confirmPassword);

         public abstract void LogPlayerIn(string emailOrUsername, string password);

         public abstract void LogPlayerOut();

    }
}