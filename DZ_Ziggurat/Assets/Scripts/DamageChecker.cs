using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ziggurat;

public class DamageChecker : MonoBehaviour
{
    [SerializeField] private UnitEnvironment _unitEnvironment;
    [SerializeField] private List<UnitBehaviour> _unitBehaviours;
    public List<UnitBehaviour> UnitBehaviours => _unitBehaviours;
    
    public Action<List<UnitBehaviour>> ContactedUnits;

    private void Start()
    {
       _unitEnvironment.ColliderIsOff += ColliderIsOff;
    }

    private void ColliderIsOff()
    {
        _unitBehaviours.Clear();
    }

    private void OnCollisionEnter(Collision other)
    {
        var unitBehaviour = other.gameObject.GetComponent<UnitBehaviour>();
        if (!_unitBehaviours.Contains(unitBehaviour))
        {
            _unitBehaviours.Add(unitBehaviour);
        }
        ContactedUnits?.Invoke(_unitBehaviours);
    }
}
