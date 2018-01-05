namespace Hive.Assets.Scripts.Network.PlayerInfrastructure.Models
{
    public class Session
    {
        HivePlayer _player;
        HivePlayer _opponentPlayer;

        public Session()
        {
            
        }

        public Session(HivePlayer player) : this()
        {
            _player = player;
        }

        public Session(HivePlayer player, HivePlayer opponentPlayer) : this(player)
        {
            _opponentPlayer = opponentPlayer;
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

        public HivePlayer OpponentPlayer
        {
            get
            {
                return _opponentPlayer;
            }
            set
            {
                _opponentPlayer = value;
            }
        }
    }
}