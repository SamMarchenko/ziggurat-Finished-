using System;
using UnityEngine;
using UnityEngine.UI;
using Ziggurat;

public class GateSettingsView : MonoBehaviour
{
    /// <summary>
    /// Placeholders
    /// </summary>
    [SerializeField] private Text _healthPlaceholderText;

    [SerializeField] private Text _moveSpeedPlaceholderText;
    [SerializeField] private Text _fastAttackDamagePlaceholderText;
    [SerializeField] private Text _slowAttackDamagePlaceholderText;
    [SerializeField] private Text _chanceDDPlaceholderText;
    [SerializeField] private Text _chanceMissAttackPlaceholderText;
    [SerializeField] private Text _frequencyFastAttackPlaceholderText;
    [SerializeField] private Text _unitMassPlaceholderText;

    /// <summary>
    /// Values
    /// </summary>
    [SerializeField] private Text _unitTypeValue;

    public Text MaxHealthValue;
    public Text MoveSpeedValue;
    public Text FastAttackValue;
    public Text SlowAttackValue;
    public Text ChanceDDValue;
    public Text ChanceMissAttackValue;
    public Text FrequencyFastAttackValue;
    public Text UnitMassValue;

    [SerializeField] private Button _updateDataButton;
    [SerializeField] private Button _closeButton;

    public Button UpdateDataButton => _updateDataButton;
    public Button CloseButton => _closeButton;

    private void Start()
    {
        SetDefaultPlaceHolderText();
    }

    private void SetDefaultPlaceHolderText()
    {
        _healthPlaceholderText.text = ">= 1";
        _moveSpeedPlaceholderText.text = "Range (5,15)";
        _fastAttackDamagePlaceholderText.text = "> 0";
        _slowAttackDamagePlaceholderText.text = "> 0";
        _chanceDDPlaceholderText.text = "Range (0, 100)";
        _chanceMissAttackPlaceholderText.text = "Range (0, 100)";
        _frequencyFastAttackPlaceholderText.text = "Range (0, 100)";
        _unitMassPlaceholderText.text = "Range (50, 100)";
    }

    public void SetCurrentUnitData(UnitConfiguration config)
    {
        ClearValues();
        _unitTypeValue.text = config.UnitType.ToString();
        _healthPlaceholderText.text = config.MaxHealth.ToString();
        _moveSpeedPlaceholderText.text = config.MoveSpeed.ToString();
        _fastAttackDamagePlaceholderText.text = config.FastAttackDamage.ToString();
        _slowAttackDamagePlaceholderText.text = config.SlowAttackDamage.ToString();
        _chanceDDPlaceholderText.text = config.ChanceDoubleDamage.ToString();
        _chanceMissAttackPlaceholderText.text = config.ChanceMissAttack.ToString();
        _frequencyFastAttackPlaceholderText.text = config.FrequencyFastAttack.ToString();
        _unitMassPlaceholderText.text = config.Mass.ToString();
    }

    public void SubscribeUpdateButton(Action onUnitData)
    {
        _updateDataButton.onClick.AddListener(() => onUnitData());
    }
    
    public void SubscribeCloseButton(Action onCloseButton)
    {
        _closeButton.onClick.AddListener(() => onCloseButton());
    }
   

    private void ClearValues()
    {
        MaxHealthValue.text = string.Empty;
        MoveSpeedValue.text = string.Empty;
        FastAttackValue.text = string.Empty;;
        SlowAttackValue.text = string.Empty;;
        ChanceDDValue.text = string.Empty;;
        ChanceMissAttackValue.text = string.Empty;;
        FrequencyFastAttackValue.text = string.Empty;;
        UnitMassValue.text = string.Empty;;
    }
}