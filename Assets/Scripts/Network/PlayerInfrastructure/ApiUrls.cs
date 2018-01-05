using System.IO;
using UnityEngine;

namespace Hive.Assets.Scripts.Network.PlayerInfrastructure
{
    public static class ApiUrls
    {
        public static string ApiUrl 
        { 
            get
            {
                return "http://hivegameapi.azurewebsites.net/api";
            }

        }

		public static string ApiLogInUrl 
		{ 
			get
			{
				return "http://hivegameapi.azurewebsites.net/api/user/create";
			}

		}

		public static string ApiCreateUserUrl 
		{ 
			get
			{
				return "http://hivegameapi.azurewebsites.net/api/user/signin";
			}

		}

        public static string ApiCreateMatchUrl
        {
            get
            {
                return "http://hivegameapi.azurewebsites.net/api/matches";
            }

        }
    }
}