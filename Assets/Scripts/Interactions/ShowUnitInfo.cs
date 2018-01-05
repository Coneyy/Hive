using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowUnitInfo : Interaction
{

	public UnitDefinition attribiutes;
	private float delay = 0.5f;
	private float delayBuffer;
	private bool isSelected = false;


    void Update()
    {
        delayBuffer +=Time.deltaTime;
        if(delayBuffer>delay)
        {
            if(isSelected)
            {
				BottomPanelConnector.Current.changePanelVisibility(attribiutes.type);
				BottomPanelConnector.Current.setPic(attribiutes.pic);
				BottomPanelConnector.Current.setLines(attribiutes.unitName, attribiutes.currentHealth + "/" + attribiutes.maxHealth);
             }
            delayBuffer = 0;

        }

    }

	public void create(string name, UnitDefinition.TYPE type)
    {
		attribiutes = new UnitDefinition ();
		attribiutes.create (name, type);
    }

    public override void Dselect()
    {
        isSelected = false;
        BottomPanelConnector.Current.clearLines();
        BottomPanelConnector.Current.clearPic();
		BottomPanelConnector.Current.setPanelVisibility (false);
		
    }

    public override void Select()
    {
        isSelected = true;
		BottomPanelConnector.Current.setPic(attribiutes.pic);
		BottomPanelConnector.Current.setLines(attribiutes.unitName, attribiutes.currentHealth + "/" + attribiutes.maxHealth);


    }


}
