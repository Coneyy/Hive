namespace Hive.Assets.Scripts.Network.PlayerInfrastructure.Models
{
    public class Session
    {
        HivePlayer _player;

        public Session()
        {
            
        }

        public Session(HivePlayer player) : base()
        {
            _player = player;
        }
        
        public HivePlayer Player
        {
            get
            {
                return _player;
            }
            set
            {
                _player=value;
            }
        }
    }
}