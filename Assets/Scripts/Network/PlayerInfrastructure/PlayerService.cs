using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Hive.Assets.Scripts.Network.PlayerInfrastructure;
using Hive.Assets.Scripts.Network.PlayerInfrastructure.Models;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;
using System.Text;
using System.Threading;
using System.Linq;
using System.Text.RegularExpressions;
using Assets.Scripts.Network.PlayerInfrastructure.Models;

public class PlayerService : IPlayerService
{    
    public HivePlayer LogIn(string emailOrUsername, string password)
    {
        var player = new HivePlayer(emailOrUsername, password);
        string json = JsonUtility.ToJson(player);
        Debug.Log(player.Username);
        Debug.Log(player.Email);
        Debug.Log(player.Password);
        Debug.Log(json);

        byte[] pData = Encoding.ASCII.GetBytes(json.ToCharArray());

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ApiSettings.ApiUrl + "/user/signin");
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

        if (response.StatusCode == HttpStatusCode.NoContent)
        {
            throw new NotFoundException();
        }

        Debug.Log(myResponse);

        var returnPlayer = JsonUtility.FromJson<HivePlayer>(myResponse);

        Debug.Log(returnPlayer.Username + returnPlayer.Email + returnPlayer.Id);
        
        return returnPlayer;
    }

    public void LogOut()
    {
    }

    public HivePlayer SignUp(string email, string username, string password)
    {
        var player = new HivePlayer( username, email, password);
        string json = JsonUtility.ToJson(player);
        Debug.Log(player.Email);
        Debug.Log(player.Password);
        Debug.Log(json);

        byte[] pData = Encoding.ASCII.GetBytes(json.ToCharArray());

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ApiSettings.ApiUrl + "/user/create");
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

        Debug.Log("Odpowiedz serwera" + myResponse);

        var returnPlayer = JsonUtility.FromJson<HivePlayer>(myResponse);
        
        return returnPlayer;
    }

}
