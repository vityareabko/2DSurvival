using UnityEngine;

namespace DevSystems
{
    public class HandEquipmentView : PickUpBase, IHandEquipment
    {
        [SerializeField] private HandEquipmentConfig _config;

        public PlayerWeaponType WeaponType => _config.PlayerWeaponType;
        public ToolsType ToolsType => _config.ToolsType;
        public int Damage => _config.Damage;
        
    }
}