using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectAsTarget : MonoBehaviour {

    public Renderer DisplayItem;
    public Material material;

    public void isTarget()
    {
        DisplayItem.enabled = true;
        DisplayItem.material = material;
    }
    public void isNotATarget()
    {
        DisplayItem.enabled = false;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}




}
