using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchEnemies : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject go in allObjects)
            if (go.activeInHierarchy)
            {
                if (go.GetComponent<Interactive>() != null)
                {
                    if (!go.GetComponent<ShowUnitInfo>().photonView.isMine)
                    {
                        if (!go.GetComponent<ShowUnitInfo>().onSight)
                        {
                            if (!RtsManager.StrategyManager.enemies.Contains(go))

                            {

                       

                                RtsManager.StrategyManager.enemies.Add(go);
                                go.SetActive(false);
                            }
                        }
                        
                        
                    }

                }
            }
    }
}
