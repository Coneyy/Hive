using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class down : MonoBehaviour {

    public GameObject Panel;
  

    // Use this for initialization
    void Start () {
        GameObject bottomBar = GameObject.Find("BottomBarController");
        RectTransform objectRectTransform = Panel.GetComponent<RectTransform>();
        float x = objectRectTransform.rect.height * Screen.height / 720;
        float y = bottomBar.transform.position.y - Screen.height + x;
        Vector3 v;
        v.x = bottomBar.transform.position.x;
        v.z = bottomBar.transform.position.z;
        v.y = y;

        bottomBar.transform.position = v;


    }
    void Update()
    {

       


    }


}
