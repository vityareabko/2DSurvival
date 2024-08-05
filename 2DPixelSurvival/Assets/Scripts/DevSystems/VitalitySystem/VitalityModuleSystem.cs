using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DevSystems.VitalitySystem
{
    public static class VitalityModuleName
    {
        public const string HealthModule = "HealthModule";
        public const string StaminaModule = "StaminaModule";
        public const string HungerModule = "HungerModule";
        public const string ThristModule = "ThristModule";
    }
    
    // public interface IVitality
    // {
    //     int GetCurrentVitalityValueByModuleName(string moduleName);
    //     int GetMaxVitalityValueByModuleName(string moduleName);
    //     void DecreaseVitalityValue(string moduleName, int amount);
    // }

    public class VitalityModuleSystem : MonoBehaviour //, IVitality
    {
        [SerializeField] private bool _healthModule;
        [ShowIf("_healthModule")][SerializeField] private int _healthMaxValue;
        
        [SerializeField] private bool _staminaModule;
        [ShowIf("_staminaModule")][SerializeField] private int _staminaMaxVaule;
        
        [SerializeField] private bool _hungerModule;
        [ShowIf("_hungerModule")][SerializeField] private int _hungerMaxVaule;
        
        [SerializeField] private bool _thristModule;
        [ShowIf("_thristModule")][SerializeField] private int _yhristMaxVaule;

        private List<IVitalityModule> _vitalityModules = new();

        private void Awake()
        {
            if (_healthModule)
            {
                _vitalityModules.Add(new HealthModule(_healthMaxValue, this));
                EventAggregator.Subscribe<HealthEmptyEvent>(OnHealthEmptyHandler);
            }

            if (_staminaModule)
            {
                _vitalityModules.Add(new StaminaModule(_staminaMaxVaule, this));
            }

            if (_hungerModule)
            {
                _vitalityModules.Add(new HungerModule(_hungerMaxVaule, this));
            }

            if (_thristModule)
            {
                _vitalityModules.Add(new ThristModule(_yhristMaxVaule, this));
            }
        }

        private void OnDestroy()
        {
            if (_healthModule)
            {
                EventAggregator.Unsubscribe<HealthEmptyEvent>(OnHealthEmptyHandler);
            }
            
        }

        private void Update()
        {
            foreach (var vitalityModule in _vitalityModules)
                vitalityModule.UpdateModule();
        }
        
        public int GetCurrentVitalityValueByModuleName(string moduleName)
        {
            var vitalityModule = _vitalityModules.First(m => m.GetType().Name == moduleName);
            if (vitalityModule != null)
                return vitalityModule.GetCurrentValue();

            return 0;
        }

        public int GetMaxVitalityValueByModuleName(string moduleName)
        {
            var vitalityModule = _vitalityModules.First(m => m.GetType().Name == moduleName);
            if (vitalityModule != null)
                return vitalityModule.GetMaxValue();
         
            return 0;
        }
        
        public void DecreaseVitalityValue(string moduleName, int amount)
        {
            var vitalityModule = _vitalityModules.FirstOrDefault(m => m.GetType().Name == moduleName);
            if (vitalityModule != null)
            {
                vitalityModule.DecreaseValue(amount);
                Debug.Log($"decrease Vitality Value - {amount}, current Vitality Value - {vitalityModule.GetCurrentValue()}");
            }
        }

        private void Died()
        {
            gameObject.SetActive(false);
            Debug.Log($"{gameObject.name} died and was deactivated.");
        }
        
        private void OnHealthEmptyHandler(object sender, HealthEmptyEvent eventData)
        {
            if (sender == this)
                Died();
        }
    }

    public class HealthModule : VitalityModuleBase
    {
        public HealthModule(int maxValue, MonoBehaviour owner) : base(maxValue, owner) { }
        
        protected override void NotifyChangeVitalityValue()
        {
            // это выполняется когда currentValue 0 
            Debug.Log($"{_currentValue / _maxValue}");
            EventAggregator.Post(_owner, new UpdatedHealthVitalityEvent() {CurrentValue = _currentValue, MaxValue = _maxValue});

            if (_currentValue <= 0)
            {
                EventAggregator.Post(_owner, new HealthEmptyEvent());
            }
        }
    }

    public class StaminaModule : VitalityModuleBase
    {
        public StaminaModule(int maxValue, MonoBehaviour owner) : base(maxValue, owner) { }
        
        protected override void NotifyChangeVitalityValue()
        {
            // это выполняется когда currentValue 0 
        }
    }
    
    public class HungerModule : VitalityModuleBase
    {
        public HungerModule(int maxValue, MonoBehaviour owner) : base(maxValue, owner) { }
        
        protected override void NotifyChangeVitalityValue()
        {
            // это выполняется когда currentValue 0 
        }
    }
    
    public class ThristModule : VitalityModuleBase
    {
        public ThristModule(int maxValue, MonoBehaviour owner) : base(maxValue, owner) { }
        
        protected override void NotifyChangeVitalityValue()
        {
            // это выполняется когда currentValue 0 
        }
    }
    
}