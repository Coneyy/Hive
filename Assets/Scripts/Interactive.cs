using UnityEngine;
using System.Collections;

public class Interactive : MonoBehaviour {

    private bool _Selected = false;
    public bool Selected { get { return _Selected; } }

    public void Select()
    {
        _Selected = true;
        foreach (var selection in GetComponents<Interaction>())
        {
            selection.Select();

        }
    }
    public void Dselect()
    {
        _Selected = false;
        foreach (var selection in GetComponents<Interaction>())
        {
            selection.Dselect();
        }
    }
	// Use this for initialization
	void Start () {
	
	}
	
	
}
