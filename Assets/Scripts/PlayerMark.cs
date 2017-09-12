using System.Collections;
using UnityEngine;

public class PlayerMark : MonoBehaviour {
    public MeshRenderer[] Renderes;

    // Use this for initialization
    void Start () {
        
        var color = GetComponentInParent<Player>().Info.AccentColor;
        foreach(var r in Renderes)
        {

            r.material.color = color;
            Debug.Log("Zmiana koloru na: " + color);
        }

	}
	
}
