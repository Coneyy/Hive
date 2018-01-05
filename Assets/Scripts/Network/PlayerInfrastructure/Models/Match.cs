using Hive.Assets.Scripts.Network.PlayerInfrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Network.PlayerInfrastructure.Models
{
    [Serializable]
    public class Match : MonoBehaviour
    {
        [SerializeField]
        private HivePlayer player1;
        [SerializeField]
        private HivePlayer player2;
        [SerializeField]
        private int player1Points;
        [SerializeField]
        private int player2Points;
        [SerializeField]
        private string date;

        public HivePlayer Player1
        {
            get
            {
                return player1;
            }

            set
            {
                player1 = value;
            }
        }

        public HivePlayer Player2
        {
            get
            {
                return player2;
            }

            set
            {
                player2 = value;
            }
        }

        public int Player1Points
        {
            get
            {
                return player1Points;
            }

            set
            {
                player1Points = value;
            }
        }

        public int Player2Points
        {
            get
            {
                return player2Points;
            }

            set
            {
                player2Points = value;
            }
        }

        public DateTime Date
        {      
            set
            {
                date = value.ToString();
            }
        }
    }
}
