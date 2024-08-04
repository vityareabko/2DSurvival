using System;
using UnityEngine;

namespace DevSystems.MiningSystem
{
    public enum ResourceType
    {
        Wood,
        Stone,
    }

    public interface IMining
    {
        ToolsType ToolsTypeForMining { get; }
        ResourceType ResourceType { get; }
        void ToolMissingNotifier(bool show);
        event Action<bool, IMining> PlayerInAreaGetOfResource;
    }
}