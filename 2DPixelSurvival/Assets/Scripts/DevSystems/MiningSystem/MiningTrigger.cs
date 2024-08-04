using System;
using Extensions;
using UnityEngine;

namespace DevPlayer
{
    public class MiningTrigger : MonoBehaviour
    {
        public event Action<bool> PlayerIsInTrigger;
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer(Layers.Player))
            {
                PlayerIsInTrigger?.Invoke(true);
            }
        }
        
        private void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer(Layers.Player))
                PlayerIsInTrigger?.Invoke(false);
        }
    }
}