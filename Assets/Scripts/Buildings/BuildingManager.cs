using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class BuildingManager : MonoBehaviour
{
    public static event Action GameOver;

    public GameObject Building;

	public IBuilding _building
    {
        get { return Building.GetComponent<IBuilding>(); }
    }

    public void UpgradeBuilding()
    {
        if (_building.UpgradeCost > RtsManager.Gold)
        {
            throw new BuildingException("Zbyt mało zasobów na ulepszenie");
        }

        if (!_building.Upgrade())
        {
            throw new BuildingException("Poziom budynku jest maksymalny");
        }

        RtsManager.Gold -=_building.UpgradeCost;
    }

    public void Start()
    {
        BaseBuilding.Destroyed += BuildingDestroyed;
    }

    public void RepairBuilding()
    {

        if (RtsManager.Gold >= _building.FullRepairCost)
        {
            RtsManager.Gold -=  _building.FullRepairCost;
            _building.Repair(100);
        }
        else
        {
            var repairPercentage = RtsManager.Gold / _building.RepairCostPerHp;
            var repairValue = repairPercentage * _building.RepairCostPerHp;
            RtsManager.Gold -= repairValue;
            _building.Repair(repairPercentage);
        }
    }

    public void SpawnUnit()
    {
        if (_building.UnitCost > RtsManager.Gold)
        {
            throw new BuildingException("Zbyt mało zasobów na stworzenie jednostki");
        }

        _building.SpawnUnit();
        RtsManager.Gold -= _building.UnitCost;
    }

    private void BuildingDestroyed()
    {
        GameOver();
    }
}

public class BuildingException : Exception
{
    public BuildingException(string message) : base(message)
    {

    }
}

