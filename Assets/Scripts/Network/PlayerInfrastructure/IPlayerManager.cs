
using Assets.Scripts.Network.PlayerInfrastructure;
using UnityEngine;

namespace Hive.Assets.Scripts.Network.PlayerInfrastructure
{
    public abstract class IPlayerManager : MonoBehaviour
    {
        protected ISessionService _sessionService { get; set; }
        protected IMatchService _matchService { get; set; }

        public virtual void UploadMatch(int myPoints, int enemyPoints) { } 

    }
}