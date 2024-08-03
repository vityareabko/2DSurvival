using System;
using Extensions;
using DevPlayer;
using UnityEngine;

namespace DevSystems
{
    public class HandEquipment : MonoBehaviour, ISelectable, IHandEquipment
    {
        public event Action<ISelectable> PickUp;

        
        // TODO: - пока я сериализую эти данные - но их нужно сделать SerializeObjects конфиг
        [SerializeField] private PickUpType _selectableType;
        [SerializeField] private PlayerWeaponType _playerWeaponType;
        [SerializeField] private ToolsType _toolsType;
        [SerializeField] private int _damage;

        public PickUpType SelectableType => _selectableType;
        public PlayerWeaponType WeaponType => _playerWeaponType;
        public ToolsType ToolsType => _toolsType;
        public int Damage => _damage;

        private void OnTriggerEnter2D(Collider2D colider)
        {
            if (colider.gameObject.layer == LayerMask.NameToLayer(Layers.Player))
            {
                PickUp?.Invoke(this);  
                
                // действия после подора 
                
                gameObject.SetActive(false);
            }
        }

    }
}