using UnityEngine;
using Ziggurat;

[CreateAssetMenu(fileName = "BaseUnitConfiguration", menuName = "UnitsConfiguration", order = 3)]
public class UnitConfiguration : ScriptableObject
{
    [SerializeField] private EUnitType _unitType;
    public EUnitType UnitType => _unitType;
    [SerializeField] private float _maxHealth;
    [SerializeField,Range(5,20)] private float _moveSpeed;
    [SerializeField] private float _fastAttackDamage;
    [SerializeField] private float _slowAttackDamage;
    [SerializeField, Range(0, 100)] private float _chanceDoubleDamage;
    [SerializeField, Range(0, 100)] private float _chanceMissAttack;
    [SerializeField, Range(0, 100)] private float _frequencyFastAttack;
    [SerializeField,Range(50,150)] float _mass;

    public float GetMaxHealth => _maxHealth;
    public float GetMoveSpeed => _moveSpeed;
    public float GetFastAttackDamage => _fastAttackDamage;
    public float GetSlowAttackDamage => _slowAttackDamage;
    public float GetChanceDoubleDamage => _chanceDoubleDamage;
    public float GetChanceMissAttack => _chanceMissAttack;
    public float GetFrequencyFastAttack => _frequencyFastAttack;
    public float GetMass => _mass;
    
}