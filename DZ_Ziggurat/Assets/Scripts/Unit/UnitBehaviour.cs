using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ziggurat;

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
    [SerializeField] private bool _hasEnemyTarget;


    public void Init(float moveSpeed, GameObject defaultTarget, float mass)
    {
        _moveSpeed = moveSpeed;
        _target = defaultTarget;
        _rigidbody = GetComponent<Rigidbody>();
        _unitEnvironment = GetComponent<UnitEnvironment>();
        _mass = mass;
    }

    private void Update()
    {
        OnSeek();
    }


    public void OnSeek()
    {
        _unitEnvironment.Moving(1);
        if (Target == null) return;

        var desired_velocity = (Target.transform.position - transform.position).normalized * _moveSpeed;
        var steering = desired_velocity - GetVelocity(EIgnoreAxisType.Y);

        steering = Vector3.ClampMagnitude(steering, _moveSpeed) / _mass;
        var velocity = Vector3.ClampMagnitude(GetVelocity() + steering, _moveSpeed);

        SetVelocity(velocity);
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

    GameObject FindClosestEnemy()
    {
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

        return target;
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