using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightInteraction : MonoBehaviour {
	
	public static event DestroyEvent OnBeforeObjectDestroying;
	public delegate void DestroyEvent (GameObject gameObject);

	[PunRPC]
	bool subtractHealth(float value)
	{
		if (!GetComponent<ShowUnitInfo>().photonView.isMine) return false; // jak nie jest to nasza jednostka, to wyłącz skrypt
		GetComponent<ShowUnitInfo>().currentHealth = GetComponent<ShowUnitInfo>().currentHealth - value;

		if (GetComponent<ShowUnitInfo>().currentHealth <= 0)
		{
			GetComponent<ShowUnitInfo>().Dselect();
			OnBeforeObjectDestroying (gameObject);
			PhotonNetwork.Destroy (gameObject);
			return true;
		}
		return false;

	}



}
