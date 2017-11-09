using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


    public class BuildingManager : MonoBehaviour
    {

	public void Update()
	{

		if (Player.DefaultPlayer!= null) {
			gold = Player.DefaultPlayer.Credits;
		}
	}

	public GameObject Building;
	int gold = 0;

	private IBuilding _building
        {
            get { return Building.GetComponent<IBuilding>(); }
        }

        public void UpgradeBuilding()
        {
            if (_building.UpgradeCost > gold)
            {
                throw new BuildingException("Zbyt mało zasobów na ulepszenie");
            }

            if (!_building.Upgrade())
            {
                throw new BuildingException("Poziom budynku jest maksymalny");
            }

            gold -= _building.UpgradeCost;
        }

        public void RepairBuilding()
        {

            if (gold >= _building.FullRepairCost)
            {
                gold -= _building.FullRepairCost;
                _building.Repair(100);                
            }
            else
            {
                var repairPercentage = gold / _building.RepairCostPerHp;
                var repairValue = repairPercentage * _building.RepairCostPerHp;
                gold -= repairValue;
                _building.Repair(repairPercentage);
            }
        }

        public void SpawnUnit()
        {
            if (_building.UnitCost > gold)
            {
                throw new BuildingException("Zbyt mało zasobów na stworzenie jednostki");
            }

            _building.SpawnUnit();
            gold -= _building.UnitCost;
        }
    }

    public class BuildingException : Exception
    {
        public BuildingException(string message) : base(message)
        {

        }
    }

