using Assets.Scripts.Buildings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBuilding : BuildingInteractive, IBuilding
{
    public GameObject[] _extensions;
    public GameObject _base;

    private NetworkManager networkManager;

    private int level;
    private int health;
    private int maxHealth;
    private int unitCost;
    private int repairCost;
    private int upgradeCost;

    public GameObject[] Extensions
    {
        get
        {
            return _extensions;
        }

        set
        {
            _extensions = value;
        }
    }

    public GameObject Base
    {
        get
        {
            return _base;
        }

        set
        {
            _base = value;
        }
    }

    public int Level
    {
        get
        {
            return level;
        }

        set
        {
            level = value;
        }
    }

    public int Health
    {
        get
        {
            return health;
        }

        set
        {
            health = value;
        }
    }

    public int UnitCost
    {
        get
        {
            return unitCost;
        }

        set
        {
            unitCost = value;
        }
    }

    public int RepairCostPerHp
    {
        get
        {
            return repairCost;
        }

        set
        {
            repairCost = value;
        }
    }

    public int UpgradeCost
    {
        get
        {
            return upgradeCost;
        }

        set
        {
            upgradeCost = value;
        }
    }

    public int MaxLevel
    {
        get
        {
            return _extensions.Length;
        }
    }

    public Vector3 UnitSpawnPosition
    {
        get
        {
            return new Vector3(transform.position.x + 20, transform.position.y, transform.position.z + 20);
        }
    }

    public int MaxHealth
    {
        get
        {
            return maxHealth;
        }

        set
        {
            maxHealth = value;
        }
    }

    public int FullRepairCost
    {
        get
        {
            return (MaxHealth - Health) * RepairCostPerHp; 
        }
    }

    private void Awake()
    {
        level = BuildingSettings.StartingLevel;
        health = BuildingSettings.StartingHealth;
        maxHealth = BuildingSettings.StartingMaxHealth;
        upgradeCost = BuildingSettings.UpgradeCost;
        unitCost = BuildingSettings.UnitCost;
        repairCost = BuildingSettings.RepairCost;

        networkManager = GameObject.Find("Manager").GetComponent<NetworkManager>();
    }

    public void Repair(int percentage)
    {
        if (health + (int)(maxHealth * percentage) <= maxHealth)
        {
            health += (int)(maxHealth * percentage);
        }
        else
        {
            health = maxHealth;
        }
    }

    public void SpawnUnit()
    {
        networkManager.SpawnNewUnit(UnitSpawnPosition);
    }

    public bool Upgrade()
    {
        if (level == MaxLevel)
        {
            return false;
        }

        level++;
        MaxHealth += (int)(MaxHealth * 0.15);
        _extensions[level - 1].SetActive(true);
        return true;
    }
}
