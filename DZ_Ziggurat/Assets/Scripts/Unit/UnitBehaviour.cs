using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ziggurat;
using Random = UnityEngine.Random;

public class UnitBehaviour : MonoBehaviour
{
    [SerializeField] private EStateType _unitState;
    [SerializeField] private EUnitType _unitType;[SerializeField] private UnitBehaviour _target;
    [SerializeField] private GameObject _defaultTarget;
    [SerializeField, Space] private TargetFinder _targetFinder;
    [SerializeField] private SphereCollider _sphere;
    
    private UnitEnvironment _unitEnvironment;
    private Rigidbody _rigidbody;
    private UnitData _unitData;
    
    public EUnitType UnitType => _unitType;
    public UnitBehaviour Target => _target;


    public void Init( GameObject defaultTarget, UnitData unitData)
    {
        _unitData = unitData;
        _defaultTarget = defaultTarget;
        _rigidbody = GetComponent<Rigidbody>();
        _unitEnvironment = GetComponent<UnitEnvironment>();
        _unitState = EStateType.Move;
        _targetFinder.SetUnitType(unitData.UnitType);
        _sphere.enabled = true;
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
                break;
            case EStateType.FastAttack:
            case EStateType.StrongAttack:
                OnAttack(_target);
                break;
            case EStateType.Die:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void SetNearestTarget()
    {
        _sphere.enabled = _target == null;
        if (_target != null) return;
        _target = _targetFinder.GetNearestTarget();
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

        var desired_velocity = (targetPosition - transform.position).normalized * _unitData.MoveSpeed;
        var steering = desired_velocity - GetVelocity(EIgnoreAxisType.Y);

        steering = Vector3.ClampMagnitude(steering, _unitData.MoveSpeed) / _unitData.Mass;
        var velocity = Vector3.ClampMagnitude(GetVelocity() + steering, _unitData.MoveSpeed);

        SetVelocity(velocity);
    }
    
    private void OnAttack(UnitBehaviour target)
    {
        //if (!isFinishAttack) return;
        
        var unitBehaviour = target.GetComponent<UnitBehaviour>();
        if (_unitState == EStateType.FastAttack)
        {
            unitBehaviour.ApplyDamage(_unitData.FastAttackDamage);
            _unitEnvironment.StartAnimation("Fast");
        }
        else
        {
            unitBehaviour.ApplyDamage(_unitData.SlowAttackDamage);
            _unitEnvironment.StartAnimation("Strong");
        }

        //isFinishAttack = false;
        _unitState = EStateType.Move;
        //if (target != null) return;
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
            //Dead?.Invoke(this);
        }

        Debug.Log($"{_unitData.UnitType} health = {_unitData.Health}");
    }
    private void RandomAttackState()
    {
        var random = Random.Range(0.0f, 1.0f);
        if (random < _unitData.FrequencyFastAttack)
        {
            _unitState = EStateType.FastAttack;
        }
        else
        {
            _unitState = EStateType.StrongAttack;
        }
    }
}