using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenGUI : MonoBehaviour {


 
    GameObject WaitingMenu;
    GameObject GameCanvas;
    GameObject PlayerName;
    GameObject OpponentName;


    // Use this for initialization
    void Awake ()
    {       
        WaitingMenu = GameObject.Find("Waiting");
		WaitingMenu.SetActive(true);
		GameCanvas = GameObject.Find ("GameCanvas");
        PlayerName = GameObject.Find("PlayerName");
        OpponentName = GameObject.Find("OpponentName");
		Debug.Log ("Screen GUI AWAKE");
    }

    private void OnEnable()
    {
        NetworkManager.RoomJoined += EnableWaitingMenu;
        NetworkManager.GameStarted += DisableWaitingMenu;
		NetworkManager.PlayersNamesObtained += SetPlayersNames;
		Debug.Log ("Screen GUI ON ENABLE");
    }
    private void OnDisable()
    {   
        NetworkManager.RoomJoined -= EnableWaitingMenu;
        NetworkManager.GameStarted -= DisableWaitingMenu;
        NetworkManager.PlayersNamesObtained -= SetPlayersNames;
    }

 

    private void EnableWaitingMenu()
    {
        WaitingMenu.SetActive(true);
		GameCanvas.SetActive (false);
    }

    private void DisableWaitingMenu()
    {
		Debug.Log ("Screen GUI DISABLE WAITING MENU");
        WaitingMenu.SetActive(false);
		GameCanvas.SetActive (true);
    }

 

    private void SetPlayersNames(string playerName, string opponentName)
    {
        var playerNameText = PlayerName.GetComponent<Text>();
        var opponentNameText = OpponentName.GetComponent<Text>();

        playerNameText.text = playerName;
        opponentNameText.text = opponentName;
    }
}
