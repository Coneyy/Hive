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

        Debug.Log(player.Username + " " + player.Email + " " + player.Password + " ");
        Debug.Log(json);

        string response;

        try
        {
            response = GetLoginHttpResponseString(json);

            Debug.Log(response);

            var returnPlayer = JsonUtility.FromJson<HivePlayer>(response);

            Debug.Log(returnPlayer.Username + returnPlayer.Email + returnPlayer.Id);

            return returnPlayer;
        }
        catch (NotFoundException nfe)
        {
            throw new Exception("Wrong credentials");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public HivePlayer Register(string email, string username, string password)
    {
        var player = new HivePlayer(username, email, password);
        string json = JsonUtility.ToJson(player);

        Debug.Log(player.Email +  " " + player.Password);
        Debug.Log(json);

        string response;

        try
        {
            response = GetRegisterHttpResponseString(json);

            Debug.Log("Odpowiedz serwera" + response);

            var returnPlayer = JsonUtility.FromJson<HivePlayer>(response);

            return returnPlayer;
        }
        catch (WebException wex)
        {
            if (((HttpWebResponse)wex.Response).StatusCode == HttpStatusCode.BadRequest)
            {
                throw new Exception("User already exists");
            }
            else
            {
                throw new Exception(wex.Message);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }   
    }

    public void LogOut()
    {
    }

    private string GetLoginHttpResponseString(string PlayerJson)
    {
        byte[] pData = Encoding.ASCII.GetBytes(PlayerJson.ToCharArray());

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ApiUrls.ApiLogInUrl);
        request.Method = "POST";
        request.KeepAlive = true;
        request.ContentType = "application/json";
        request.ContentLength = pData.Length;

        Stream requestStream = request.GetRequestStream();
        requestStream.Write(pData, 0, pData.Length);
        requestStream.Close();

        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        if (response.StatusCode == HttpStatusCode.NoContent)
        {
            throw new NotFoundException();
        }

        string myResponse = "EMPTY";

        using (System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream()))
        {
            myResponse = sr.ReadToEnd();
        }

        return myResponse;
    }

    private string GetRegisterHttpResponseString(string PlayerJson)
    {
        byte[] pData = Encoding.ASCII.GetBytes(PlayerJson.ToCharArray());

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ApiUrls.ApiCreateUserUrl);
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
