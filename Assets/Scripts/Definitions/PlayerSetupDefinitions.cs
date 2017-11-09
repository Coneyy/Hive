using UnityEngine;
using System.Collections.Generic;


[System.Serializable]
public class PlayerSetupDefinitions {

    public string Name;

    public Transform location;

    public Color AccentColor;

    public List<GameObject> startingUnits = new List<GameObject>();

    public int Credits;


	
}
