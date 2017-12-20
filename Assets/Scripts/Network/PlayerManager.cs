using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hive.Assets.Scripts.Network.PlayerInfrastructure;
using Hive.Assets.Scripts.Network.PlayerInfrastructure.Models;
using UnityEngine.UI;
using System.Net;
using Assets.Scripts.Network.PlayerInfrastructure.Models;
using System;
using Assets.Scripts.Network.PlayerInfrastructure;

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
     
    }
    
    public PlayerManager()
    {
        _playerService = new FakePlayerService();
        _sessionService = new SessionService();
        _matchService = new MatchService();
    }

    public override void UploadMatch(int myPoints, int enemyPoints)
    {
        var match = new Match();
        match.Player1 = _sessionService.GetCurrentSession().Player;
        match.Player2 = _sessionService.GetCurrentSession().OpponentPlayer;
        match.Player1Points = myPoints;
        match.Player2Points = enemyPoints;
        match.Date = DateTime.Now;

        _matchService.UploadMatchInfo(match);
    }
    


}
