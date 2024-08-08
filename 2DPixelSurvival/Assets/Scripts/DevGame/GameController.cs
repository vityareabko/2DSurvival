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
        
        [SerializeField] private CollectorContrloller collectorContrloller;
        [SerializeField] private MiningRulesController miningRulesController;
        
        [SerializeField] private Player _player;
        
        private PlayerHandEquipmentStorage _playerHandEquipmentStorage;
        
        [Inject] private void Construct()
        {
            _playerHandEquipmentStorage = new PlayerHandEquipmentStorage(new List<HandEquipment>(), new List<HandEquipment>(), _handEquipmentDatabase);
            
            _player.Initialize(_playerHandEquipmentStorage);
            miningRulesController.Initialize(_playerHandEquipmentStorage, _player, _player);

            collectorContrloller.PickUp += OnPickUpSelectableHandler;
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
                    Debug.Log("Подобрал Кируку епт!");
                    if (obj is HandEquipment toolPickAxe)
                        _playerHandEquipmentStorage.AddTools(toolPickAxe);
                    break;
                case PickUpType.Sickle:
                    Debug.Log("Подобрал Серп епт!");
                    if (obj is HandEquipment toolSickle)
                        _playerHandEquipmentStorage.AddTools(toolSickle);
                    break;
                case PickUpType.Sword:
                    break;
                case PickUpType.Wood:
                    if (obj is ResourceView resource)
                    {
                    // обрабртка подбора рессурса
                        resource.gameObject.SetActive(false);
                        Debug.Log("Ресурс подобран епт !!!");
                    }
                    break;
                case PickUpType.Stone:
                    break;
                case PickUpType.Leaf:
                    break;
                default:
                    Debug.LogError("Такого мы не Хандлим !!!");    
                    break;  
            }
        }
    }
}