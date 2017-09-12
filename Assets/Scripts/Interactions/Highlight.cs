using UnityEngine;
using System.Collections;
using System;

public class Highlight : Interaction
{

    public GameObject DisplayItem; // odnośnik do czegoś, co ma być włączane/wyłączane przy zaznaczaniu

    public override void Dselect()
    {
        DisplayItem.SetActive(false);         // wyłączenie
    }

    public override void Select()
    {
        DisplayItem.SetActive(true);  // włączenie
    }

    // Use this for initialization
    void Start () {
        DisplayItem.SetActive(false); // wyłączenie na start
	}
	

}
