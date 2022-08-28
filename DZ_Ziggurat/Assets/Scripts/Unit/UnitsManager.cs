using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ziggurat
{
    public class UnitsManager : MonoBehaviour
    {
        [SerializeField] private UnitsFactory _unitFactory;
        [SerializeField] float _createCoolDown = 0f;
        [SerializeField] private UnitData[] _unitsData;
        private List<SpawnPositions> _spawnPositions = new List<SpawnPositions>();


        private void Start()
        {
            SetDatasForFactory();
            GetAllSpawnPositions();
            foreach (var spawnPosition in _spawnPositions)
            {
                spawnPosition.PressedOnGate += PressedOnGate;
            }

            CreateUnits();
        }

        private void SetDatasForFactory()
        {
            foreach (var unitData in _unitsData)
            {
                _unitFactory.SetUnitConfiguration(unitData);
            }
        }

        private void PressedOnGate(EUnitType unitType)
        {
            //todo: вывод конфигов юнитов в UI
            Debug.Log(unitType);
        }

        // private void Update()
        // {
        //     _createCoolDown -= Time.deltaTime;
        //     CreateUnits();
        // }
        private void GetAllSpawnPositions()
        {
            foreach (var spawnPosition in _unitFactory.SpawnPositions)
            {
                _spawnPositions.Add(spawnPosition);
            }
        }

        private void CreateUnits()
        {
            if (_createCoolDown > 0)
                return;
            _unitFactory.CreateUnit();
            _createCoolDown = 3f;
        }

        private void OnDisable()
        {
            foreach (var spawnPosition in _spawnPositions)
            {
                spawnPosition.PressedOnGate -= PressedOnGate;
            }
        }
    }
}