using System;
using System.Collections.Generic;
using UnityEngine;

namespace DevSystems
{
    public class PickUpEvent { public PickUpType PickUpType; }
    
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
            
            EventAggregator.Subscribe<PickUpEvent>(OnPickUpEventHandler);
        }

        private void OnDestroy()
        {
            if (_сollectionSelecatable is not null)
            {
                foreach (var selectable in _сollectionSelecatable)
                    selectable.PickUp -= OnPickUpHandler;
            }
            
            EventAggregator.Unsubscribe<PickUpEvent>(OnPickUpEventHandler);
        }
        
        private void PickUpResourceHandler(ResourceView resourceView)
        {
            switch (resourceView.SelectableType)
            {
                case PickUpType.Axe:
                    break;
                case PickUpType.Pickaxe:
                    break;
                case PickUpType.Sickle:
                    break;
                case PickUpType.Sword:
                    break;
                case PickUpType.Wood:
                    break;
                case PickUpType.Stone:
                    break;
                case PickUpType.Leaf:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnPickUpHandler(ISelectable obj)
        {
            PickUp?.Invoke(obj);
        }
        
        private void OnPickUpEventHandler(object sender, PickUpEvent eventData)
        {
            if (sender is ISelectable selectable)
            {
                PickUp?.Invoke(selectable);

                switch (selectable)
                {
                    case HandEquipment handEquipment:
                        var handEquipPickUp = selectable as HandEquipment;
                        PickUp?.Invoke(handEquipPickUp);
                        break;
                    case ResourceView resourceView:
                        // делаю Event на то что игрок подобрал рессурс и нужено ввести UI щетчик 
                        PickUpResourceHandler(resourceView);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(selectable));
                }
            }
        }
    }

    
}