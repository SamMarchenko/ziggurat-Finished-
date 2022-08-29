using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ziggurat;
using Random = UnityEngine.Random;

public class UnitBehaviour : MonoBehaviour
{
    [SerializeField] private EUnitType _unitType;
    [SerializeField] private List<UnitBehaviour> _enemiesInRadius;
    private UnitEnvironment _unitEnvironment;
    public EUnitType UnitType => _unitType;
    private Rigidbody _rigidbody;
    [SerializeField] private GameObject _target;
    public GameObject Target => _target;
    private float _moveSpeed;
    private float _mass;
    private float _chanceFastAttack;
    private float _fastAttackDamage;
    private float _slowAttackDamage;
    private float _health;
    private UnitData _unitData;
    [SerializeField] private DamageChecker _damageChecker;
    [SerializeField] private bool _hasEnemyTarget;
    [SerializeField] private bool _canAttack;
    [SerializeField] private EAnimationType _currentState;
    [SerializeField] private bool isFinishAttack = true;

    public Action<UnitBehaviour> Dead;


    public void Init(UnitData _unitData, GameObject defaultTarget)
    {
        _moveSpeed = _unitData.GetMoveSpeed;
        _target = defaultTarget;
        _rigidbody = GetComponent<Rigidbody>();
        _unitEnvironment = GetComponent<UnitEnvironment>();
        _mass = _unitData.GetMass;
        _currentState = EAnimationType.Move;
        _chanceFastAttack = _unitData.GetFrequencyFastAttack / 100;
        _fastAttackDamage = _unitData.GetFastAttackDamage;
        _slowAttackDamage = _unitData.GetSlowAttackDamage;
        _health = _unitData.GetMaxHealth;
        _damageChecker.ContactedUnits += ContactedUnits;
        _unitEnvironment.ColliderIsOff += ColliderIsOff;
    }

    private void ColliderIsOff()
    {
        isFinishAttack = true;
    }

    private void ContactedUnits(List<UnitBehaviour> contactedUnits)
    {
        foreach (var contactedUnit in contactedUnits)
        {
            if (contactedUnit == _target)
            {
                OnAttack();
                break;
            }
        }
    }

    private void Update()
    {
        CheckCurrentState();
    }

    private void CheckCurrentState()
    {
        switch (_currentState)
        {
            case EAnimationType.Move:
                OnSeek();
                break;
            case EAnimationType.FastAttack:
            case EAnimationType.StrongAttack:
                OnAttack();
                break;
            case EAnimationType.Die:
                OnDie();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnDie()
    {
        _unitEnvironment.StartAnimation("Die");
    }

    private void OnAttack()
    {
        if (!isFinishAttack) return;
        
        var unitBehaviour = _target.GetComponent<UnitBehaviour>();
        if (_currentState == EAnimationType.FastAttack)
        {
            unitBehaviour.ApplyDamage(_fastAttackDamage);
            _unitEnvironment.StartAnimation("Fast");
        }
        else
        {
            unitBehaviour.ApplyDamage(_slowAttackDamage);
            _unitEnvironment.StartAnimation("Strong");
        }

        isFinishAttack = false;
        
        if (_target != null) return;
        FindClosestEnemy();
        _currentState = EAnimationType.Move;
    }


    public void ApplyDamage(float damage)
    {
        if (_health - damage > 0)
        {
            _health -= damage;
        }
        else
        {
            _health = 0;
            _currentState = EAnimationType.Die;
            Dead?.Invoke(this);
        }

        Debug.Log($"{_unitType} health = {_health}");
    }

    public void OnSeek()
    {
        //if (_canAttack) return;

        if (_target == null)
        {
            _unitEnvironment.Moving(0);
            _target = FindClosestEnemy();
            return;
        }

        _unitEnvironment.Moving(1);
        var desired_velocity = (_target.transform.position - transform.position).normalized * _moveSpeed;
        var steering = desired_velocity - GetVelocity(EIgnoreAxisType.Y);

        steering = Vector3.ClampMagnitude(steering, _moveSpeed) / _mass;
        var velocity = Vector3.ClampMagnitude(GetVelocity() + steering, _moveSpeed);

        SetVelocity(velocity);
        CheckTargetDistance();
    }

    public Vector3 GetVelocity(EIgnoreAxisType ignore = EIgnoreAxisType.None)
    {
        return UpdateIgnoreAxis(_rigidbody.velocity, ignore);
    }

    //todo поиск ближайшего врага
    private void OnTriggerStay(Collider other)
    {
        if (_hasEnemyTarget) return;
        if (!other.CompareTag("Unit")) return;
        var unit = other.GetComponent<UnitBehaviour>();
        if (!_enemiesInRadius.Contains(unit) && _unitType != unit.UnitType)
        {
            _enemiesInRadius.Add(unit);
        }

        if (_enemiesInRadius.Count <= 0) return;
        _target = FindClosestEnemy();
        _hasEnemyTarget = true;
    }

    private void CheckTargetDistance()
    {
        var curDistance = Vector3.Distance(transform.position, _target.transform.position);
        if (curDistance < 10)
        {
            Debug.Log($"{_unitType} PODOSHeL k {_target.GetComponent<UnitBehaviour>()._unitType}");
            RandomAttackState();
        }
    }

    private void RandomAttackState()
    {
        var random = Random.Range(0.0f, 1.0f);
        if (random < _chanceFastAttack)
        {
            _currentState = EAnimationType.FastAttack;
        }
        else
        {
            _currentState = EAnimationType.StrongAttack;
        }
    }


    GameObject FindClosestEnemy()
    {
        if (_enemiesInRadius.Count == 0)
        {
            return null;
        }
        GameObject target = null;
        float distance = Mathf.Infinity;
        var position = transform.position;
        foreach (var enemy in _enemiesInRadius)
        {
            var diff = enemy.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                target = enemy.gameObject;
                distance = curDistance;
            }
        }

        target.GetComponent<UnitBehaviour>().Dead += OnDead;
        return target;
    }

    private void OnDead(UnitBehaviour enemyBehaviour)
    {
        //enemyBehaviour.Dead -= OnDead;
        _target = null;
        _currentState = EAnimationType.Move;
    }

    public void SetVelocity(Vector3 velocity, EIgnoreAxisType ignore = EIgnoreAxisType.None)
    {
        _rigidbody.velocity = UpdateIgnoreAxis(velocity, ignore);
    }

    private Vector3 UpdateIgnoreAxis(Vector3 velocity, EIgnoreAxisType ignore)
    {
        if ((ignore & EIgnoreAxisType.None) == EIgnoreAxisType.None) return velocity;
        if ((ignore & EIgnoreAxisType.X) == EIgnoreAxisType.X) velocity.x = 0;
        else if ((ignore & EIgnoreAxisType.Y) == EIgnoreAxisType.Y) velocity.y = 0;
        else if ((ignore & EIgnoreAxisType.Z) == EIgnoreAxisType.Z) velocity.z = 0;
        return velocity;
    }
}