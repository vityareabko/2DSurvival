using System;
using Sirenix.OdinInspector;

namespace DevSystems
{
    public enum PickUpType
    {
        // tools
        Axe       = 0,
        Pickaxe   = 1,
        Sickle    = 2,
        
        // weapons
        Sword     = 20,
        
        // resources 
        Wood     = 50,
        Stone    = 51,
        Leaf     = 52,
    }
    
    public enum ToolsType
    {
        None    = 0,
        Axe     = 1,
        Pickaxe = 2,
        Sickle  = 3,
    }

    public enum PlayerWeaponType
    {
        None  = 0,
        Sword = 1,
    }

    public interface IHandEquipment
    {
        PlayerWeaponType WeaponType {get;}
        ToolsType ToolsType { get; }
        int Damage { get; }
    }

    public interface ISelectable
    {
        PickUpType SelectableType { get; }
        event Action<ISelectable> PickUp; 
    }
}