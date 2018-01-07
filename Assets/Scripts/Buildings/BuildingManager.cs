using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class BuildingManager : MonoBehaviour
{
    public static event Action GameOver;

    public GameObject Building;
    public GameObject BuildingInfo;
    public GameObject ResourcesInfo;
    public GameObject LevelInfo;

    private float time = 0;
    private bool isMessage;

	public IBuilding _building
    {
        get { return Building.GetComponent<IBuilding>(); }
    }

    public void UpgradeBuilding()
    {
        if (_building.UpgradeCost > RtsManager.Gold)
        {
            BuildingInfo.GetComponent<Text>().text = "Zbyt mało zasobów na ulepszenie";
            isMessage = true;
            return;
            //throw new BuildingException("Zbyt mało zasobów na ulepszenie");            
        }

        if (!_building.Upgrade())
        {
            BuildingInfo.GetComponent<Text>().text = "Poziom budynku jest maksymalny";
            isMessage = true;
            return;
            //throw new BuildingException("Poziom budynku jest maksymalny");
        }

        RtsManager.Gold -=_building.UpgradeCost;
        LevelInfo.GetComponent<Text>().text = "Poziom " + _building.Level;
        ResourcesInfo.GetComponent<Text>().text = "+ " +( (_building.Level + 1) * 50).ToString();
        
    }

    public void Start()
    {
        BaseBuilding.Destroyed += BuildingDestroyed;
        ResourcesInfo.GetComponent<Text>().text = "+ " + ((_building.Level + 1) * 50).ToString();
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
            BuildingInfo.GetComponent<Text>().text = "Zbyt mało zasobów na stworzenie jednostki";
            isMessage = true;
            return;
            //throw new BuildingException("Zbyt mało zasobów na stworzenie jednostki");

        }

        _building.SpawnUnit();
        RtsManager.Gold -= _building.UnitCost;
    }

    private void BuildingDestroyed()
    {
        GameOver();
    }

    public void Update()
    {
       time+=Time.deltaTime;

        if (isMessage && time > 4)
        {
            time = 0;
            BuildingInfo.GetComponent<Text>().text = "";
            isMessage = false;
        }
    }
}

public class BuildingException : Exception
{
    public BuildingException(string message) : base(message)
    {

    }
}

