using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ziggurat
{
    public class UnitsManager : MonoBehaviour
    {
        [SerializeField] private UnitsFactory _unitFactory;
        [SerializeField] float _createCoolDown = 0f;
        [SerializeField] private List<UnitBehaviour> _createdUnits;
        [SerializeField] private GateSettingsView _gateSettingsView;

        private void Start()
        {
            foreach (var SpawnPosition in _unitFactory.SpawnPositions)
            {
                SpawnPosition.OnGateClick += OnGateClick; 
            }
        }

        private void OnGateClick(EUnitType obj)
        {
           //todo: открытие окна параметров юнита
           GetUnitDatasForSettingView(obj);
        }

        private void GetUnitDatasForSettingView(EUnitType unitType)
        {
            var unitConfiguration = _unitFactory.GetUnitConfiguration(unitType);
            _gateSettingsView.GetCurrentUnitData(unitConfiguration);
        }

        private void Update()
        {
            _createCoolDown -= Time.deltaTime;
            CreateUnits();
        }

        private void CreateUnits()
        {
            if (_createCoolDown >0)
                return;
            _unitFactory.CreateUnit();
            _createCoolDown = 5f;
        }
    }
}