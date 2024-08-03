using System;
using System.Collections.Generic;
using UnityEngine;

namespace DevSystems
{
    public class Collector : MonoBehaviour
    {
        public event Action<ISelectable> PickUp; 
        public List<HandEquipment> CollectionSelecatable = new();

        private void Awake()
        {
            foreach (var selectable in CollectionSelecatable)
                selectable.PickUp += OnPickUpHandler;
        }

        private void OnDestroy()
        {
            foreach (var selectable in CollectionSelecatable)
                selectable.PickUp -= OnPickUpHandler;
        }

        private void OnPickUpHandler(ISelectable obj)
        {
            PickUp?.Invoke(obj);
            
        }
    }
}