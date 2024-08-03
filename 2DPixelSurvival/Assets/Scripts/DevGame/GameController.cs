using System;
using System.Collections.Generic;
using DevSystems;
using DevPlayer;
using UnityEngine;

namespace DevGame
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private Collector _collector;

        private PlayerHandEquipmentStorage _playerHandEquipmentStorage;
        
        private void Awake()
        {
            _playerHandEquipmentStorage = new PlayerHandEquipmentStorage(new List<HandEquipment>());
            
            _player.Initialize(_playerHandEquipmentStorage);

            _collector.PickUp += OnPickUpSelectableHandler;
        }

        private void OnPickUpSelectableHandler(ISelectable obj)
        {
            switch (obj.SelectableType)
            {
                case PickUpType.Axe:
                    // действия когда игрок подобрал Axe
                    Debug.Log("Подобрал епта !");
                    if (obj is HandEquipment tool)
                        _playerHandEquipmentStorage.AddTools(tool);
                    
                    break;
                case PickUpType.Pickaxe:
                    // действия когда игрок подобрал Pickaxe
                    break;
                case PickUpType.Sickle:
                    // действия когда игрок подобрал Sickle
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}