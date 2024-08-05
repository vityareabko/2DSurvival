using System;
using DevSystems;
using DevSystems.MiningSystem;
using DevSystems.VitalitySystem;
using DG.Tweening;
using Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace DevPlayer
{
    public class ResourceBehavior : MonoBehaviour, IMining
    {
        public event Action<bool, IMining> PlayerInAreaGetOfResource;

        // TODO - SO Конфиг
        [SerializeField] private MiningTrigger _miningTrigger;
        [SerializeField] private ToolsType _toolsTypeForMining;
        [SerializeField] private ResourceType _resourceType;
        [SerializeField] private bool _shakeWhenIsNearby = true;
        
        [SerializeField] private Canvas _canvas;
        [SerializeField] private RectTransform _infoAboutNeededTool;
        [SerializeField] private RectTransform _healthProgressBar;
        [SerializeField] private Image _healthFillAmount;

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
            if (_canvas.worldCamera == null)
                _canvas.worldCamera = Camera.main;
            
            if (_vitalitySystem != null)
                EventAggregator.Subscribe<UpdatedHealthVitalityEvent>(OnVitalityHealthChangeHandler);

            ToolMissingNotifier(false);
            _healthProgressBar.localScale = Vector3.zero;
            _healthFillAmount.fillAmount = 1f;
            _healthProgressBar.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            if (_vitalitySystem != null)
                EventAggregator.Unsubscribe<UpdatedHealthVitalityEvent>(OnVitalityHealthChangeHandler);
        }

        public void ToolMissingNotifier(bool show)
        {
            if (show)
            {
                _infoAboutNeededTool.DOScale(1f, 0.1f).SetEase(Ease.InOutSine);
            }
            else
            {
                DOVirtual.DelayedCall(1.5f, () =>
                {
                    _infoAboutNeededTool.DOScale(0f, 0.1f)
                        .SetEase(Ease.InOutSine);
                });
            }
        }

        private void OnEnterInZoneOfGetResource(bool inZone)
        {
            PlayerInAreaGetOfResource?.Invoke(inZone, this);
        }

        private Tween _healthBarDelayHide;
        private void OnVitalityHealthChangeHandler(object sender, UpdatedHealthVitalityEvent eventData)
        {
            Debug.Log($"eventData.CurrentValue / eventData.MaxValue; {sender == _vitalitySystem}");
            if (sender == _vitalitySystem)
            {
                if (_healthBarDelayHide != null)
                    _healthBarDelayHide.Kill();
                
                _healthProgressBar.gameObject.SetActive(true);
                _healthProgressBar.DOScale(1, 0.1f)
                    .SetEase(Ease.InOutSine);

                _healthFillAmount.fillAmount = (float)eventData.CurrentValue / eventData.MaxValue;

                _healthBarDelayHide = DOVirtual.DelayedCall(3f, () =>
                {
                    _healthProgressBar.DOScale(0, 0.1f)
                        .SetEase(Ease.InOutSine)
                        .OnComplete(() => _healthProgressBar.gameObject.SetActive(false));
                });
            }
        }
        
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer(Layers.Tool))
            {
                if (_playerReader.GetCurrentToolType() == _toolsTypeForMining)
                {
                    Debug.Log("Hit");
                    
                    // сдесь выполняем когда игрок с tools в руках 
                    _vitalitySystem.DecreaseVitalityValue(VitalityModuleName.HealthModule, _playerReader.GetHitToolDamageByType(_toolsTypeForMining));
                    
                    transform.DOShakeScale(0.1f, 0.1f);
                }
                else if (_shakeWhenIsNearby)
                    transform.DOShakeScale(0.1f, 0.1f);
            }
        }
    }
}