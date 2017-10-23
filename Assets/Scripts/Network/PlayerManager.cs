using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hive.Assets.Scripts.Network.PlayerInfrastructure;
using Hive.Assets.Scripts.Network.PlayerInfrastructure.Models;

public class PlayerManager : IPlayerManager
{
    public static event SignedIn PlayerSignedIn;
    public static event SignedUp PlayerSignedUp;

    public delegate void SignedIn();
    public delegate void SignedUp();

    public PlayerManager()
    {
        _playerService = new FakePlayerService();
        _sessionService = new SessionService();
    }

    public override void RegisterPlayer(string email, string username, string password)
    {
		var newPlayer = _playerService.SignUp(email,username,password);
        if (newPlayer != null)
        {
            _sessionService.UpdateCurrentSession(newPlayer);
            _sessionService.UpdateCurrentSession(newPlayer);
            OnPlayerSignedUp();
        }    
        Debug.Log("Player registered, session:" + SessionSingleton.Session.Player.Username);
    }

    public override void SignPlayerIn(string emailOrUsername, string password)
    {
        var player = _playerService.LogIn(emailOrUsername,password);
        if (player!= null)
        {
            _sessionService.UpdateCurrentSession(player);
            OnPlayerSignedIn();
        }

        Debug.Log("Player logged in, session:" + SessionSingleton.Session.Player.Username);
    }

    public override void SignPlayerOut()
    {
        _playerService.LogOut();
        _sessionService.ClearCurrentSession();
        Debug.Log("Session cleared");
    }

    private void OnPlayerSignedIn()
    {
        PlayerSignedIn();
    }

    private void OnPlayerSignedUp()
    {
        PlayerSignedUp();
    }

}
