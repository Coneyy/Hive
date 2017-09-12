using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDefinition : MonoBehaviour {

    static public UnitDefinition definition;
    enum TYPE { ANT=100,SPIDER=1000,WARRIORANT=500};
    public PhotonView photonView;
    float maxHealth;
    float currentHealth;
    string name;
    int type;
	
	// Update is called once per frame
	void Update () {
		
	}
   public UnitDefinition()
    { 
        definition = this;
    }
   public void create(string name, int type)
    {
        this.type = type;
        this.name = name;
        this.maxHealth = type;
        this.currentHealth = this.maxHealth;
    }

}
