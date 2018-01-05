using UnityEngine;
using System.Collections;
using System;

public class ResourceScript :MonoBehaviour
{
 
	BuildingManager manager;
	float timer = 0.0f;
	float maxTime=5.0f;
	bool started=false;


	void Start()
	{
		manager = GetComponent<BuildingManager> ();
		NetworkManager.GameStarted += setStarted;
		NetworkManager.GameOver += setEnded;
	}
	void setStarted()
	{
		started = true;
	}
	void setEnded(string name)
	{
		started = false;
	}

	void Update()
	{
		if(!started)
			return;
			timer += Time.deltaTime; 
		if (timer >= maxTime) {
			addResources ();
			timer = 0.0f;
		}

	}

	int getResourceToAdd ()
	{
		int lvl = manager._building.Level;
		return lvl * 50;

	}

	void addResources ()
	{
		RtsManager.Gold = RtsManager.Gold + getResourceToAdd ();
		
	}
}
