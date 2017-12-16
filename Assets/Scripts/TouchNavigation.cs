using UnityEngine;
using System.Collections;
using System;

public class TouchNavigation : Interaction
{
    private GameObject clickObj; // skopiowany obiekt znacznika by móc na nim działać
    public GameObject Click; // obiekt znacznika
    public Transform location; // miejsce znacznika
    private CodeGUI GUI; // Przyciski a świat gry
    private GameObject Manager; // Przejście do skryptu GUI
    public float RelaxDistance = 10; // Przy jakiej odległości od celu agent może wyluzować i przestać szukać dalszej drogi

    private UnityEngine.AI.NavMeshAgent agent; // agent do szukania drogi

    private Vector3 change = Vector3.zero; // margines błędu dla przytrzymania 
    private Vector3 firstTarget = Vector3.zero; // czy przy rozpoczęciu naciskania trafiliśmy w mapę
    private Vector3 firstTargetTouched = Vector3.zero; // miejsce na ekranie tel przy rozpoczęciu naciskania
    private Vector3 target = Vector3.zero; // cel dla agenta

    private bool selected = false; // czy jednostka jest zaznaczona
    public bool isActive = false; // czy agent szuka drogi 

    public Vector3 RotateV = Vector3.zero; // rotacja dla znacznika na mapie
    private bool rotateDone = false; // rotacja zakończona, minął czas
    private bool isCreated = false; // czy znacznik istnieje
    private float timesincestart = 0; // czas od rozpoczęcia gry
    private bool bottomMenuBlock = false; // czy dolne menu blokuje kawałek swiata gry

    
	public void stopAgent()
	{
		agent.Stop(); // to pozwól mu zakończyć 
		isActive = false; // agent przestaje działać
	}

    public override void Dselect()
    {
        selected = false; // jednostka odznaczona
    }

    public override void Select()
    {
        selected = true; // jednostka zaznaczona
    }

