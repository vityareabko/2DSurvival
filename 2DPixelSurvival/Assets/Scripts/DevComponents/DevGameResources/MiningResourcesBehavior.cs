using System;
using System.Collections.Generic;
using DevPlayer;
using DevSystems;
using DevSystems.MiningSystem;
using DevSystems.VitalitySystem;
using DG.Tweening;
using Extensions;
using UnityEngine;
using UnityEngine.UI;

public class MiningResourcesBehavior : MonoBehaviour, IMining
{
    public event Action<bool, IMining> PlayerInAreaGetOfResource;

    [SerializeField] private MiningTrigger _miningTrigger;
    
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private List<Sprite> _miningResourceRecoverySprites;
    [SerializeField] private float recoverResourceForMiningTime;
    
    // TODO - SO Конфиг
    [SerializeField] private ToolsType _toolsTypeForMining;
    [SerializeField] private ResourceType _resourceType;
    [SerializeField] private bool _shakeWhenIsNearby = true;
    
    [SerializeField] private MiningResourcesWorldUI _miningResourcesWorldUI;

    [SerializeField] private VitalityModuleSystem _vitalitySystem;
    
    private IPlayerReader _playerReader;

    public ToolsType ToolsTypeForMining => _toolsTypeForMining;
    public ResourceType ResourceType => _resourceType;

    public void Initialize(IPlayerReader playerReader)
    {
        _playerReader = playerReader;
    }

    private void Awake()
    {
        _miningTrigger.PlayerIsInTrigger += OnEnterInZoneOfGetResource;

        if (_vitalitySystem != null)
        {
            EventAggregator.Subscribe<HealthEmptyEvent>(OnMiningResourceRunOutHandler);
            EventAggregator.Subscribe<UpdatedHealthVitalityEvent>(OnVitalityHealthChangeHandler);
            
        }
    }

    private void OnDestroy()
    {
        if (_vitalitySystem != null)
        {
            EventAggregator.Subscribe<HealthEmptyEvent>(OnMiningResourceRunOutHandler);
            EventAggregator.Unsubscribe<UpdatedHealthVitalityEvent>(OnVitalityHealthChangeHandler);
        }
    }

    public void ToolMissingNotifier(bool show)
    {
        _miningResourcesWorldUI.ToolMissingNotifier(show);
    }

    private void OnEnterInZoneOfGetResource(bool inZone)
    {
        PlayerInAreaGetOfResource?.Invoke(inZone, this);
    }
    
    private void OnMiningResourceRunOutHandler(object sender, HealthEmptyEvent eventData)
    {
        DOVirtual.DelayedCall(recoverResourceForMiningTime, () => gameObject.SetActive(true));
    }

    private void OnVitalityHealthChangeHandler(object sender, UpdatedHealthVitalityEvent eventData)
    {
        Debug.Log($"eventData.CurrentValue / eventData.MaxValue;");
        if (sender == _vitalitySystem)
        {
            _miningResourcesWorldUI.HealthChangeHandler(eventData.CurrentValue, eventData.MaxValue, eventData.Damage);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer(Layers.Tool))
        {
            if (_playerReader.GetCurrentToolType() == _toolsTypeForMining)
            {
                Debug.Log("Hit");
                
                int damage = _playerReader.GetHitToolDamageByType(_toolsTypeForMining);
                _vitalitySystem.DecreaseVitalityValue(VitalityModuleName.HealthModule, damage);
                
                transform.DOShakeScale(0.1f, 0.1f);
            }
            else if (_shakeWhenIsNearby)
                transform.DOShakeScale(0.1f, 0.1f);
        }
    }
}
