using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ziggurat;

public class UnitsFactory : MonoBehaviour
{
    [SerializeField] private UnitBehaviour[] _units;
    [SerializeField] private SpawnPositions[] _spawnPositions;
    [SerializeField] private UnitConfiguration[] _unitsData;
    [SerializeField] private GameObject _defaultTarget;

    public void CreateUnit()
    {
        foreach (var spawnPosition in _spawnPositions)
        {
            var unit = Instantiate(GetUnitForCreation(spawnPosition.UnitType),
                spawnPosition.transform.position + new Vector3(0, 8, 0),
                Quaternion.identity);
            var unitData = GetUnitData(unit.UnitType);
            unit.Init(unitData.GetMoveSpeed, _defaultTarget, unitData.GetMass);
            unit.transform.LookAt(Vector3.zero);
        }
    }

    private UnitBehaviour GetUnitForCreation(EUnitType spawnUnitType)
    {
        foreach (var unit in _units)
        {
            if (spawnUnitType == unit.UnitType)
                return unit;
        }

        return null;
    }

    private UnitConfiguration GetUnitData(EUnitType unitType)
    {
        foreach (var unitData in _unitsData)
        {
            if (unitType == unitData.UnitType)
            {
                return unitData;
            }
        }
        return null;
    }
}