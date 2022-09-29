using System;
using System.Collections;
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
        [SerializeField] private UIAnimator _uiAnimator;
        private UnitConfiguration _currentConfig;
        private EUnitType _currentUnitType;
        private bool _isUIOpen;

        

        private void Start()
        {
            _gateSettingsView.SubscribeUpdateButton(SetCurrentUnitDatas);
            _gateSettingsView.SubscribeCloseButton(StartCloseAnimation);

            foreach (var SpawnPosition in _unitFactory.SpawnPositions)
            {
                SpawnPosition.OnGateClick += OnGateClick;
            }
        }

        private void StartCloseAnimation()
        {
            _uiAnimator.PlayClosed();
        }
        private void SetCurrentUnitDatas()
        {
            //todo: реализовать передачу параметров из вьюхи в конфиги юнитов. Сделать проверку в сеттерах на корректный и не пустой ввод

            var data = new UnitData();
            data.UnitType = _currentUnitType;
            data.Health = string.IsNullOrEmpty(_gateSettingsView.MaxHealthValue.text)
                ? _currentConfig.MaxHealth
                : Convert.ToSingle(_gateSettingsView.MaxHealthValue.text);
            data.MoveSpeed = string.IsNullOrEmpty(_gateSettingsView.MoveSpeedValue.text)
                ? _currentConfig.MoveSpeed
                : Convert.ToSingle(_gateSettingsView.MoveSpeedValue.text);
            data.FastAttackDamage = string.IsNullOrEmpty(_gateSettingsView.FastAttackValue.text)
                ? _currentConfig.FastAttackDamage
                : Convert.ToSingle(_gateSettingsView.FastAttackValue.text);
            data.SlowAttackDamage = string.IsNullOrEmpty(_gateSettingsView.SlowAttackValue.text)
                ? _currentConfig.SlowAttackDamage
                : Convert.ToSingle(_gateSettingsView.SlowAttackValue.text);
            data.ChanceDoubleDamage = string.IsNullOrEmpty(_gateSettingsView.ChanceDDValue.text)
                ? _currentConfig.ChanceDoubleDamage
                : Convert.ToSingle(_gateSettingsView.ChanceDDValue.text);
            data.ChanceMissAttack = string.IsNullOrEmpty(_gateSettingsView.ChanceMissAttackValue.text)
                ? _currentConfig.ChanceMissAttack
                : Convert.ToSingle(_gateSettingsView.ChanceMissAttackValue.text);
            data.FrequencyFastAttack = string.IsNullOrEmpty(_gateSettingsView.FrequencyFastAttackValue.text)
                ? _currentConfig.FrequencyFastAttack
                : Convert.ToSingle(_gateSettingsView.FrequencyFastAttackValue.text);
            data.Mass = string.IsNullOrEmpty(_gateSettingsView.UnitMassValue.text)
                ? _currentConfig.Mass
                : Convert.ToSingle(_gateSettingsView.UnitMassValue.text);

            _unitFactory.SetUpdatedConfiguration(data);
            _uiAnimator.PlayClosed();
        }
        
        private void OnGateClick(EUnitType type)
        {
            ClearValues();
            _uiAnimator.PlayOpened();
            _currentUnitType = type;
            SetUnitDatasForSettingView(type);
        }
        
        private void ClearValues()
        {
            _gateSettingsView.MaxHealthValue.text = string.Empty;
            _gateSettingsView.MoveSpeedValue.text = string.Empty;
            _gateSettingsView.FastAttackValue.text = string.Empty;;
            _gateSettingsView.SlowAttackValue.text = string.Empty;;
            _gateSettingsView.ChanceDDValue.text = string.Empty;;
            _gateSettingsView.ChanceMissAttackValue.text = string.Empty;;
            _gateSettingsView.FrequencyFastAttackValue.text = string.Empty;;
            _gateSettingsView.UnitMassValue.text = string.Empty;;
        }

        private void SetUnitDatasForSettingView(EUnitType unitType)
        {
            _currentConfig = _unitFactory.GetUnitConfiguration(unitType);
            _gateSettingsView.SetCurrentUnitData(_currentConfig);
        }

        private void Update()
        {
            _createCoolDown -= Time.deltaTime;
            CreateUnits();
        }

        private void CreateUnits()
        {
            if (_createCoolDown > 0)
                return;
            _unitFactory.CreateUnit();
            _createCoolDown = 5f;
        }

        public void Dispose()
        {
            _gateSettingsView.UpdateDataButton.onClick.RemoveAllListeners();
        }
    }
}