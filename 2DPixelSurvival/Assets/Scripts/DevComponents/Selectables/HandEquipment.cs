using UnityEngine;

namespace DevSystems
{
    public class HandEquipment : PickUpBase, IHandEquipment
    {
        [SerializeField] private HandEquipmentConfig _config;

        public PlayerWeaponType WeaponType => _config.PlayerWeaponType;
        public ToolsType ToolsType => _config.ToolsType;
        public int Damage => _config.Damage;
        
    }
}

// public class HandEquipment : MonoBehaviour, ISelectable, IHandEquipment
// {
//     public event Action<ISelectable> PickUp;
//         
//     [SerializeField] private HandEquipmentConfig _config;
//
//     public PickUpType SelectableType => _config.SelectableType;
//     public PlayerWeaponType WeaponType => _config.PlayerWeaponType;
//     public ToolsType ToolsType => _config.ToolsType;
//     public int Damage => _config.Damage;
//         
//     private void OnTriggerEnter2D(Collider2D colider)
//     {
//         if (colider.gameObject.layer == LayerMask.NameToLayer(Layers.Player))
//         {
//             PickUp?.Invoke(this);  
//                 
//             // действия после подора 
//                 
//             gameObject.SetActive(false);
//         }
//     }
//
// }