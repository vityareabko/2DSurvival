using System.Collections.Generic;
using UnityEngine;

namespace DevSystems
{
    [CreateAssetMenu(fileName = "HandEquipmentDatabase", menuName = "Databases/HandEquipmentDatabase")]
    public class HandEquipmentDatabase : ScriptableObject
    {
        public List<HandEquipmentConfig> HandEquipmentConfigs;
    }
}