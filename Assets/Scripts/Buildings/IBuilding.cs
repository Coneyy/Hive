using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Buildings
{
    public interface IBuilding
    {
        GameObject[] Extensions { get; set; }
        GameObject Base { get; set; }

        int Level { get; set; }
        int MaxLevel { get; }
        int Health { get; set; }
        int MaxHealth { get; set; }
        int UnitCost { get; set; }
        int RepairCostPerHp { get; set; }
        int FullRepairCost { get; }
        int UpgradeCost { get; set; }


        bool Upgrade();
        void SpawnUnit();
        void Repair(int percentage);

    }
}
