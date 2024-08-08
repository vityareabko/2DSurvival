using System;
using UnityEngine;

namespace DevSystems.MiningSystem
{
    public enum ResourceType
    {
        None   = 0,
        Wood   = 1,
        Stone  = 2,
        Leaf   = 3,
    }

    public interface IMining
    {
        ToolsType ToolsTypeForMining { get; }
        ResourceType ResourceType { get; }
        void ToolMissingNotifier(bool show);
        event Action<bool, IMining> PlayerInAreaGetOfResource;
    }
}