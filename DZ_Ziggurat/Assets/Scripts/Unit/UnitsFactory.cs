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
    [SerializeField] private UnitConfiguration[] _unitConfigsSO;
    private List<UnitConfiguration> _unitConfigClones = new List<UnitConfiguration>();
    [SerializeField] private GameObject _defaultTarget;


    private void Start()
    {
        SetInitialData();
    }

    public void CreateUnit()
    {
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
        if (_unitConfigClones.Count < 1)
        {
            foreach (var unitConfiguration in _unitConfigsSO)
            {
                
                _unitConfigClones.Add(GetCloneConfig(unitConfiguration));
            }
        }
    }

    private UnitConfiguration GetCloneConfig(UnitConfiguration config)
    {
        //todo: переписать возможно под дату
        var clone = new UnitConfiguration();

        clone.MaxHealth = config.MaxHealth;
        clone.UnitType = config.UnitType;
        clone.MoveSpeed = config.MoveSpeed;
        clone.FastAttackDamage = config.FastAttackDamage;
        clone.SlowAttackDamage = config.SlowAttackDamage;
        clone.ChanceDoubleDamage = config.ChanceDoubleDamage;
        clone.ChanceMissAttack = config.ChanceMissAttack;
        clone.FrequencyFastAttack = config.FrequencyFastAttack;
        clone.Mass = config.Mass;
        
        return clone;
    }
    private UnitData GetUnitData(UnitConfiguration config)
    {
        UnitData unitData = new UnitData();
        
        unitData.Health = config.MaxHealth;
        unitData.UnitType = config.UnitType;
        unitData.MoveSpeed = config.MoveSpeed;
        unitData.FastAttackDamage = config.FastAttackDamage;
        unitData.SlowAttackDamage = config.SlowAttackDamage;
        unitData.ChanceDoubleDamage = config.ChanceDoubleDamage;
        unitData.ChanceMissAttack = config.ChanceMissAttack;
        unitData.FrequencyFastAttack = config.FrequencyFastAttack;
        unitData.Mass = config.Mass;
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
        foreach (var unitConfiguration in _unitConfigClones)
        {
            if (unitType == unitConfiguration.UnitType)
            {
                return unitConfiguration;
            }
        }
        return null;
    }

    public void SetUpdatedConfiguration(UnitData unitData)
    {
        foreach (var unitConfiguration in _unitConfigClones)
        {
            if (unitData.UnitType == unitConfiguration.UnitType)
            {
                unitConfiguration.MaxHealth = unitData.Health;
                unitConfiguration.MoveSpeed = unitData.MoveSpeed;
                unitConfiguration.FastAttackDamage = unitData.FastAttackDamage;
                unitConfiguration.SlowAttackDamage = unitData.SlowAttackDamage;
                unitConfiguration.ChanceDoubleDamage = unitData.ChanceDoubleDamage;
                unitConfiguration.ChanceMissAttack = unitData.ChanceMissAttack;
                unitConfiguration.FrequencyFastAttack = unitData.FrequencyFastAttack;
                unitConfiguration.Mass = unitData.Mass;
                Debug.Log($"Perezapisal");
            }
        }
    }
}