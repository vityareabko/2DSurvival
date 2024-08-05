using UnityEngine;

namespace DevSystems.VitalitySystem
{
    public interface IVitalityModule
    {
        void UpdateModule();
        int GetCurrentValue();
        int GetMaxValue();
        void DecreaseValue(int amount);
    }
    
    public abstract class VitalityModuleBase : IVitalityModule
    {
        protected readonly MonoBehaviour _owner;
        protected int _maxValue;
        protected int _currentValue;

        protected VitalityModuleBase(int maxValue, MonoBehaviour owner)
        {
            _owner = owner;
            _maxValue = maxValue;
            _currentValue = maxValue;
        }

        public virtual void UpdateModule()
        {
        }
        
        public virtual int GetCurrentValue() => _currentValue;

        public virtual int GetMaxValue() => _maxValue;
        
        public void DecreaseValue(int amount)
        {
            _currentValue -= amount;
            if (_currentValue < 0)
            {
                _currentValue = 0;
            }
            
            NotifyChangeVitalityValue();
            
        }

        protected abstract void NotifyChangeVitalityValue();
    }
}