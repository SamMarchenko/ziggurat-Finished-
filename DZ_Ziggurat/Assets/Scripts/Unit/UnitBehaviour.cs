using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ziggurat;
using Random = UnityEngine.Random;

public class UnitBehaviour : MonoBehaviour
{
    [SerializeField] private EStateType _unitState;
    public EStateType UnitState => _unitState;
    [SerializeField] private EAttackState _attackState;
    [SerializeField] private EUnitType _unitType;
    [SerializeField] private UnitBehaviour _target;
    [SerializeField] private GameObject _defaultTarget;
    [SerializeField, Space] private TargetFinder _targetFinder;
    [SerializeField] private SphereCollider _sphere;
    [SerializeField, Space] private SwordContact _swordContact;
    private bool AttackAnimationEnd = true;

    private UnitEnvironment _unitEnvironment;
    private Rigidbody _rigidbody;
    private UnitData _unitData;

    public EUnitType UnitType => _unitType;
    public UnitBehaviour Target => _target;
    public Action Die;


    public void Init(GameObject defaultTarget, UnitData unitData)
    {
        _unitData = unitData;
        _defaultTarget = defaultTarget;
        _rigidbody = GetComponent<Rigidbody>();
        _unitEnvironment = GetComponent<UnitEnvironment>();
        _unitEnvironment.SwordColliderIsOff += SwordColliderIsOff;
        _unitState = EStateType.Move;
        _targetFinder.SetUnitType(unitData.UnitType);
        _sphere.enabled = true;
        _swordContact.SetUnitType(unitData.UnitType);
        _swordContact.SwordTargetContact += SetTargetDamage;
        _unitEnvironment.AttackAnimationEnd += AttackAnimation;
        _unitEnvironment.UnitDied += UnitDied;
    }

    private void UnitDied()
    {
        Destroy(gameObject, 2f);
    }

    private void AttackAnimation(string result)
    {
        if (result == "End1" || result == "End2")
        {
            AttackAnimationEnd = true;
        }
    }

    private void SwordColliderIsOff()
    {
        _swordContact.ClearContacts();
    }

    private void Update()
    {
        CheckUnitState();
    }

    private void CheckUnitState()
    {
        switch (_unitState)
        {
            case EStateType.Move:
                OnSeek();
                SetNearestTarget();
                CheckTargetDistance();
                break;
            case EStateType.Attack:
                if (_target == null || _target.UnitState == EStateType.Die)
                {
                    _unitState = EStateType.Move;
                    break;
                }
                if (!AttackAnimationEnd) break;
                OnAttack(_target);
                break;
            case EStateType.Die:
                _unitEnvironment.StartAnimation("Die");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void CheckTargetDistance()
    {
        if (_target == null) return;
        var distance = Vector3.Distance(transform.position, _target.transform.position);
        if (distance <= 5f)
        {   
            _unitState = EStateType.Attack;
        }
    }

    private void SetNearestTarget()
    {
        _sphere.enabled = _target == null;
        if (_target != null) return;
        _target = _targetFinder.GetNearestTarget();
        if (_target != null)
        {
            _target.Die += OnTargetDied;
        }
    }

    private void OnTargetDied()
    {
        _target.Die -= OnTargetDied;
        _target = null;
        _unitState = EStateType.Move;
    }

    public Vector3 GetVelocity(EIgnoreAxisType ignore = EIgnoreAxisType.None)
    {
        return UpdateIgnoreAxis(_rigidbody.velocity, ignore);
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

    public void OnSeek()
    {
        _unitEnvironment.Moving(1);

        var targetPosition = _target == null ? _defaultTarget.transform.position : _target.transform.position;
        transform.LookAt(targetPosition);
        var desired_velocity = (targetPosition - transform.position).normalized * _unitData.MoveSpeed;
        var steering = desired_velocity - GetVelocity(EIgnoreAxisType.Y);

        steering = Vector3.ClampMagnitude(steering, _unitData.MoveSpeed) / _unitData.Mass;
        var velocity = Vector3.ClampMagnitude(GetVelocity() + steering, _unitData.MoveSpeed);

        SetVelocity(velocity);
    }

    private void OnAttack(UnitBehaviour target)
    {
        RandomAttackState();
        switch (_attackState)
        {
            case EAttackState.FastAttack:
                _unitEnvironment.StartAnimation("Fast");
                AttackAnimationEnd = false;
                break;
            case EAttackState.SlowAttack:
                _unitEnvironment.StartAnimation("Strong");
                AttackAnimationEnd = false;
                break;
        }
    }

    private void SetTargetDamage(UnitBehaviour unit)
    {
        if (unit != _target) return;
        switch (_attackState)
        {
            case EAttackState.FastAttack:

                unit.ApplyDamage(_unitData.FastAttackDamage);
                break;
            case EAttackState.SlowAttack:
                Debug.Log($"{unit.name} health = {_unitData.Health} - {_unitData.SlowAttackDamage} Strong");
                unit.ApplyDamage(_unitData.SlowAttackDamage);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void ApplyDamage(float damage)
    {
        if (_unitData.Health - damage > 0)
        {
            _unitData.Health -= damage;
        }
        else
        {
            _unitData.Health = 0;
            _unitState = EStateType.Die;
            Die?.Invoke();
        }

        Debug.Log($"{gameObject.name} health = {_unitData.Health} - {_unitData.FastAttackDamage} Fast");
//      Debug.Log($"{_unitData.UnitType} health = {_unitData.Health}");
    }

    private void RandomAttackState()
    {
        var random = Random.Range(0.0f, 1.0f);

        _attackState = random < _unitData.FrequencyFastAttack / 100 ? EAttackState.FastAttack : EAttackState.SlowAttack;
        Debug.Log($"{random} + {_attackState}");
    }
}