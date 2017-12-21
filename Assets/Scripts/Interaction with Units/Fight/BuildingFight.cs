using Hive.Assets.Scripts.Network.PlayerInfrastructure.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingFight : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject Manager = GameObject.Find("Manager");
		FightInteraction fightInteraction = GetComponent<FightInteraction> ();
		FightInteraction.OnBeforeObjectDestroying += EndGame;
	}

    static bool isAlive = true;

	public void EndGame(GameObject gameObject)
	{
		if (gameObject.GetComponent<BuildingInteractive> () != null && gameObject.GetComponent<ShowUnitInfo> ().photonView.isMine && isAlive) {
            isAlive = false;
            Debug.Log ("Gra zakończona, przegrał: " + SessionSingleton.Session.Player.Username);
            gameObject.GetComponent<IBuilding>().BuildingDestroyed();
        }
	}
	

}
