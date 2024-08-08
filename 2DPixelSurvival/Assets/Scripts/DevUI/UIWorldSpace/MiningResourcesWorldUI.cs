 using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DevPlayer
{
    public class MiningResourcesWorldUI : BaseWorldUI
    {
        
        [SerializeField] private RectTransform _infoAboutNeededTool;
        [SerializeField] private RectTransform _healthProgressBar;
        [FormerlySerializedAs("_healthFillAmount")] [SerializeField] private Image _healthFill;
        [SerializeField] private Color _defoultFillColor; 
        [SerializeField] private Color _recoverFillColor;
        
        [SerializeField] private TMP_Text _damageText;
        private RectTransform _damageTextRT;
        
        private Tween _healthBarDelayHide;
        private Tween _tweenDamageTextDelayEffect;
        private Tween _tweenDamageTextPunchEffect;

        private int _consumableDamage;

        protected override void Awake()
        {
            base.Awake();
            _damageTextRT = _damageText.GetComponent<RectTransform>();
            
            ToolMissingNotifier(false);
            ResetWorldUI();
        }

        private void OnEnable()
        {
            _consumableDamage = 0;
            _healthProgressBar.localScale = Vector3.zero;
            _healthFill.fillAmount = 1f;
            _healthProgressBar.gameObject.SetActive(false);
            _damageText.gameObject.SetActive(false);
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
        
        public void HealthChangeHandler(float currentHealth, float maxHealth, int damage)
        {
            DamageEffect(damage);
            ProgressBarDecreaseHealthEffect(currentHealth, maxHealth);

            if (currentHealth <= 0)
                ResetWorldUI();
            
        }

        public void RecoverProcess(float recoverDurationTime)
        {
            _healthBarDelayHide?.Kill();
            _tweenDamageTextDelayEffect?.Kill();
            
            _healthFill.color = _recoverFillColor;
            _healthFill.fillAmount = 0f;
            
            DOVirtual.DelayedCall(recoverDurationTime * 0.2f, () =>
            {
                _healthProgressBar.localScale = Vector3.zero;
                _healthProgressBar.gameObject.SetActive(true);
                _healthProgressBar.DOScale(1, 0.1f).SetEase(Ease.InOutSine);
                _healthFill.DOFillAmount(1f, recoverDurationTime - (recoverDurationTime * 0.2f))
                    .SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        _healthFill.color = _defoultFillColor;
                        DOVirtual.DelayedCall(0.2f, () => _healthProgressBar.gameObject.SetActive(false));
                    });
            });
        }

        private void ResetWorldUI()
        {
            _consumableDamage = 0;
            _healthProgressBar.localScale = Vector3.zero;
            _healthFill.fillAmount = 1f;
            _healthProgressBar.gameObject.SetActive(false);
            _damageText.gameObject.SetActive(false);
        }

        private void ProgressBarDecreaseHealthEffect(float currentHealth, float maxHealth)
        {
            if (_healthBarDelayHide != null)
                _healthBarDelayHide.Kill();

            _healthProgressBar.gameObject.SetActive(true);
            _healthProgressBar.DOScale(1, 0.1f)
                .SetEase(Ease.InOutSine);

            _healthFill.fillAmount = currentHealth / maxHealth;

            _healthBarDelayHide = DOVirtual.DelayedCall(3f, () =>
            {
                _healthProgressBar.DOScale(0, 0.1f)
                    .SetEase(Ease.InOutSine)
                    .OnComplete(() => _healthProgressBar.gameObject.SetActive(false));
            });
        }

        private void DamageEffect(int damage)
        {
            if (_tweenDamageTextDelayEffect is not null)
            {
                _tweenDamageTextDelayEffect.Kill();
            }

            _consumableDamage += damage;
            _damageText.text = $"-{_consumableDamage}";

            _damageText.gameObject.SetActive(true);
            _tweenDamageTextDelayEffect = DOVirtual.DelayedCall(2f, () =>
            {
                if (_tweenDamageTextPunchEffect is null)
                {
                    _tweenDamageTextPunchEffect = _damageTextRT.DOPunchScale(new Vector2(0.3f, 0.3f), 0.03f)
                        .SetEase(Ease.InOutSine)
                        .OnComplete(() =>
                        {
                            _damageText.gameObject.SetActive(false);
                            _tweenDamageTextDelayEffect = null;
                            _tweenDamageTextPunchEffect = null;
                            _consumableDamage = 0;
                        });
                }
            });
        }
    }
}