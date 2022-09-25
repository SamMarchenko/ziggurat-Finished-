using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ziggurat;

public class SpawnPositions : MonoBehaviour
{
    [SerializeField] private EUnitType _unitType;
    public Action<EUnitType> OnGateClick;

    public EUnitType UnitType => _unitType;

    private void OnMouseDown()
    {
        Debug.Log(_unitType);
        OnGateClick?.Invoke(_unitType);
    }
}
