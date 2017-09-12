using UnityEngine;
using System.Collections;

public class Rotation : MonoBehaviour {

    public Vector3 RotateV = Vector3.zero;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        transform.Rotate(RotateV * Time.deltaTime);
    }
}
