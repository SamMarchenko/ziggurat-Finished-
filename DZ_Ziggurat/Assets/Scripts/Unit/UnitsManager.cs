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