    public void sendToTarget()
    {
       Vector2 randomTarget= UnityEngine.Random.insideUnitCircle*0.05f; // obszar do wysłania agentów (żeby nie wysyłać wszystkich w dokładnie jeden punkt)
        target.x += randomTarget.x;
        target.z += randomTarget.y;
        target.y = 10.2f; // ustawienie wysokości na tę, która jest naszym terenem
        

        agent.SetDestination(target); // ustawienie celu
        agent.Resume(); // wysłanie do celu
        isActive = true; // agent działa

    }
    private bool closeto(Vector3 start, Vector3 finish, Vector3 change) 
    {

        if ((finish.x > start.x - change.x && finish.x < start.x + change.x) && (finish.y > start.y - change.y && finish.y < start.y + change.y) && (finish.z > start.z - change.z && finish.z < start.z + change.z))
        {
            return true; // funkcja zwróci prawdę jeśli punkt finish zawiera się w obszarze start (+-) change
        }

        return false; // jeśli poza to zwróci fałsz
    }
    private bool checkIfInteractive() // sprawdzamy czy przyciskamy coś, z czym można wejść w interakcję
    {
        var ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position); // tworzymy promień
        RaycastHit hit;
        Physics.Raycast(ray, out hit); // rzucamy raz jeszcze promień 
        var interact = hit.transform.GetComponent<Interactive>(); // rzutowanie tego, w co uderzył promień na kompontent Interactive
        if (interact != null)
            return true; // jeśli !null, to znaczy ze wcisnęliśmy coś, co nie jest interaktywne i możemy tam ustawić target
        return false;
    }
    private void destroyLoadingMark()
    {
        if (isCreated) // jeśli kółko jest stworzone 
        {
            Destroy(clickObj); // to je zniszcz
            isCreated = false;
            rotateDone = true;
            // i ustaw poprawnie boole
            return; // jeśli skrypt zablokowany to zwróć
        }
    }
    void Start()
    {
        clickObj = null; // musimy zainicjalizować obiekt znacznika 
        Manager = GameObject.Find("Manager"); // w Managerze znajdują się wszystkie skrypty, obiekt do łączenia się 
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>(); // by móc działać na agencie przypisanym do tego obiektu
        GUI = Manager.GetComponent<CodeGUI>(); // aby móc działać na GUI
       
        change.x = 30;
        change.y = 30;
        change.z = 30;
        // change to wektor, który jest używany w funkcji "closeto", wartości wyznaczone empirycznie
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive && Vector3.Distance(target, transform.position) < RelaxDistance) // jeśli agent jest aktywny i jest już w pobliżu celu
        {

            agent.Stop(); // to pozwól mu zakończyć 
            isActive = false; // agent przestaje działać
        }


        if (Input.touchCount == 0) // jeśli nie ma dotyku na ekranie koniec funkcji
            return;

        if (Input.touchCount != 0)
        {
            if (bottomMenuBlock == true && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                bottomMenuBlock = false; // jeśli skrypt był zablokowany, to odblokuj jeśli nie ma już dotyku
            }
            if(Input.GetTouch(0).phase==TouchPhase.Ended)
            {
                destroyLoadingMark();
                return;
            }
        }

        if (Input.touchCount == 1)
        {
            if(CodeGUI.returnBottomBarStatus()) // jeśli dolny pasek GUI jest SCHOWANY
            {
                if (Input.GetTouch(0).position.y <= GUI.HiddenBottomBar) // jeśli dotykamy w miejscu schowanego dolnego menu
                {
                    if (Input.GetTouch(0).phase == TouchPhase.Began)
                        bottomMenuBlock = true; // to zablokuj skrypt
                    return;
                }
            }else if(CodeGUI.returnBottomBarStatus()== false) // jeśli dolny pasek GUI jest POKAZANY
            {
                if (Input.GetTouch(0).position.y <= GUI.ShowedBottomBar) // jeśli dotykamy w miejscu pokazanego dolnego menu
                {
                    if (Input.GetTouch(0).phase == TouchPhase.Began)
                        bottomMenuBlock = true; // to zablokuj skrypt
                    return;
                }
            }

            if (bottomMenuBlock)
            {
                destroyLoadingMark();
                return;
            }
                


            if (Input.GetTouch(0).phase == TouchPhase.Began && selected) // jeśli rozpoczęliśmy wciskanie i jednostka była zaznaczona
            {
                timesincestart = Time.time; // zapisujemy czas od początku gry
                firstTargetTouched = Input.GetTouch(0).position; // zapisujemy miejsce w którym rozpoczęliśmy dotyk 
                firstTarget = MainScreenUtils.ScreenPointToMapPosition(Input.GetTouch(0).position); // rzucamy promień w to miejsce
                if (firstTarget == Vector3.zero)
                    return; // jeśli Vector3.zero, to kliknęliśmy w złe miejsce (np poza mapę)

                if (checkIfInteractive()) return; // jeżeli to w co kliknęliśmy jest interaktywnym obiektem, to zwróć

                Vector3 clickSpot = firstTarget; // miejsce do stworzenia kółka oczekiwania 
                clickSpot.y = 10; // jego wysokość by nie dotykało podłoża 

                if (isCreated == false) // jeśli nie ma w świecie gry kółka
                {

                    clickObj = (GameObject)GameObject.Instantiate(Click, clickSpot, location.rotation); // stworzenie kółka oczekiwania 
                    rotateDone = false; // jeszcze nie ma przestać się kręcić 
                    isCreated = true; // ale jest już stworzone
                }

            }

            if (Input.GetTouch(0).phase == TouchPhase.Ended) // jeśli faza dotykania się zakończyła 
            {
                if (isCreated) // jeśli kółko jest stworzone 
                {
                    Destroy(clickObj); // to je zniszcz
                    isCreated = false;
                    rotateDone = true;
                    // i ustaw poprawnie boole
                    return;
                }

            }
        }
        if (selected == false && isCreated == true) // obsługa błędu. jeśli nic nie jest zaznaczone, a kółko jest, to je zniszcz
        {
            Destroy(clickObj);
            isCreated = false;
            rotateDone = true;
            return;
        }
        if (selected && Input.touchCount != 0) // jeśli coś jest zaznaczone i dotykamy ekranu
        {
            var tempTarget = MainScreenUtils.ScreenPointToMapPosition(Input.GetTouch(0).position); // wynik promienia z obecnego miejsca które dotykamy
            var tempTargetTouched = Input.GetTouch(0).position; // obecne miejsce które dotykamy na ekranie
            if (checkIfInteractive()) return; // jeżeli to w co kliknęliśmy jest interaktywnym obiektem, to zwróć

            if (tempTarget != Vector3.zero) // jeśli miejsce to jest w porządku
            {
                if (rotateDone == false) // jeśli nie skończyło się kręcić 
                {
                    if (clickObj != null) // jeśli nie zniszczyliśmy wcześniej kółka
                    {
                        clickObj.transform.Rotate(RotateV * Time.deltaTime * 6); // kręć kołem
                        clickObj.transform.position = tempTarget; // przestaw koło
                    }
                }
                if (!(closeto(firstTargetTouched, tempTargetTouched, change))) // jeśli miejsce dotykania nie jest już pobliżu początkowego miejsca
                {
                    timesincestart = Time.time; // ponownie ustaw czas (obsługa błędu)
                    Destroy(clickObj); // zniszcz
                    rotateDone = true;
                    isCreated = false;
                    return;
                }
                float timenow = Time.time; // w innym przypadku sprawdź obecny czas 
                if (timenow - timesincestart >= 0.5f) // jeśli dłuższy od pół sekundy to OK
                {
                    timesincestart = Time.time; // ustaw ponownie czas (obsługa błędu)
                    rotateDone = true; // koniec kręcenia 
                    if (closeto(firstTargetTouched, tempTargetTouched, change)) // jeśli nadal jest w porządku (obsługa błędu 1 klatki)
                    {
                        target = tempTarget; // ustaw cel
                        sendToTarget(); // wyślij agenta do celu

                    }
                    else // w innym przypadku zniszcz i ustaw
                    {
                        timesincestart = Time.time;
                        Destroy(clickObj);
                        rotateDone = true;
                        isCreated = false;
                        return;

                    }
                }

            }
        }

        

    }
}
