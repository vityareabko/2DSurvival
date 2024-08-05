using System.Collections.Generic;
using DevSystems.MiningSystem;
using UnityEngine;

namespace DevPlayer
{

    public class MiningRulesController : MonoBehaviour
    {
        [SerializeField] private Transform _resourceBehavioursParent;
        
        private List<ResourceBehavior> _resources;
        
        private PlayerHandEquipmentStorage _playerHandEquipmentStorage;
        private IPlayerAction _playerActions;
        private IPlayerReader _playerReader;
        
        private bool _isMiningResource;

        public void Initialize(PlayerHandEquipmentStorage playerHandEquipmentStorage, IPlayerAction playerAction, IPlayerReader playerReader)
        {
            _playerHandEquipmentStorage = playerHandEquipmentStorage;
            _playerActions = playerAction;
            _playerReader = playerReader;
        }

        private void Awake()
        {
            _resources = new List<ResourceBehavior>(_resourceBehavioursParent.GetComponentsInChildren<ResourceBehavior>());

            if (_resources is not null)
            {
                foreach (var resource in _resources)
                {
                    resource.Initialize(_playerReader);
                    resource.PlayerInAreaGetOfResource += OnGetOfResourceHandle;
                }
            }
        }

        private void OnDestroy()
        {
            if (_resources is not null)
            {
                foreach (var resource in _resources)
                    resource.PlayerInAreaGetOfResource -= OnGetOfResourceHandle;
            }
        }

        
        private void Update()
        {
            if (_isMiningResource)
                _playerActions.GetMiningResource(true);
        }

        private void OnGetOfResourceHandle(bool isInTrigger, IMining miningResource)
        {
            if (isInTrigger && _playerHandEquipmentStorage.HasTypeOfTools(miningResource.ToolsTypeForMining))
            {
                _playerActions.SelectTool(miningResource.ToolsTypeForMining);
                _playerActions.GetMiningResource(true);
                _isMiningResource = true;
            }
            else if (isInTrigger && _playerHandEquipmentStorage.HasTypeOfTools(miningResource.ToolsTypeForMining) == false)
            {
                miningResource.ToolMissingNotifier(true);
                _isMiningResource = false;

            }
            else if (isInTrigger == false)
            {
                miningResource.ToolMissingNotifier(false);
                _playerActions.GetMiningResource(false);
                _isMiningResource = false;
            }
        }
    }
}