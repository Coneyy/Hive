using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowUnitInfo : Interaction
{


    public string unitName;
    public float maxHealth, currentHealth, attack;
    public float attackDuration;
    public int sight;

    public bool onSight = false;

    public Sprite pic;
    enum TYPE { ANT = 100, SPIDER = 1000, WARRIORANT = 500, MOTHERBASE = 0 };
    enum ATTACK { ANT = 20, SPIDER = 500, WARRIORANT = 30, MOTHERBASE=0 };
    enum DURATION { ANT = 4, SPIDER = 7, WARRIORANT = 3, MOTHERBASE=0};
    enum SIGHT { ANT = 100, SPIDER = 5, WARRIORANT = 100, MOTHERBASE=150};


    public PhotonView photonView;

    private float delay = 0.5f;
    private float delayBuffer;

    private bool isSelected = false;

    String type;

    // Update is called once per frame
    void Update()
    {
        delayBuffer +=Time.deltaTime;
        if(delayBuffer>delay)
        {
            if(isSelected)
            {
            
                UnitInfo.Current.setPic(pic);
                UnitInfo.Current.setLines(unitName, currentHealth + "/" + maxHealth);
             }
            delayBuffer = 0;

        }

    }

    public void create(string name, string type)
    {
        this.type = type;
        this.unitName = name;
        this.maxHealth = (float)(TYPE)Enum.Parse(typeof(TYPE), type);
        this.currentHealth = this.maxHealth;
        this.attack = (float)(ATTACK)Enum.Parse(typeof(ATTACK), type);
        this.attackDuration= (float)(DURATION)Enum.Parse(typeof(DURATION), type);
        this.sight = (int)(SIGHT)Enum.Parse(typeof(SIGHT), type);
    }

    public override void Dselect()
    {
        isSelected = false;
        UnitInfo.Current.clearLines();
        UnitInfo.Current.clearPic();
    }

    public override void Select()
    {
        isSelected = true;
        UnitInfo.Current.setPic(pic);
        UnitInfo.Current.setLines(unitName, currentHealth + "/" + maxHealth);


    }


}
