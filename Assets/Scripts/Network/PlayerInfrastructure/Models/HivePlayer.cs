using System;
using UnityEngine;

namespace Hive.Assets.Scripts.Network.PlayerInfrastructure.Models
{
    [Serializable]
    public class HivePlayer
    {

        public HivePlayer(Guid id, string username, string email, string password)
        {
            Id = id;
            Username = username;
            Email = email;
            Password = password;
        }

        public HivePlayer(string username, string email, string password)
        {
            Username = username;
            Email = email;
            Password = password;
        }
	

		public HivePlayer(string emailOrUsername, string password)
        {
            if (emailOrUsername.Contains("@"))
            {
                Email = emailOrUsername;
            }
            else
            {
                Username = emailOrUsername;
            }          

            Password = password;
        }

        public HivePlayer() { }

        [SerializeField]
        private Guid id;
        [SerializeField]
        private string username;
        [SerializeField]
        private string email;
        [SerializeField]
        private string password;

        public string Email
        {
            get
            {
                return email;
            }

            set
            {
                email = value;
            }
        }

        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
            }
        }

        public string Username
        {
            get
            {
                return username;
            }

            set
            {
                username = value;
            }
        }

        public Guid Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }
    }
}