using UnityEngine;
using Ziggurat;

[CreateAssetMenu(fileName = "BaseUnitConfiguration", menuName = "UnitsConfiguration", order = 3)]
public class UnitConfiguration : ScriptableObject
{
    [SerializeField] private EUnitType _unitType;
    public EUnitType UnitType => _unitType;
    [SerializeField] private float _maxHealth;
    [SerializeField, Range(5, 15)] private float _moveSpeed;
    [SerializeField] private float _fastAttackDamage;
    [SerializeField] private float _slowAttackDamage;
    [SerializeField, Range(0, 100)] private float _chanceDoubleDamage;
    [SerializeField, Range(0, 100)] private float _chanceMissAttack;
    [SerializeField, Range(0, 100)] private float _frequencyFastAttack;
    [SerializeField, Range(50, 100)] private float _mass;

    public float MaxHealth
    {
        get => _maxHealth;
        set => _maxHealth = value;
    }

    public float MoveSpeed
    {
        get => _moveSpeed;
        set => _moveSpeed = value;
    }
    public float FastAttackDamage
    {
        get => _fastAttackDamage;
        set => _fastAttackDamage = value;
    }

    public float SlowAttackDamage
    {
        get => _slowAttackDamage;
        set => _slowAttackDamage = value;
    }

    public float ChanceDoubleDamage
    {
        get => _chanceDoubleDamage;
        set => _chanceDoubleDamage = value;
    }

    public float ChanceMissAttack
    {
        get => _chanceMissAttack;
        set => _chanceMissAttack = value;
    }

    public float FrequencyFastAttack
    {
        get => _frequencyFastAttack;
        set => _frequencyFastAttack = value;
    }

    public float Mass
    {
        get => _mass;
        set => _mass = value;
    }
}