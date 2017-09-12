using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorControllerSpider : MonoBehaviour {

    static Animator anim;
    public GameObject spider;
    private Vector3 curPosition;
    private Vector3 lastPosition;
    // Use this for initialization
    void Start () {

        anim = GetComponent<Animator>();
       curPosition = spider.transform.position;

       lastPosition = spider.transform.position;

    }

    // Update is called once per frame
    void Update () {

        float translation = Input.GetAxis("Vertical");

        curPosition = spider.transform.position;
        if (curPosition != lastPosition)
        {
            anim.SetBool("isWalking", true);
        }else
            anim.SetBool("isWalking", false);

        lastPosition= spider.transform.position;

    }
}
