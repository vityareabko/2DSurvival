using DevSystems.MiningSystem;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace DevPlayer
{
    public class NotifyPickUpText
    {
        private TMP_Text _contentText;
        private RectTransform _counterTextRT;
        private Tween _tweenCurrentResourceCollectCount;
        private Tween _tweenScaleEffect;
        private Tween _tweenPickUp;
        private int _currentResourceCollectCount;
        private ResourceType _lastPickUpResourceType;
        
        public NotifyPickUpText(TMP_Text contentText)
        {
            _lastPickUpResourceType = ResourceType.None;
            _contentText = contentText;
            ResetCounterResourceText();
            _counterTextRT = _contentText.gameObject.GetComponent<RectTransform>();
        }

        public void UpdateCounterText(string text, ResourceType type)
        {
            _tweenCurrentResourceCollectCount?.Kill();
            
            _contentText.gameObject.SetActive(true);
            if (_lastPickUpResourceType == type)
            {
                _contentText.text = $"{++_currentResourceCollectCount} {text}";
            }
            else
            {
                _currentResourceCollectCount = 0;
                _contentText.text = $"{++_currentResourceCollectCount} {text}";
            }
            
            _lastPickUpResourceType = type;
            
            if (_tweenScaleEffect is null)
                _tweenScaleEffect = _counterTextRT.DOPunchScale(new Vector2(0.4f, 0.4f), 0.1f).OnComplete(() => _tweenScaleEffect = null);
        
            _tweenCurrentResourceCollectCount = DOVirtual.DelayedCall(2f, ResetCounterResourceText);
        }

        public void NotifyPickUptText(string text)
        {
            if (_tweenPickUp is not null)
                _tweenPickUp.Kill();
            
            _contentText.fontSize = 0.56f;
            _contentText.text = text;
            _contentText.gameObject.SetActive(true); 
            _tweenPickUp = DOVirtual.DelayedCall(2f, () =>
            {
                _contentText.gameObject.SetActive(false);
                _contentText.text = "";
                _contentText.fontSize = 0.7f;
                _tweenPickUp = null;
            });
        }

        private void ResetCounterResourceText()
        {
            _contentText.gameObject.SetActive(false);
            _currentResourceCollectCount = 0;
            _contentText.text = string.Empty;
            _tweenCurrentResourceCollectCount = null;
        }
    }
}