using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntAnimatorScript : MonoBehaviour {


	Animator animator;
	// Use this for initialization
	Fight fight;

	Vector3 oldPosition;

	bool moving=false;

	void Start () {
		oldPosition = transform.position;
		animator = GetComponent<Animator> ();
		fight = GetComponentInParent<Fight> ();
	}
	
	// Update is called once per frame
	void Update () {
		

		Vector3 newPosition = transform.position;
		if (oldPosition != newPosition) {
			moving = true;
			oldPosition = newPosition;
		} else
			moving = false;
			


		if (moving) {
			animator.SetBool ("doWalk", true);
			animator.SetBool ("doFight", false);

		} else if (fight.isFighting) {
			animator.SetBool ("doFight", true);
			animator.SetBool ("doWalk", false);

		
		} else {
			animator.SetBool ("doWalk", false);
			animator.SetBool ("doFight", false);
			animator.SetBool ("doNothing", true);
		}

	}
}
