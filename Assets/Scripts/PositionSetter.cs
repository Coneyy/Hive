using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PositionSetter : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var playerName = GameObject.Find("PlayerName");
        var opponentName = GameObject.Find("OpponentName");

        playerName.transform.position = new Vector3(200,(Screen.height-50) - 10,0);
        opponentName.transform.position = new Vector3(Screen.width -200, (Screen.height-50) - 10,0);

    }

    // Update is called once per frame
    void Update () {
		
	}
}
