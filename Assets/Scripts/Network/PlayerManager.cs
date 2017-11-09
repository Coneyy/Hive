using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hive.Assets.Scripts.Network.PlayerInfrastructure;
using Hive.Assets.Scripts.Network.PlayerInfrastructure.Models;
using UnityEngine.UI;
using System.Net;
using Assets.Scripts.Network.PlayerInfrastructure.Models;
using System;

public class PlayerManager : IPlayerManager
{
    public static event LoggedIn PlayerLoggedIn;
    public static event Registered PlayerRegistered;

    public delegate void LoggedIn();
    public delegate void Registered();
    
    private Text loginNotifier;
    private Text registerNotifier;

    public void Start()
    {
        registerNotifier = GameObject.FindGameObjectWithTag("RegisterNotifier").GetComponent<Text>();        
        loginNotifier = GameObject.FindGameObjectWithTag("LoginNotifier").GetComponent<Text>();
    }

    private void OnEnable()
    {
        LoginClick.LoginClicked += LogPlayerIn;
        RegisterClick.RegisteredClicked += RegisterPlayer;
    }

    private void OnDisable()
    {
        LoginClick.LoginClicked -= LogPlayerIn;
        RegisterClick.RegisteredClicked -= RegisterPlayer;
    }
    
    public PlayerManager()
    {
        _playerService = new FakePlayerService();
        _sessionService = new SessionService();
    }

    public override void RegisterPlayer(string email, string username, string password, string confirmPassword)
    {
        try
        {
            if (password.Equals(confirmPassword))
            {
                var newPlayer = _playerService.Register(email, username, password);
                if (newPlayer != null)
                {
                    _sessionService.UpdateCurrentSession(newPlayer);
                    _sessionService.UpdateCurrentSession(newPlayer);
                    OnPlayerRegistered();
                }
            }
            else
            {
                SetRegisterError("Passwords does not match");
            }
        }
        catch (Exception ex)
        {
            SetRegisterError(ex.Message);
        }

        Debug.Log("Player registered, session:" + SessionSingleton.Session.Player.Username);
    }

    public override void LogPlayerIn(string emailOrUsername, string password)
    {
        try
        {
            var player = _playerService.LogIn(emailOrUsername, password);
            if (player != null)
            {
                _sessionService.UpdateCurrentSession(player);
                OnPlayerLoggedIn();
            }
            ClearNotifications();
        }
        catch (Exception ex)
        {
            SetLoginError(ex.Message);
        }

        Debug.Log("Player logged in, session:" + SessionSingleton.Session.Player.Username);
    }

    public override void LogPlayerOut()
    {
        _playerService.LogOut();
        _sessionService.ClearCurrentSession();
        Debug.Log("Session cleared");
    }



    private void SetLoginError(string error)
    {
        loginNotifier.text = error;
    }

    private void SetRegisterError(string error)
    {
        registerNotifier.text = error;
    }

    private void ClearNotifications()
    {
        loginNotifier.text = "";
        registerNotifier.text = "";
    }



    private void OnPlayerLoggedIn()
    {
        if (PlayerLoggedIn != null)
        {
            PlayerLoggedIn();
        }
    }

    private void OnPlayerRegistered()
    {
        if (PlayerRegistered != null)
        {
            PlayerRegistered();
        }
    }

}
