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
        private int _currentResourceCollectCount;
        private bool _isNotifyPickUp;

        public NotifyPickUpText(TMP_Text contentText)
        {
            _contentText = contentText;
            ResetCounterResourceText();
            _counterTextRT = _contentText.gameObject.GetComponent<RectTransform>();
        }

        public void UpdateCounterText()
        {
            if (_isNotifyPickUp)
                return;
            
            _tweenCurrentResourceCollectCount?.Kill();

            _contentText.gameObject.SetActive(true);
            _contentText.text = $"+{++_currentResourceCollectCount}";
            
            if (_tweenScaleEffect is null)
                _tweenScaleEffect = _counterTextRT.DOPunchScale(new Vector2(0.4f, 0.4f), 0.03f).OnComplete(() => _tweenScaleEffect = null);
        
            _tweenCurrentResourceCollectCount = DOVirtual.DelayedCall(2f, ResetCounterResourceText);
        }

        public void NotifyPickUptText(string text)
        {
            _contentText.fontSize = 0.24f;
            
            _contentText.text = text;
            _isNotifyPickUp = true;
            _contentText.gameObject.SetActive(true); 
            DOVirtual.DelayedCall(2f, () =>
            {
                _contentText.gameObject.SetActive(false);
                _contentText.text = "";
                _isNotifyPickUp = false;
                _contentText.fontSize = 0.32f;
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