using System;
using DevPlayer;
using UnityEngine;

namespace DevSystems
{
    public class PickUpEvent { public PickUpType PickUpType; }
    
    public class CollectorContrloller : MonoBehaviour
    {
        private PlayerHandEquipmentStorage _playerHandEquipmentStorage;

        public void Initialize(PlayerHandEquipmentStorage playerHandEquipmentStorage)
        {
            _playerHandEquipmentStorage = playerHandEquipmentStorage;
        }

        private void Awake()
        {
            EventAggregator.Subscribe<PickUpEvent>(OnPickUpEventHandler);
        }

        private void OnDestroy()
        {
            EventAggregator.Unsubscribe<PickUpEvent>(OnPickUpEventHandler);
        }
        
        private void PickUPHandler(ISelectable pickUpView)
        {
            ResourceView? gameResource = pickUpView is ResourceView resource ? resource : null;
            HandEquipmentView? handEquipment = pickUpView is HandEquipmentView handEquip ? handEquip : null;
                
            switch (pickUpView.SelectableType)
            {
                case PickUpType.Axe:
                    _playerHandEquipmentStorage.AddTools(handEquipment);
                    break;
                case PickUpType.Pickaxe:
                    _playerHandEquipmentStorage.AddTools(handEquipment);
                    break;
                case PickUpType.Sickle:
                    _playerHandEquipmentStorage.AddTools(handEquipment);
                    break;
                case PickUpType.Sword:
                    _playerHandEquipmentStorage.AddTools(handEquipment);
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
        
        private void OnPickUpEventHandler(object sender, PickUpEvent eventData)
        {
            if (sender is ISelectable selectable)
            {
                PickUPHandler(selectable);
            }
        }
    }

    
}