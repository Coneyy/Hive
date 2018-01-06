using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAutoFight : MonoBehaviour
{

    // Use this for initialization
    private float range;

    private List<Interactive> enemies; // lista przeciwników jednostki
    private Vector3 enemyPositionToSend; // pozycja przeciwnika 
    private Interactive enemy; //referencja do wybranego przeciwnika
    private UnityEngine.AI.NavMeshAgent agent; // agent do szukania drogi
    private bool isActive = false; // czy agent jest aktywny
    public bool isFighting = false; // czy jednostka walczy 
    private Fight fight; //skrypt obsługujący walkę


    private MovingUnit movingUnit; //skrypt do poruszania jednostkami



    void Start()
    {
        enemies = new List<Interactive>(); // tworzenie listy wrogów
        movingUnit = GetComponent<MovingUnit>();
        fight = GetComponent<Fight>();
		range = GetComponent<ShowUnitInfo> ().sight;
    }

    // Update is called once per frame
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
        if (fight.isFighting)
            return;
		
		if (GetComponent<TouchNavigation>().isActive) return; // jeżeli poruszamy się jednostką, to automatyczna walka jest wyłączona!


        float distance = float.MaxValue;
        float tempDistance;
        foreach (var e in enemies)
        {
            tempDistance = Vector3.Distance(e.transform.position, transform.position);
            if (tempDistance < distance)
            {
                distance = tempDistance;
                this.enemy = e;

            }

        }
        if (enemy != null)
        {
            if (!MainScreenUtils.isClose(enemy.transform.position, transform.position, 50)) // JEŚLI SĄ W ZASIĘGU WYSZUKIWANIA, LECZ DALEJ NIŻ 50 JEDNOSTEK
            {
                movingUnit.sendToTarget(enemy.transform.position); Debug.Log("WYSYŁAM DO WALKI!"); // TO WYŚLIJ DO WALKI 
            }
            else
            {

                fight.startFight(this.enemy, true); Debug.Log("WŁĄCZAM WALKĘ!"); // rozpocznij walkę


            }
        }


    }

	public void removeEnemy (Interactive enemy)
	{
		enemies.Remove (enemy);
	}
}
