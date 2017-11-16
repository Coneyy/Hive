using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenGUI : MonoBehaviour {


    GameObject LoggingMenu; //menu (canvas) z logowaniem i rejestracja
    GameObject RegisterMenu;
    GameObject WaitingMenu;
    GameObject Background;
    GameObject GameTitle;
    GameObject PlayerName;
    GameObject OpponentName;


    // Use this for initialization
    void Awake ()
    {
        LoggingMenu = GameObject.Find("Login");
        RegisterMenu = GameObject.Find("Register");
        WaitingMenu = GameObject.Find("Waiting");
        WaitingMenu.SetActive(false);
        Background = GameObject.Find("Background");
        GameTitle = GameObject.Find("GameName");
        PlayerName = GameObject.Find("PlayerName");
        OpponentName = GameObject.Find("OpponentName");
    }

    private void OnEnable()
    {
        NetworkManager.RoomJoined += DisableLoggingMenu;
        NetworkManager.RoomJoined += EnableWaitingMenu;
        NetworkManager.GameStarted += DisableWaitingMenu;
        NetworkManager.GameStarted += DisableCanvasBackground;
        NetworkManager.PlayersNamesObtained += SetPlayersNames;
    }
    private void OnDisable()
    {   
        NetworkManager.RoomJoined -= DisableLoggingMenu;
        NetworkManager.RoomJoined -= EnableWaitingMenu;
        NetworkManager.GameStarted -= DisableWaitingMenu;
        NetworkManager.GameStarted -= DisableCanvasBackground;
        NetworkManager.PlayersNamesObtained -= SetPlayersNames;
    }

    private void DisableLoggingMenu()
    {
        LoggingMenu.SetActive(false);
        RegisterMenu.SetActive(false);
    }

    private void EnableWaitingMenu()
    {
        WaitingMenu.SetActive(true);
    }

    private void DisableWaitingMenu()
    {
        WaitingMenu.SetActive(false);
    }

    private void DisableCanvasBackground()
    {
        Background.SetActive(false);
        GameTitle.SetActive(false);
    }

    private void EnableCanvasBackground()
    {
        Background.SetActive(true);
        GameTitle.SetActive(true);
    }

    private void SetPlayersNames(string playerName, string opponentName)
    {
        var playerNameText = PlayerName.GetComponent<Text>();
        var opponentNameText = OpponentName.GetComponent<Text>();

        playerNameText.text = playerName;
        opponentNameText.text = opponentName;
    }
}
