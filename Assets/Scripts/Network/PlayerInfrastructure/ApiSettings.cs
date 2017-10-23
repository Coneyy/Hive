using System.IO;
using UnityEngine;

namespace Hive.Assets.Scripts.Network.PlayerInfrastructure
{
    public static class ApiSettings
    {
        public static string ApiUrl 
        { 
            get
            {
                return "http://hivegameapi.azurewebsites.net/api";
            }

        }
    }
}