using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private Text _maxHealthValue;
    [SerializeField] private Text _moveSpeedValue;
    [SerializeField] private Text _fastAttackValue;
    [SerializeField] private Text _slowAttackValue;
    [SerializeField] private Text _chanceDDValue;
    [SerializeField] private Text _chanceMissAttackValue;
    [SerializeField] private Text _frequencyFastAttackValue;
    [SerializeField] private Text _unitMassValue;
    

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
    
}
