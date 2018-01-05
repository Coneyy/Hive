using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightInteraction : MonoBehaviour {
	
	public static event Action<GameObject> OnBeforeObjectDestroying;

    [PunRPC]
    void subtractHealth(float value)
    {
        if (!GetComponent<ShowUnitInfo>().photonView.isMine) return; // jak nie jest to nasza jednostka, to wyłącz skrypt
        GetComponent<ShowUnitInfo>().currentHealth -= value;

        if (GetComponent<ShowUnitInfo>().currentHealth <= 0)
        {
            GetComponent<ShowUnitInfo>().Dselect();
            if (OnBeforeObjectDestroying != null)
            {
                OnBeforeObjectDestroying(gameObject);
            }
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
