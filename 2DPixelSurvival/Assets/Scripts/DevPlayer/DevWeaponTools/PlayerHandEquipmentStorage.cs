using System.Collections.Generic;
using System.Linq;
using DevSystems;

namespace DevPlayer
{
   public class PlayerHandEquipmentStorage
    {
        private List<HandEquipment> _weapons = new();
        private List<HandEquipment> _tools = new();
    
        public PlayerHandEquipmentStorage(List<HandEquipment> weapons)
        {
            _weapons = weapons;
        }

        public void AddWeapons(HandEquipment weapon) => _weapons.Add(weapon);
        public void AddTools(HandEquipment tool) => _tools.Add(tool);
    
        public bool HasTypeOfTools(ToolsType wType) => _tools.Any(w => w.ToolsType == wType);
        public bool HasTypeOfWeapon(PlayerWeaponType wType) => _weapons.Any(w => w.WeaponType == wType);
        
        
    }
}