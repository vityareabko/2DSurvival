using System;
using System.Linq;
using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using Assets.HeroEditor4D.Common.Scripts.Enums;
using DevSystems;
using UnityEngine;

namespace DevPlayer
{
    public interface IPlayerAction
    {
        void SelectTool(ToolsType type);
        void GetMiningResource(bool isStart);
    }

    public class Player : MonoBehaviour, IPlayerAction
    {
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private AnimationManager _animation;
        [SerializeField] private Character4D _character4D;
        
        private PlayerMove _playerMove;
        private PlayerHandEquipmentStorage _playerHandEquipmentStorage;

        private bool _isEquip;
        
        public void Initialize(PlayerHandEquipmentStorage playerHandEquipmentStorage)
        {
            _playerHandEquipmentStorage = playerHandEquipmentStorage;
        }

        private void Awake()
        {
            UnEquip();
           _playerMove = new PlayerMove(_animation, _rb, _character4D);
        }

        private void Update()
        {
            TempMethod();
            

            _playerMove.Tick();
        }

        private void FixedUpdate()
        {
            _playerMove.FixedTick();
        }
        
        
        private void TempMethod()
        {
            // if (Input.GetMouseButton(0))
            // {
            //     _animation.Slash(true);
            // }
            //
            // if (Input.GetKeyDown(KeyCode.Alpha1))
            // {
            //     if (_playerHandEquipmentStorage.HasTypeOfTools(ToolsType.Axe))
            //     {
            //         var axe = _character4D.SpriteCollection.MeleeWeapon1H[119];
            //         _character4D.Equip(axe, EquipmentPart.MeleeWeapon1H);
            //     }
            //     else
            //     {
            //         Debug.Log($"нет такого - {ToolsType.Axe} !!");
            //     }
            //     
            // }
            //
            // if (Input.GetKeyDown(KeyCode.Alpha2))
            // {
            //     var pickaxe = _character4D.SpriteCollection.MeleeWeapon2H.First(t =>
            //             t.Id == "FantasyHeroes.Basic.MeleeWeapon2H.IronPickaxe");
            //     _character4D.Equip(pickaxe, EquipmentPart.MeleeWeapon2H);
            // }
            //
            // if (Input.GetKeyDown(KeyCode.Alpha3))
            // {
            //     var axe = _character4D.SpriteCollection.MeleeWeapon1H[119];
            //     _character4D.Equip(axe, EquipmentPart.MeleeWeapon1H);
            // }
            //
            // if (Input.GetKeyDown(KeyCode.Alpha0))
            // {
            //     UnEquip();
            // }
        }

        private void UnEquip()
        {
            _isEquip = false;
            _character4D.UnEquip(EquipmentPart.MeleeWeapon1H);
            _character4D.UnEquip(EquipmentPart.MeleeWeapon2H);
            _character4D.UnEquip(EquipmentPart.Bow);
        }

        public void SelectTool(ToolsType type)
        {
            switch (type)
            {
                case ToolsType.None:
                    UnEquip();
                    break;
                case ToolsType.Axe:
                    var axe = _character4D.SpriteCollection.MeleeWeapon1H[119];
                    _character4D.Equip(axe, EquipmentPart.MeleeWeapon1H);
                    _isEquip = true;
                    break;
                case ToolsType.Pickaxe:
                    var pickaxe = _character4D.SpriteCollection.MeleeWeapon2H.First(t =>
                        t.Id == "FantasyHeroes.Basic.MeleeWeapon2H.IronPickaxe");
                    _character4D.Equip(pickaxe, EquipmentPart.MeleeWeapon2H);
                    _isEquip = true;
                    break;
                case ToolsType.Sickle:
                    _isEquip = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
        
        public void GetMiningResource(bool isStart)
        {
            if (isStart)
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
    }
}