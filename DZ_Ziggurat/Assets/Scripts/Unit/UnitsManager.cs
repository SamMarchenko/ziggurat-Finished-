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
        [SerializeField] private RectTransform _gateWindowUI;
        private UnitConfiguration _currentConfig;
        private EUnitType _currentUnitType;
        private bool _isUIOpen;
        private Vector3 _closedUIPos;
        private Vector3 _openedUIPos;
        private bool _onAnimation;
        private float _animationSpeed = 1;
        


        private void Start()
        {
            _closedUIPos = _gateWindowUI.localPosition;
            _openedUIPos = _closedUIPos + new Vector3(0, -480, 0);
            
            _gateSettingsView.SubscribeUpdateButton(SetCurrentUnitDatas);
            _gateSettingsView.SubscribeCloseButton(StartCloseAnimation);

            foreach (var SpawnPosition in _unitFactory.SpawnPositions)
            {
                SpawnPosition.OnGateClick += OnGateClick;
            }
        }

        private void StartCloseAnimation()
        {
            ClearValues();
            if (!_onAnimation)
            {
                StartCoroutine(MoveRoutine(_openedUIPos, _closedUIPos, _animationSpeed));
            }
            //анимации ui через аниматор
            //_uiAnimator.PlayClosed();
        }
        private void SetCurrentUnitDatas()
        {
            //todo: реализовать передачу параметров из вьюхи в конфиги юнитов. Сделать проверку в сеттерах на корректный и не пустой ввод

            var data = new UnitData();
            data.UnitType = _currentUnitType;
            data.Health = string.IsNullOrEmpty(_gateSettingsView.MaxHealthInputField.text)
                ? _currentConfig.MaxHealth
                : Convert.ToSingle(_gateSettingsView.MaxHealthInputField.text);
            data.MoveSpeed = string.IsNullOrEmpty(_gateSettingsView.MoveSpeedInputField.text)
                ? _currentConfig.MoveSpeed
                : Convert.ToSingle(_gateSettingsView.MoveSpeedInputField.text);
            data.FastAttackDamage = string.IsNullOrEmpty(_gateSettingsView.FastAttackInputField.text)
                ? _currentConfig.FastAttackDamage
                : Convert.ToSingle(_gateSettingsView.FastAttackInputField.text);
            data.SlowAttackDamage = string.IsNullOrEmpty(_gateSettingsView.SlowAttackInputField.text)
                ? _currentConfig.SlowAttackDamage
                : Convert.ToSingle(_gateSettingsView.SlowAttackInputField.text);
            data.ChanceDoubleDamage = string.IsNullOrEmpty(_gateSettingsView.ChanceDDInputField.text)
                ? _currentConfig.ChanceDoubleDamage
                : Convert.ToSingle(_gateSettingsView.ChanceDDInputField.text);
            data.ChanceMissAttack = string.IsNullOrEmpty(_gateSettingsView.ChanceMissAttackInputField.text)
                ? _currentConfig.ChanceMissAttack
                : Convert.ToSingle(_gateSettingsView.ChanceMissAttackInputField.text);
            data.FrequencyFastAttack = string.IsNullOrEmpty(_gateSettingsView.FrequencyFastAttackInputField.text)
                ? _currentConfig.FrequencyFastAttack
                : Convert.ToSingle(_gateSettingsView.FrequencyFastAttackInputField.text);
            data.Mass = string.IsNullOrEmpty(_gateSettingsView.UnitMassInputField.text)
                ? _currentConfig.Mass
                : Convert.ToSingle(_gateSettingsView.UnitMassInputField.text);

            _unitFactory.SetUpdatedConfiguration(data);
            if (!_onAnimation)
            {
                StartCoroutine(MoveRoutine(_openedUIPos, _closedUIPos, _animationSpeed));
            }
            //анимации ui через аниматор
            //_uiAnimator.PlayClosed();
        }
        
        private void OnGateClick(EUnitType type)
        {
            ClearValues();
            //анимации ui через аниматор
            //_uiAnimator.PlayOpened();
            if (!_onAnimation) 
            {
                StartCoroutine(MoveRoutine(_closedUIPos, _openedUIPos, _animationSpeed)); 
            }
            _currentUnitType = type;
            SetUnitDatasForSettingView(type);
        }
        
        
        private IEnumerator MoveRoutine(Vector3 startPosition, Vector3 endPosition, float time)
        {
            _onAnimation = true;
            InteractableUI(_onAnimation);
            var currentTime = 0f;
            while (currentTime < time)
            {
                _gateWindowUI.localPosition = Vector3.Lerp(startPosition, endPosition, 1 - (time - currentTime) / time);
                currentTime += Time.deltaTime;
                yield return null;
            }
            _gateWindowUI.localPosition = endPosition;
            _onAnimation = false;
            InteractableUI(_onAnimation);
        }

        private void InteractableUI(bool state)
        {
            _gateSettingsView.UpdateDataButton.interactable = !state;
            _gateSettingsView.CloseButton.interactable = !state;
            _gateSettingsView.MaxHealthInputField.interactable = !state;
            _gateSettingsView.MoveSpeedInputField.interactable = !state;
            _gateSettingsView.FastAttackInputField.interactable = !state;
            _gateSettingsView.SlowAttackInputField.interactable = !state;
            _gateSettingsView.ChanceDDInputField.interactable = !state;
            _gateSettingsView.ChanceMissAttackInputField.interactable = !state;
            _gateSettingsView.FrequencyFastAttackInputField.interactable = !state;
            _gateSettingsView.UnitMassInputField.interactable = !state;
        }
        
        private void ClearValues()
        {
            _gateSettingsView.MaxHealthInputField.text = string.Empty;
            _gateSettingsView.MoveSpeedInputField.text = string.Empty;
            _gateSettingsView.FastAttackInputField.text = string.Empty;
            _gateSettingsView.SlowAttackInputField.text = string.Empty;
            _gateSettingsView.ChanceDDInputField.text = string.Empty;
            _gateSettingsView.ChanceMissAttackInputField.text = string.Empty;
            _gateSettingsView.FrequencyFastAttackInputField.text = string.Empty;
            _gateSettingsView.UnitMassInputField.text = string.Empty;
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
            _gateSettingsView.CloseButton.onClick.RemoveAllListeners();
        }
    }
}