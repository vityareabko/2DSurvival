using System;
using DevSystems;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace DevPlayer
{
    public class PlayerWorldUI : BaseWorldUI
    {
        [SerializeField] private TMP_Text _counterTextCollectResources;
        private NotifyPickUpText _notifyPickUpText;

        protected override void Awake()
        {
            base.Awake();
            _notifyPickUpText = new NotifyPickUpText(_counterTextCollectResources);
            EventAggregator.Subscribe<PickUpEvent>(OnPickUPEvent);
        }

        private void OnDestroy()
        {
            EventAggregator.Unsubscribe<PickUpEvent>(OnPickUPEvent);
        }

        private void OnPickUPEvent(object sender, PickUpEvent eventData)
        {
            switch (eventData.PickUpType)
            {
                case PickUpType.Axe:
                    _notifyPickUpText.NotifyPickUptText("+Rusted  Axe");
                    break;
                case PickUpType.Pickaxe:
                    _notifyPickUpText.NotifyPickUptText("+Rusted  Pickaxe");
                    break;
                case PickUpType.Sickle:
                    _notifyPickUpText.NotifyPickUptText("+Rusted  Sickle");
                    break;
                case PickUpType.Sword:
                    _notifyPickUpText.NotifyPickUptText("+Sword");
                    break;
                case PickUpType.Wood:
                    _notifyPickUpText.UpdateCounterText();
                    break;
                case PickUpType.Stone:
                    _notifyPickUpText.UpdateCounterText();
                    break;
                case PickUpType.Leaf:
                    _notifyPickUpText.UpdateCounterText();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}