using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ziggurat;

public class UnitsFactory : MonoBehaviour
{
    [SerializeField] private UnitBehaviour[] _units;
    [SerializeField] private SpawnPositions[] _spawnPositions;
    public SpawnPositions[] SpawnPositions => _spawnPositions;
    private List<UnitData> _unitsData = new List<UnitData>();
    [SerializeField] private GameObject _defaultTarget;

    public void SetUnitConfiguration(UnitData unitdata)
    {
        _unitsData.Add(unitdata);
    }

    public void CreateUnit()
    {
        foreach (var spawnPosition in _spawnPositions)
        {
            var unit = Instantiate(GetUnitForCreation(spawnPosition.UnitType),
                spawnPosition.transform.position + new Vector3(0, 8, 0),
                Quaternion.identity);
            var unitData = GetUnitData(unit.UnitType);
            unit.Init(unitData, _defaultTarget);
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

    private UnitData GetUnitData(EUnitType unitType)
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