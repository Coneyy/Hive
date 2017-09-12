using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitScript : MonoBehaviour {

	GameObject imageTarget;


	// Use this for initialization
	void Start () {
		imageTarget=GameObject.Find("ImageTarget");
		imageTarget.SetActive(false);
	}

}
