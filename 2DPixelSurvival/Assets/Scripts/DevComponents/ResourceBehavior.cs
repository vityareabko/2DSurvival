using System;
using DevSystems;
using DevSystems.MiningSystem;
using DG.Tweening;
using Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace DevPlayer
{
    public class ResourceBehavior : MonoBehaviour, IMining
    {
        public event Action<bool, IMining> PlayerInAreaGetOfResource;
        
        [SerializeField] private MiningTrigger _miningTrigger;
        [SerializeField] private ToolsType _toolsTypeForMining;
        [SerializeField] private ResourceType _resourceType;
        [SerializeField] private bool _shakeWhenIsNearby = true;


        [SerializeField] private Canvas _canvas;
        [SerializeField] private RectTransform _infoAboutNeededTool;
        
        public ToolsType ToolsTypeForMining => _toolsTypeForMining;
        public ResourceType ResourceType => _resourceType;

        private void Awake()
        {
            _miningTrigger.PlayerIsInTrigger += OnEnterInZoneOfGetResource;
            if (_canvas.worldCamera == null)
                _canvas.worldCamera = Camera.main;

            ToolMissingNotifier(false);
        }
        
        public void ToolMissingNotifier(bool show)
        {
            if (show)
            {
                _infoAboutNeededTool.DOScale(1f, 0.1f).SetEase(Ease.InOutSine);
            }
            else
            {
                _infoAboutNeededTool.DOScale(0f, 0.1f)
                    .SetEase(Ease.InOutSine);
            }
        }

        private void OnEnterInZoneOfGetResource(bool inZone)
        {
            PlayerInAreaGetOfResource?.Invoke(inZone, this);
        }
        
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer(Layers.Tool))
            {
                if (collider.gameObject.GetComponent<SpriteRenderer>().sprite != null)
                {
                    Debug.Log("Hit");
                    // сдесь выполняем когда игрок с tools в руках 
                    
                    transform.DOShakeScale(0.1f, 0.1f);
                }
                else if (_shakeWhenIsNearby)
                    transform.DOShakeScale(0.1f, 0.1f);
            }
        }
    }
}