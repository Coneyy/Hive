using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight : MonoBehaviour {



    private MovingUnit movingUnit; //skrypt do poruszania jednostkami
    private FogOfWar fog; //skrypt mgły

    private Interactive enemy; // referencja do przeciwnika
    public bool isFighting; // czy jednostka walczy
    private bool automatic; // czy walka wywołana przez skrypt automatyczny, czy gracza

    // Walka
    private float attackBuffer; //buffer do wyliczania kiedy zaatakować 
	private FightInteraction fightInteraction; // funkcje PUN z Photona
  


    public void startFight(Interactive enemy, bool automatic)
    {
        if(isFighting)
        {
			if (!automatic) {

				if (this.automatic) {
					Debug.Log ("WYŁĄCZAM AUTOMATYCZNY ATAK I USTAWIAM MANALNY!");
				} else {
					Debug.Log ("NOWY CEL!");
				}
				this.enemy = enemy;

			}
				
        }
        else
        {
            isFighting = true;
            this.enemy = enemy;
            this.automatic = automatic;
        }
		stopMoving ();

       
       
    }

	void stopMoving ()
	{
		movingUnit.stopMoving ();
		GetComponent<TouchNavigation> ().stopAgent();

	}

    private void fight()
    {
        if (isFighting)
        {
            if (enemy == null)
            {
                isFighting = false;

            }


            if (!MainScreenUtils.isClose(enemy.transform.position, transform.position, 50))
            {

                movingUnit.sendToTarget(enemy.transform.position);
                return;
                // TO WYŚLIJ DO WALKI 
            }

            // OBRÓT W KIERUNKU PRZECIWNIKA START
            Quaternion _lookRotation;
            Vector3 _direction;
            _direction = (enemy.transform.position - transform.position).normalized;
            _lookRotation = Quaternion.LookRotation(_direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, _lookRotation, Time.deltaTime * 10f);
            // OBRÓT W KIERUNKU PRZECIWNIKA KONIEC


            attackBuffer += Time.deltaTime; // zwiększamy buffer ataku
			if (GetComponent<ShowUnitInfo>().attribiutes.attackDuration < attackBuffer) // jeśli buffer jest większy od częstotliwości ataku
            {
				enemy.GetComponent<ShowUnitInfo>().attribiutes.photonView.RPC("subtractHealth", PhotonTargets.All, GetComponent<ShowUnitInfo>().attribiutes.attack);


                attackBuffer = 0; // resetuj buffer 

            }


        }
    }

  


    // Use this for initialization
    void Start () {
        movingUnit = GetComponent<MovingUnit>();

        GameObject Manager = GameObject.Find("Manager");
        fog = Manager.GetComponent<FogOfWar>();
		fightInteraction = GetComponent<FightInteraction> ();
		FightInteraction.OnBeforeObjectDestroying += RemoveRevealer;
    }

    // Update is called once per frame
    void Update () {

		if (!GetComponent<ShowUnitInfo>().attribiutes.photonView.isMine) return; // jak nie jest to nasza jednostka, to wyłącz skrypt


        if (GetComponent<TouchNavigation>().isActive) // jeżeli poruszamy się jednostką, to automatyczna walka jest wyłączona!
        {
            isFighting = false; //wyłącz walkę
            return;
        }

        fight();

    }

	private void RemoveRevealer(GameObject gameObject){
		fog.removeRevealer (gameObject);		
	}
}
