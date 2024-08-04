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

        [SerializeField] private CheckingMiningRules _checkingMiningRules;
        private PlayerHandEquipmentStorage _playerHandEquipmentStorage;
        
        private void Awake()
        {
            _playerHandEquipmentStorage = new PlayerHandEquipmentStorage(new List<HandEquipment>());
            
            _player.Initialize(_playerHandEquipmentStorage);
            _checkingMiningRules.Initialize(_playerHandEquipmentStorage, _player);

            _collector.PickUp += OnPickUpSelectableHandler;
        }

        private void OnPickUpSelectableHandler(ISelectable obj)
        {
            switch (obj.SelectableType)
            {
                case PickUpType.Axe:
                    Debug.Log("Подобрал топор епт!");
                    if (obj is HandEquipment toolAxe)
                        _playerHandEquipmentStorage.AddTools(toolAxe);
                    break;
                case PickUpType.Pickaxe:
                    Debug.Log("Подобрал топор епт!");
                    if (obj is HandEquipment toolPickAxe)
                        _playerHandEquipmentStorage.AddTools(toolPickAxe);
                    break;
                case PickUpType.Sickle:
                    Debug.Log("Подобрал топор епт!");
                    if (obj is HandEquipment toolSickle)
                        _playerHandEquipmentStorage.AddTools(toolSickle);
                    break;
                default:
                    Debug.LogError("Такого мы не Хандлим !!!");    
                    break;  
            }
        }
    }
}