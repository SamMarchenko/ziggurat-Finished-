using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ziggurat
{
    public class TargetFinder : MonoBehaviour
    {
        [SerializeField] private List<UnitBehaviour> _targets = new List<UnitBehaviour>(0);
        private EUnitType _unitType;

        public void SetUnitType(EUnitType unitType)
        {
            _unitType = unitType;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Unit")) return;
            var unit = other.gameObject.GetComponent<UnitBehaviour>();
            if (unit == null) return;
            if (unit.UnitType != _unitType && !_targets.Contains(unit))
            {
                _targets.Add(unit);
            }
        }

        public UnitBehaviour GetNearestTarget()
        {
            UnitBehaviour unit = null;
            if (_targets.Count < 1)
            {
                _targets.Clear();
                return unit;
            }

            var nearest = Mathf.Infinity;

            foreach (var target in _targets)
            {
                if (target == null)
                {
                    continue;
                }
                var distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance < nearest)
                {
                    nearest = distance;
                    unit = target;
                }
            }
            _targets.Clear();
            return unit;
        }
    }
}