using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ziggurat
{
    public class SwordContact : MonoBehaviour
    {
        private EUnitType _unitType;
        private List<UnitBehaviour> _contacts = new List<UnitBehaviour>();
        [SerializeField]
        public Action<UnitBehaviour> SwordTargetContact;

        public void SetUnitType(EUnitType unitType)
        {
            _unitType = unitType;
        }
        private void OnTriggerEnter(Collider other)
        {
            var unit = other.gameObject.GetComponent<UnitBehaviour>();
            if (unit == null || _contacts.Contains(unit)) return;
            _contacts.Add(unit);
            if (unit.UnitType != _unitType)
            {
                SwordTargetContact?.Invoke(unit);
            }
        }
        
        public void ClearContacts()
        {
            _contacts.Clear();
        }
    }
}