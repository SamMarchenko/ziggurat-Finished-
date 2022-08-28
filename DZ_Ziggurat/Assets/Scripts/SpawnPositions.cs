using System;
using UnityEngine;
using Ziggurat;

public class SpawnPositions : MonoBehaviour
{
    [SerializeField] private EUnitType _unitType;

    public EUnitType UnitType => _unitType;
    
    public Action<EUnitType> PressedOnGate;

    private void OnMouseDown()
    {
        PressedOnGate?.Invoke(_unitType);
    }
}
