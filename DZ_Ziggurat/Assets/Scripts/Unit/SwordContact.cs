using System;
using UnityEngine;

namespace Ziggurat
{
    public class SwordContact : MonoBehaviour
    {
        private EUnitType _unitType;

        public Action<UnitBehaviour> SwordTargetContact;

        public void SetUnitType(EUnitType unitType)
        {
            _unitType = unitType;
        }
        private void OnTriggerEnter(Collider other)
        {
            var unit = other.gameObject.GetComponent<UnitBehaviour>();
            if (unit == null) return;
            if (unit.UnitType != _unitType)
            {
                SwordTargetContact?.Invoke(unit);
            }
        }
    }
}