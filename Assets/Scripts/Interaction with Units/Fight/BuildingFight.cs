﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingFight : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject Manager = GameObject.Find("Manager");
		FightInteraction fightInteraction = GetComponent<FightInteraction> ();
		FightInteraction.OnBeforeObjectDestroying += EndGame;
	}


	public void EndGame(GameObject gameObject)
	{
		if (gameObject.GetComponent<BuildingInteractive> () != null && gameObject.GetComponent<ShowUnitInfo> ().attribiutes.photonView.isMine) {
			Debug.Log ("Gra zakończona, przegrał: " + Player.DefaultPlayer.Name);
		}
	}
	

}