using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Ziggurat;

public class UnitsFactory : MonoBehaviour
{
    [SerializeField] private UnitBehaviour[] _units;
    [SerializeField] private SpawnPositions[] _spawnPositions;
    public SpawnPositions[] SpawnPositions => _spawnPositions;
    [SerializeField] private UnitConfiguration[] _unitConfigs;
    private List<UnitConfiguration> _unitConfigurations = new List<UnitConfiguration>();
    [SerializeField] private GameObject _defaultTarget;
    

    public void CreateUnit()
    {
        SetInitialData();
        foreach (var spawnPosition in _spawnPositions)
        {
            var unit = Instantiate(GetUnitForCreation(spawnPosition.UnitType),
                spawnPosition.transform.position + new Vector3(0, 8, 0), Quaternion.identity);
            var unitConfiguration = GetUnitConfiguration(unit.UnitType);
            unit.Init(_defaultTarget, GetUnitData(unitConfiguration));
            unit.transform.LookAt(Vector3.zero);
        }
    }

    private void SetInitialData()
    {
        if (_unitConfigurations.Count < 1)
        {
            foreach (var unitConfiguration in _unitConfigs)
            {
                _unitConfigurations.Add(unitConfiguration);
            }
        }
    }

    private UnitData GetUnitData(UnitConfiguration config)
    {
        UnitData unitData = new UnitData();
        
        unitData.Health = config.GetMaxHealth;
        unitData.UnitType = config.UnitType;
        unitData.MoveSpeed = config.GetMoveSpeed;
        unitData.FastAttackDamage = config.GetFastAttackDamage;
        unitData.SlowAttackDamage = config.GetSlowAttackDamage;
        unitData.ChanceDoubleDamage = config.GetChanceDoubleDamage;
        unitData.ChanceMissAttack = config.GetChanceMissAttack;
        unitData.FrequencyFastAttack = config.GetFrequencyFastAttack;
        unitData.Mass = config.GetMass;
        return unitData;
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

    public UnitConfiguration GetUnitConfiguration(EUnitType unitType)
    {
        foreach (var unitConfiguration in _unitConfigurations)
        {
            if (unitType == unitConfiguration.UnitType)
            {
                return unitConfiguration;
            }
        }
        return null;
    }
}