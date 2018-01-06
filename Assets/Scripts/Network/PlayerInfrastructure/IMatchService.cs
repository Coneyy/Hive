using Assets.Scripts.Network.PlayerInfrastructure.Models;

namespace Assets.Scripts.Network.PlayerInfrastructure
{
    public interface IMatchService
    {
        void UploadMatchInfo(Match match);
    }
}
