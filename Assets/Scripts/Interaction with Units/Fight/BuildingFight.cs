using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingFight : MonoBehaviour {


	private List<Interactive> enemies; // lista przeciwników jednostki
	private float range;

	// Use this for initialization
	void Start () {
		GameObject Manager = GameObject.Find("Manager");
		FightInteraction fightInteraction = GetComponent<FightInteraction> ();
		FightInteraction.OnBeforeObjectDestroying += EndGame;
		enemies = new List<Interactive>(); // tworzenie listy wrogów
		range = GetComponent<ShowUnitInfo> ().sight;


	}


	void Update()
	{
		if (!GetComponent<ShowUnitInfo>().photonView.isMine) return; // jak nie jest to nasza jednostka, to wyłącz skrypt

		foreach (Interactive i in enemies)
		{
			if (i == null)
				continue;

			if (MainScreenUtils.isClose(i.transform.position, transform.position, range))
				continue;
			else
			{
				Renderer[] rs = i.gameObject.GetComponentsInChildren<Renderer>();
				foreach (Renderer r in rs)
					r.enabled = false;
			}

		}




		Collider[] list = Physics.OverlapSphere(transform.position, range); // Sprawdzamy wszystkie kolizje z podaną sferą, zwraca listę colliderów
		enemies.Clear();
		foreach (var l in list)
		{
			var interact = l.gameObject.transform.GetComponent<Interactive>(); // rzutuj element kolizji na interactive 
			if (interact == null) //jeśli nie jest interaktywny 
				continue; // to przejdź do kolejnego elementu listy 
			if (l.GetComponent<ShowUnitInfo>().photonView.isMine) // jeżeli to nie nasza jednostka
				continue; // to przejdź do kolejnego elementu listy 
			if (interact.transform.position == transform.position) // jeżeli to ta jednostka na której włączyliśmy skrypt
				continue; // to przejdź do kolejnego elementu listy 
			Renderer[] rs = l.gameObject.GetComponentsInChildren<Renderer>();
			foreach (Renderer r in rs)
				r.enabled = true;

			enemies.Add(interact); // to dodaj

		}

	}


	static bool isAlive = true;

	public void EndGame(GameObject gameObject)
	{
		if (gameObject.GetComponent<BuildingInteractive> () != null && gameObject.GetComponent<ShowUnitInfo> ().photonView.isMine && isAlive) {
			isAlive = false;
			Debug.Log ("GRA ZAKONCZONA!");
			gameObject.GetComponent<IBuilding>().BuildingDestroyed();
		}
	}


}