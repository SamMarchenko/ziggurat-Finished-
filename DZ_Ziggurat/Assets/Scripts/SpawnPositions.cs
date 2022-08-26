using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ziggurat;

public class SpawnPositions : MonoBehaviour
{
    [SerializeField] private EUnitType _unitType;

    public EUnitType UnitType => _unitType;
}
