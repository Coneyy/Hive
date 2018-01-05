using Assets.Scripts.Network.PlayerInfrastructure.Models;
using Hive.Assets.Scripts.Network.PlayerInfrastructure;
using Hive.Assets.Scripts.Network.PlayerInfrastructure.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Network.PlayerInfrastructure
{
    public class MatchService : IMatchService
    {
        public void UploadMatchInfo(Match match)
        {
            string json = JsonUtility.ToJson(match);

            Debug.Log(match.Player1.Username + " " + match.Player1.Email + " " + match.Player1Points + " " + match.Player2.Username + " " + match.Player2.Email + " " + +match.Player2Points);
            Debug.Log(json);

            string response;

            try
            {
                response = GetMatchCreateHttpResponseString(json);

                Debug.Log(response);                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private string GetMatchCreateHttpResponseString(string PlayerJson)
        {
            byte[] pData = Encoding.ASCII.GetBytes(PlayerJson.ToCharArray());

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ApiUrls.ApiCreateMatchUrl);
            request.Method = "POST";
            request.KeepAlive = true;
            request.ContentType = "application/json";
            request.ContentLength = pData.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(pData, 0, pData.Length);
            requestStream.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            string myResponse = "EMPTY";

            using (System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream()))
            {
                myResponse = sr.ReadToEnd();
            }

            return myResponse;
        }
    }
}

