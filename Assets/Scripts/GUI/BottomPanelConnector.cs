using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BottomPanelConnector : MonoBehaviour {

    public static BottomPanelConnector Current;


    public BottomPanelConnector ()
    {
        Current = this;
    }

	public GameObject BuildingPanel;

	public Image Pic, ResourcePic;
	public Text Line1, Line2, ResourceText;


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
      
	}

    public void prepareUnitView(string name,string status,Sprite pic=null)
    {
        setLines(name, status);
        setPic(pic);
    }

    public void setResourceValue(string resource)
    {
        ResourceText.text = resource;

    }

	public void changePanelVisibility (UnitDefinition.TYPE type)
	{
		if (type == UnitDefinition.TYPE.MOTHERBASE) {
			BuildingPanel.SetActive (true);
		} else
			BuildingPanel.SetActive (false);
	}
	public void setPanelVisibility(bool visibilty)
	{
		BuildingPanel.SetActive (visibilty);
	}



}
