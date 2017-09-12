using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingUnit : MonoBehaviour {

    Vector3 target; // miejsce do wysłania jednostki
    private UnityEngine.AI.NavMeshAgent agent; // agent do szukania drogi
    public bool isActive = false; // czy agent jest aktywny

    


    // Use this for initialization
    void Start () {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>(); // by móc działać na agencie przypisanym do tego obiektu

    }
  


    public void sendToTarget(Vector3 target)
    {
        Vector3 trueTarget = new Vector3();
        trueTarget.x += target.x;
        trueTarget.z += target.z;
        trueTarget.y = 10.2f; // ustawienie wysokości na tę, która jest naszym terenem
        this.target = trueTarget;

        agent.SetDestination(trueTarget); // ustawienie celu
        agent.Resume(); // wysłanie do celu
        isActive = true; // agent aktywny

    }

    // Update is called once per frame
    void Update () {
        if (!GetComponent<ShowUnitInfo>().photonView.isMine) return; // jak nie jest to nasza jednostka, to wyłącz skrypt

        if (!isActive) return;

        if (RtsManager.Current.isClose(target, transform.position, 50)) // jeśli agent jest aktywny i jest już w pobliżu celu
        {
            agent.Stop(); // to pozwól mu zakończyć 
            isActive = false; // agent przestaje działać
        }

    }
}
