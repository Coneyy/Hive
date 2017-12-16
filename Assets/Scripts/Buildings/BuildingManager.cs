using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


    public class BuildingManager : MonoBehaviour
    {


	public GameObject Building;

	private IBuilding _building
        {
            get { return Building.GetComponent<IBuilding>(); }
        }

        public void UpgradeBuilding()
        {
		if (_building.UpgradeCost > RtsManager.Current.getCredits())
            {
                throw new BuildingException("Zbyt mało zasobów na ulepszenie");
            }

            if (!_building.Upgrade())
            {
                throw new BuildingException("Poziom budynku jest maksymalny");
            }

		RtsManager.Current.setCredits(RtsManager.Current.getCredits()-_building.UpgradeCost);
        }

        public void RepairBuilding()
        {

		if (RtsManager.Current.getCredits() >= _building.FullRepairCost)
            {
			RtsManager.Current.setCredits(RtsManager.Current.getCredits()-_building.FullRepairCost);
			_building.Repair(100);                
            }
            else
            {
			var repairPercentage = RtsManager.Current.getCredits() / _building.RepairCostPerHp;
               var repairValue = repairPercentage * _building.RepairCostPerHp;
			RtsManager.Current.setCredits(RtsManager.Current.getCredits()-repairValue);
			_building.Repair(repairPercentage);
            }
        }

        public void SpawnUnit()
        {
		if (_building.UnitCost > RtsManager.Current.getCredits())
            {
                throw new BuildingException("Zbyt mało zasobów na stworzenie jednostki");
            }

            _building.SpawnUnit();
		RtsManager.Current.setCredits(RtsManager.Current.getCredits()-_building.UnitCost);
	   }
    }

    public class BuildingException : Exception
    {
        public BuildingException(string message) : base(message)
        {

        }
    }

