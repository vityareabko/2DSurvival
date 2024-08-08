using System.Collections.Generic;
using DevSystems;
using DevPlayer;
using UnityEngine;
using Zenject;

namespace DevGame
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private HandEquipmentDatabase _handEquipmentDatabase;
        
        [SerializeField] private CollectorContrloller _collectorContrloller;
        [SerializeField] private MiningRulesController _miningRulesController;
        
        [SerializeField] private Player _player;
        
        private PlayerHandEquipmentStorage _playerHandEquipmentStorage;
        
        [Inject] private void Construct()
        {
            _playerHandEquipmentStorage = new PlayerHandEquipmentStorage(new List<HandEquipmentView>(), new List<HandEquipmentView>(), _handEquipmentDatabase);
            
            _player.Initialize(_playerHandEquipmentStorage);
            _miningRulesController.Initialize(_playerHandEquipmentStorage, _player, _player);
            _collectorContrloller.Initialize(_playerHandEquipmentStorage);
        }
    
    }
}