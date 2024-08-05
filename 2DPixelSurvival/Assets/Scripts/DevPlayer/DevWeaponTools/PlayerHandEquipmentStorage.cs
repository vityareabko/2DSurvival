using System.Collections.Generic;
using System.Linq;
using DevSystems;

namespace DevPlayer
{
   public class PlayerHandEquipmentStorage
    {
        private List<HandEquipment> _weapons = new();
        private List<HandEquipment> _tools = new();

        private HandEquipmentDatabase _handEquipmentDatabase;
    
        public PlayerHandEquipmentStorage(List<HandEquipment> weapons, List<HandEquipment> tools, HandEquipmentDatabase handEquipmentDatabase)
        {
            _weapons = weapons;
            _tools = tools;
            _handEquipmentDatabase = handEquipmentDatabase;
        }

        public void AddWeapons(HandEquipment weapon) => _weapons.Add(weapon);
        public void AddTools(HandEquipment tool) => _tools.Add(tool);
    
        public bool HasTypeOfTools(ToolsType wType) => _tools.Any(w => w.ToolsType == wType);
        public bool HasTypeOfWeapon(PlayerWeaponType wType) => _weapons.Any(w => w.WeaponType == wType);

        public HandEquipmentConfig GetToolData(ToolsType type) =>
            _handEquipmentDatabase.HandEquipmentConfigs.First(e => e.ToolsType == type);
        
        public HandEquipmentConfig GetWeaponData(PlayerWeaponType type) =>
            _handEquipmentDatabase.HandEquipmentConfigs.First(e => e.PlayerWeaponType == type);
        

    }
}