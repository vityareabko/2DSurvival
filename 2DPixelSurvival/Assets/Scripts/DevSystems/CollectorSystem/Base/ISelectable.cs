using System;

namespace DevSystems
{
    public enum PickUpType
    {
        Axe,
        Pickaxe,
        Sickle,
        Sword,
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