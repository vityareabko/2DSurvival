using System;
using System.Collections.Generic;
using System.Linq;
using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using Assets.HeroEditor4D.Common.Scripts.Data;
using Assets.HeroEditor4D.Common.Scripts.Enums;
using DevExtensions;
using DevPlayer;
using DevSystems;
using DevSystems.VitalitySystem;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace DevPlayer
{
    public interface IPlayerAction
    {
        void SelectTool(ToolsType type);
        void GetMiningResource(bool isStartMining);
    }

    public interface IPlayerReader
    {
        ToolsType GetCurrentToolType();
        PlayerWeaponType GetCurrentWeaponType();
        int GetHitToolDamageByType(ToolsType type);
        int GetHitWeaponDamageByType(PlayerWeaponType type);
        float AttackSlashSpeed { get; }
    }

    public class Player : MonoBehaviour, IPlayerAction, IPlayerReader
    {
        [FormerlySerializedAs("_resourceView")] [SerializeField] private List<ResourceView> _resourceCollection;
        
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private AnimationManager _animation;
        [SerializeField] private Character4D _character4D;
        [SerializeField] private VitalityModuleSystem _vitalitySystem;

        [SerializeField] private float _moveSpeed = 2f;


        public float AttackSlashSpeed => _animation.Animator.GetClipLength("Slash2H");

        private PlayerMove _playerMove;
        private PlayerHandEquipmentStorage _playerHandEquipmentStorage;

        private ToolsType _currentToolsType;
        private PlayerWeaponType _currentWeaponType;

        private bool _isEquip;

        private ItemSprite _sickle =>
            _character4D.SpriteCollection.MeleeWeapon1H.FirstOrDefault(i => i.Id == "CustomToolSickle");

        private ItemSprite _axe =>
            _character4D.SpriteCollection.MeleeWeapon1H.FirstOrDefault(i => i.Id == "CustomToolAxe");

        private ItemSprite _pickaxe =>
            _character4D.SpriteCollection.MeleeWeapon2H.FirstOrDefault(i => i.Id == "CustomToolPickAxe");

        public void Initialize(PlayerHandEquipmentStorage playerHandEquipmentStorage)
        {
            _playerHandEquipmentStorage = playerHandEquipmentStorage;
        }

        private void Awake()
        {
            UnEquip();

            _currentToolsType = ToolsType.None;
            _currentWeaponType = PlayerWeaponType.None;
            _playerMove = new PlayerMove(_animation, _rb, _character4D, _moveSpeed);
        }


        private void Update()
        {
            // if (Input.GetMouseButtonDown(0))
            // {
            //     _vitalitySystem.DecreaseVitalityValue(VitalityModuleName.HealthModule, 10);
            //     Debug.Log($"Player geting 10 damage, current HP - {_vitalitySystem.GetCurrentVitalityValueByModuleName(VitalityModuleName.HealthModule)}");
            // }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Instantiate(_resourceCollection[Random.Range(0, _resourceCollection.Count)], transform.position, Quaternion.identity);
            }

            _playerMove.Tick();
        }

        private void FixedUpdate()
        {
            _playerMove.FixedTick();
        }

        public ToolsType GetCurrentToolType() => _currentToolsType;
        public PlayerWeaponType GetCurrentWeaponType() => _currentWeaponType;
        public int GetHitToolDamageByType(ToolsType type) => _playerHandEquipmentStorage.GetToolData(type).Damage;

        public int GetHitWeaponDamageByType(PlayerWeaponType type) =>
            _playerHandEquipmentStorage.GetWeaponData(type).Damage;

        public void GetMiningResource(bool isStartMining)
        {
            if (isStartMining)
            {
                if (_isEquip)
                    _animation.Slash(true);
            }
            else
            {
                _animation.ForceStopAnimation();
                UnEquip();
            }
        }

        public void SelectTool(ToolsType type)
        {
            switch (type)
            {
                case ToolsType.None:
                    UnEquip();
                    _currentToolsType = ToolsType.None;
                    break;

                case ToolsType.Axe:
                    _character4D.Equip(_axe, EquipmentPart.MeleeWeapon1H);
                    _isEquip = true;
                    _currentToolsType = ToolsType.Axe;
                    break;

                case ToolsType.Pickaxe:
                    _character4D.Equip(_pickaxe, EquipmentPart.MeleeWeapon2H);
                    _isEquip = true;
                    _currentToolsType = ToolsType.Pickaxe;
                    break;

                case ToolsType.Sickle:
                    _character4D.Equip(_sickle, EquipmentPart.MeleeWeapon1H);
                    _isEquip = true;
                    _currentToolsType = ToolsType.Sickle;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private void UnEquip()
        {
            _isEquip = false;
            _currentToolsType = ToolsType.None;
            _currentWeaponType = PlayerWeaponType.None;

            _character4D.UnEquip(EquipmentPart.MeleeWeapon1H);
            _character4D.UnEquip(EquipmentPart.MeleeWeapon2H);
            _character4D.UnEquip(EquipmentPart.Bow);
        }
    }

}