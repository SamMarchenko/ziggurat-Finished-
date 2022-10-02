using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Ziggurat;

public class SpawnPositions : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private EUnitType _unitType;
    public Action<EUnitType> OnGateClick;

    public EUnitType UnitType => _unitType;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("2");
        Debug.Log(_unitType);
        OnGateClick?.Invoke(_unitType);
    }
}