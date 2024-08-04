using System.Collections.Generic;
using DevSystems.MiningSystem;
using UnityEngine;

namespace DevPlayer
{

    public class CheckingMiningRules : MonoBehaviour
    {
        [SerializeField] private List<ResourceBehavior> _resourceBehaviors;
        
        private PlayerHandEquipmentStorage _playerHandEquipmentStorage;
        private IPlayerAction _playerActions;
        
        private bool _isMiningResource;

        public void Initialize(PlayerHandEquipmentStorage playerHandEquipmentStorage, IPlayerAction playerAction)
        {
            _playerHandEquipmentStorage = playerHandEquipmentStorage;
            _playerActions = playerAction;
        }

        private void Awake()
        {
            foreach (var resource in _resourceBehaviors)
                resource.PlayerInAreaGetOfResource += OnGetOfResourceHandle;
        }

        private void OnDestroy()
        {
            foreach (var resource in _resourceBehaviors)
                resource.PlayerInAreaGetOfResource -= OnGetOfResourceHandle;
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