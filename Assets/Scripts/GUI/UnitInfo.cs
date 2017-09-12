using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UnitInfo : MonoBehaviour {

    public static UnitInfo Current;
    public Image Pic;
    public Text Line1, Line2;

    public UnitInfo ()
    {
        Current = this;


    }
    public void setLines(string line1, string line2)
    {
        Line1.text = line1;
        Line2.text = line2;

    }
    public void clearLines()
    {
        setLines("", "");

    }
    public void setPic(Sprite sprite)
    {
        Pic.sprite = sprite;
        Pic.color = Color.white;
    }
    public void clearPic()
    {
        Pic.color = Color.clear;
    }
	void Start () {
        clearLines();
        clearPic();
		
	}
	

}
