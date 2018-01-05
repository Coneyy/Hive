using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendUnitToFight : MonoBehaviour {


    private GameObject Manager; // Przejście do skryptu zaznaczania jednostek
    private SelectManager sMManager; // skrypt do zaznaczania jednostek
    private Fight fight; //skrypt obsługujący walkę



    // Use this for initialization
    void Start () {

        Manager = GameObject.Find("Manager"); // połączenie do innych skryptów
        sMManager = Manager.GetComponent<SelectManager>();
        fight = GetComponent<Fight>();



    }

    // Update is called once per frame
    void Update () {

		if (!GetComponent<ShowUnitInfo>().attribiutes.photonView.isMine) return; // jak nie jest to nasza jednostka, to wyłącz skrypt

        if (sMManager.Selections.Count == 0) // jeśli nie ma zaznaczonych jednostek, wyłącz skrypt
            return;

        if (Input.touchCount == 0) // jeśli brak dotknięcia to koniec 
            return;

        if (Input.GetTouch(0).phase == TouchPhase.Ended) // jeśli faza dotyku się zakończyła
        {

            var ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position); // stwórz promień 

            RaycastHit hit; // wyjście promienia (wynik)

            if (!Physics.Raycast(ray, out hit)) // rzuć promień
                return;


            var interact = hit.transform.GetComponent<Interactive>(); // rzutowanie na obiekt interactive
            if (interact == null) // jeśli obiekt dotknięty przez promień nie jest interaktywny 
                return; // to zakończ
			if (!hit.transform.GetComponent<ShowUnitInfo>().attribiutes.photonView.isMine) // jeśli obiekt nie należy do gracza
            {
                Debug.Log("DO ATAKU!");
                fight.startFight(interact, false);
            }
            return; // zakończ

        }

		
	}
}
