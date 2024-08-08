using System;
using System.Collections;
using System.Collections.Generic;
using DevPlayer;
using DevSystems;
using DevSystems.MiningSystem;
using DevSystems.VitalitySystem;
using DG.Tweening;
using Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[SelectionBase]
public class MiningResourcesBehavior : MonoBehaviour, IMining
{
    public event Action<bool, IMining> PlayerInAreaGetOfResource;

    [SerializeField] private MiningTrigger _miningTrigger;
    [SerializeField] private MiningResourcesWorldUI _miningResourcesWorldUI;
    [SerializeField] private VitalityModuleSystem _vitalitySystem;
    
    [TitleGroup("Recover")][SerializeField] private bool _isTriggerRecoverProcee = false;
    [TitleGroup("Recover")][SerializeField] private SpriteRenderer _spriteRenderer;
    [TitleGroup("Recover")][SerializeField] private List<Sprite> _miningResourceRecoverySprites;
    [TitleGroup("Recover")][SerializeField] private float _fullRecoverResourceTime;
    
    // TODO - SO Конфиг
    [SerializeField] private ToolsType _toolsTypeForMining;
    [SerializeField] private ResourceType _resourceType;
    [SerializeField] private bool _shakeWhenIsNearby = true;
    
    private RecoverMiningResourceSystem _recoverMiningResourceSystem;
    private Collider2D _collider;
    
    private IPlayerReader _playerReader;
    
    private bool _isInProcessRecover;
    private Tween _tweenShakeTouchSimulate;
    private Tween _tweenHitOnlyOnForOneSlash;

    public ToolsType ToolsTypeForMining => _toolsTypeForMining;
    public ResourceType ResourceType => _resourceType;

    public void Initialize(IPlayerReader playerReader)
    {
        _playerReader = playerReader;
    }

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        
        _recoverMiningResourceSystem = new RecoverMiningResourceSystem(_spriteRenderer, _spriteRenderer.GetComponent<Transform>(),
            _miningResourceRecoverySprites, _fullRecoverResourceTime);
            
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
    
    private void RecoverProcess()
    {
        
        PlayerInAreaGetOfResource?.Invoke(false, this);
        _isInProcessRecover = true;
        
        _miningResourcesWorldUI.RecoverProcess(_fullRecoverResourceTime);
        _collider.isTrigger = _isTriggerRecoverProcee;
        _recoverMiningResourceSystem.StartRecoverMiningResource(() =>
        {
            var maxHealth = _vitalitySystem.GetMaxVitalityValueByModuleName(VitalityModuleName.HealthModule);
            _vitalitySystem.RecoveryValueByModuleName(VitalityModuleName.HealthModule, maxHealth, false);
            _isInProcessRecover = false;
            _collider.isTrigger = false;
        });
    }

    private void OnEnterInZoneOfGetResource(bool inZone)
    {
        if (_isInProcessRecover)
        {
            PlayerInAreaGetOfResource?.Invoke(false, this);
            return;
        }

        PlayerInAreaGetOfResource?.Invoke(inZone, this);
    }
    
    private void OnMiningResourceRunOutHandler(object sender, HealthEmptyEvent eventData)
    {
        if (sender == _vitalitySystem)
        {
            RecoverProcess();
        }
    }

    private void OnVitalityHealthChangeHandler(object sender, UpdatedHealthVitalityEvent eventData)
    {
        if (sender == _vitalitySystem)
        {
            _miningResourcesWorldUI.HealthChangeHandler(eventData.CurrentValue, eventData.MaxValue, eventData.Damage);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer(Layers.Tool))
        {
            if (_isInProcessRecover)
                return;
            
            if (_playerReader.GetCurrentToolType() == _toolsTypeForMining)
            {
                if (_tweenHitOnlyOnForOneSlash is null)
                {
                    _tweenHitOnlyOnForOneSlash = DOVirtual.DelayedCall(_playerReader.AttackSlashSpeed, () =>
                        {
                            Debug.Log("Hit");

                            int damage = _playerReader.GetHitToolDamageByType(_toolsTypeForMining);
                            _vitalitySystem.DecreaseVitalityValue(VitalityModuleName.HealthModule, damage);
                        })
                        .OnComplete(() => _tweenHitOnlyOnForOneSlash = null);

                    if (_tweenShakeTouchSimulate is null)
                        _tweenShakeTouchSimulate = transform.DOShakeScale(0.1f, 0.1f).SetEase(Ease.InOutSine).OnComplete(() => _tweenShakeTouchSimulate = null);
                        
                }
            }

            if (_shakeWhenIsNearby && _tweenShakeTouchSimulate is null)
                _tweenShakeTouchSimulate = transform.DOShakeScale(0.1f, 0.1f).SetEase(Ease.InOutSine).OnComplete(() => _tweenShakeTouchSimulate = null);
        }
    }
}

public class RecoverMiningResourceSystem
{
    private Transform _recoverTarget;
    private float _timeFullRecover;
    private int _intervalRecovers;
    private SpriteRenderer _spriteRenderer;
    private Sprite _spriteDefault;
    private Sprite _spriteZeroRecover;
    private Vector3 _initialScale;
    

    public RecoverMiningResourceSystem(SpriteRenderer spriteRenderer, Transform recoverTarget, List<Sprite> _sprites, float timeFullRecover)
    {
        _initialScale = recoverTarget.localScale;
        _intervalRecovers = Random.Range(3, 5);
        _spriteDefault = _sprites[0];
        _spriteZeroRecover = _sprites.Count > 1 ? _sprites[1] : null;
        _timeFullRecover = timeFullRecover;
        _spriteRenderer = spriteRenderer;
        _recoverTarget = recoverTarget;
    }

    public void StartRecoverMiningResource(Action callback)
    {
        float intervalTime = _timeFullRecover / _intervalRecovers;
        if (_spriteZeroRecover != null)
            _spriteRenderer.sprite = _spriteZeroRecover;
        else
        {
            _spriteRenderer.sprite = _spriteDefault;
            _recoverTarget.localScale = _initialScale * (1f / _intervalRecovers);
        }

        Sequence sequence = DOTween.Sequence();

        for (int i = 1; i <= _intervalRecovers; i++)
        {
            int currentInterval = i;
            sequence.Append(DOVirtual.DelayedCall(intervalTime, () =>
            {
                HandleInterval(currentInterval);
            }));
        }

        sequence.OnComplete(() => callback.Invoke());
        sequence.Play();
    }

    private void HandleInterval(int intervalIndex)
    {
        if (intervalIndex == _intervalRecovers)
        {
            _recoverTarget.DOScale(_initialScale, 0.2f);
        }
        else
        {
            _spriteRenderer.sprite = _spriteDefault;
            float scaleMultiplier = (float)intervalIndex / _intervalRecovers;
            _recoverTarget.DOScale(_initialScale * scaleMultiplier, 0.2f);
        }
    }
}
