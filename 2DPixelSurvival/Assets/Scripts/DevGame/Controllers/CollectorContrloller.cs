using System;
using System.Collections.Generic;
using UnityEngine;

namespace DevSystems
{
    public class CollectorContrloller : MonoBehaviour
    {
        public event Action<ISelectable> PickUp; 
        
        [SerializeField] private Transform _selectableObjectsParent;
        
        private List<HandEquipment> _сollectionSelecatable = new();

        private void Awake()
        {
            _сollectionSelecatable = new List<HandEquipment>(_selectableObjectsParent.GetComponentsInChildren<HandEquipment>());

            if (_сollectionSelecatable is not null)
            {
                foreach (var selectable in _сollectionSelecatable)
                    selectable.PickUp += OnPickUpHandler;
            }
        }

        private void OnDestroy()
        {
            if (_сollectionSelecatable is not null)
            {
                foreach (var selectable in _сollectionSelecatable)
                    selectable.PickUp -= OnPickUpHandler;
            }
        }

        private void OnPickUpHandler(ISelectable obj)
        {
            PickUp?.Invoke(obj);
        }
    }
}