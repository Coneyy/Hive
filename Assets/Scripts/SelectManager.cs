using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;

public class SelectManager : MonoBehaviour
{
    public List<Interactive> Selections = new List<Interactive>(); // lista zaznaczonych elementów 
    private Vector3 begin; // wynik promienia z pierwszego miejsca dotknięcia
    private Vector3 beginTouched; // pierwsze miejsce dotknięcia 
    private Vector3 now; // miejsce obecnego dotknięcia 
    private Vector3 change = Vector3.zero; // do funkcji closetoSMM
    private CodeGUI GUI; // Obiekt skryptu GUI
    private GameObject Manager; // Przejście do skryptu GUI

    public bool bottomMenuBlock = false; // czy skrypt ma być zablokowany


    public int getSelectedNumber()
    {
        return Selections.Count; // zwróć liczbę zaznaczonych jednostek
    }
    void Start()
    {
        Manager = GameObject.Find("Manager"); // połączenie do innych skryptów

        GUI = Manager.GetComponent<CodeGUI>(); // skrypt GUI
        change.x = 30;
        change.y = 30;
        change.z = 30;
        // wektor change do funkcji closetoSMM
    }

	void dselectAll()
	{
		if (Selections.Count > 0) // i jest cokolwiek w liście 
		{
			foreach (var sel in Selections)
			{
				if (sel != null)
					sel.Dselect(); // to odznacz 
			}
			Selections.Clear(); // i wyczyść listę
		}
	}
    void Update()
    {
        if (Input.touchCount == 0) // jeśli brak dotknięcia to koniec 
            return;

        var es = UnityEngine.EventSystems.EventSystem.current;

        if (es != null && es.IsPointerOverGameObject()) return; // obsługa błędu

        if (Input.touchCount == 2) // jeśl dotkniemy dwoma palcami 
        {

			dselectAll ();
            return;
        }
        if (Input.touchCount != 0)
        {

            if (bottomMenuBlock == true && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                bottomMenuBlock = false;  // jeśli skrypt był zablokowany, to odblokuj jeśli nie ma już dotyku
            }
        }






        if (Input.touchCount == 1)
        {
            if (CodeGUI.returnBottomBarStatus())
            {
                if (Input.GetTouch(0).position.y <= GUI.HiddenBottomBar + GUI.panelHeight / 10) // jeśli dolny pasek GUI jest SCHOWANY
                {
                    if (Input.GetTouch(0).phase == TouchPhase.Began) // i faza rozpoczęcia
                        bottomMenuBlock = true; //  to zablokuj
                    return;
                }
            }
            else if (CodeGUI.returnBottomBarStatus() == false) // jeśli dolny pasek GUI jest POKAZANY
            {
                if (Input.GetTouch(0).position.y <= GUI.ShowedBottomBar) // jeśli dotykamy w miejscu pokazanego dolnego menu
                {
                    if (Input.GetTouch(0).phase == TouchPhase.Began) // i faza rozpoczęcia
                        bottomMenuBlock = true; // to zablokuj
                    return;
                }
            }
            if (bottomMenuBlock) // jeśli skrypt zablokowany
                return; // to zakończ


            if (Input.GetTouch(0).phase == TouchPhase.Ended) // jeśli faza dotyku się zakończyła
            {

                var ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position); // stwórz promień 

                RaycastHit hit; // wyjście promienia (wynik)

                if (!Physics.Raycast(ray, out hit)) // rzuć promień
                    return;


                var buildingInteract = hit.transform.GetComponent<BuildingInteractive>();
                if (buildingInteract == null)
                {
                    var interact = hit.transform.GetComponent<Interactive>(); // rzutowanie na obiekt interactive
                    if (interact == null) // jeśli obiekt dotknięty przez promień nie jest interaktywny 
                    {
                        return;
                    }
                    else
                    {
         
						if (hit.transform.GetComponent<ShowUnitInfo>().attribiutes.photonView.isMine) // jeśli obiekt należy do gracza
                        {
							if (Selections.Any (x => x is BuildingInteractive)) {
								dselectAll ();
							}
                            Selections.Add(interact); // w innym wypadku dodaj do listy zaznaczonych
                            interact.Select(); // zaznacz
                        }
                    }

                }else
                {
					if (buildingInteract.GetComponent<ShowUnitInfo> ().attribiutes.photonView.isMine) {
						dselectAll ();
						Selections.Add(buildingInteract);
						buildingInteract.Select ();
					}

                }




            }
            if (Input.GetTouch(0).phase == TouchPhase.Began) // jeśli faza rozpoczęcia 
            {
                beginTouched = Input.GetTouch(0).position; // zapisujemy miejsce w którym rozpoczęliśmy dotyk 
                begin = MainScreenUtils.ScreenPointToMapPosition(Input.GetTouch(0).position); // rzucamy promień w to miejsce

            }

            now = MainScreenUtils.ScreenPointToMapPosition(Input.GetTouch(0).position); // promień na miejsce, które dotykamy obecnie

            if (!(MainScreenUtils.isNotCloseTo(beginTouched, Input.GetTouch(0).position, change))) // czy wyszliśmy poza zakres i można rozpocząć zaznaczanie wielu jednostek
                return; // jeśli nie to zakończ 




            if (Selections.Count > 0) // jeśli cokolwiek jest w liście zaznaczonych
            {
				dselectAll ();
		
            }


            // ALGORYTM TWORZENIA PROSTOKĄTA DO ZAZNACZANIA 

            Vector3 half;
            half.x = Math.Abs(begin.x - now.x) / 2;
            half.y = 100;
            half.z = Math.Abs(begin.z - now.z) / 2;


            Vector3 tempBoxMiddle = Vector3.zero;

            if (begin.x > now.x)
            {
                tempBoxMiddle.x = begin.x - half.x;

            }
            if (begin.z > now.z)
            {
                tempBoxMiddle.z = begin.z - half.z;
            }
            if (begin.x < now.x)
            {
                tempBoxMiddle.x = begin.x + half.x;

            }
            if (begin.z < now.z)
            {
                tempBoxMiddle.z = begin.z + half.z;
            }
            tempBoxMiddle.y = half.y;


            // KONIEC ALGORYTMU TWORZENIA PROSTOKĄTA DO ZAZNACZANIA 

            Collider[] list = Physics.OverlapBox(tempBoxMiddle, half); // Sprawdzamy wszystkie kolizje z podanym prostokątem, zwraca listę colliderów
            foreach (var l in list)
            {
                var interact = l.gameObject.transform.GetComponent<Interactive>(); // rzutuj element kolizji na interactive 
                if (interact == null) //jeśli nie jest interaktywny 
                    continue; // to przejdź do kolejnego elementu listy 
				if (l.gameObject.transform.GetComponent<PhotonView>().isMine && l.gameObject.GetComponent<BuildingInteractive>()==null)
                {
                    Selections.Add(interact); // w innym przypadku dodaj do listy zaznaczonych 
                    interact.Select(); // i zaznacz
                }

            }



        }




    }
}
