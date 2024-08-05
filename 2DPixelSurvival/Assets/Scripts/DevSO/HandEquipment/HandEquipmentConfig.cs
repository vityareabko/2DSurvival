using UnityEngine;

namespace DevSystems
{
    [CreateAssetMenu(fileName = "Config", menuName = "Configs/HandEquipmentConfig")]
    public class HandEquipmentConfig : ScriptableObject
    {
        [field: SerializeField] public PickUpType SelectableType {get; private set;}
        [field: SerializeField] public PlayerWeaponType PlayerWeaponType {get; private set;}
        [field: SerializeField] public ToolsType ToolsType {get; private set;}
        [field: SerializeField] public int Damage {get; private set;}
    }
}