using UnityEngine;

namespace DevSystems.VitalitySystem
{
    public interface IVitalityModule
    {
        void UpdateModule();
        int GetCurrentValue();
        int GetMaxValue();
        void DecreaseValue(int decraseAmount);
        void RecoverValue(int recoverValue, bool notify = true);
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
        
        public void DecreaseValue(int decraseAmount)
        {
            _currentValue -= decraseAmount;
            if (_currentValue < 0)
            {
                _currentValue = 0;
            }
            
            NotifyChangeVitalityValue(decraseAmount);
            
        }

        public void RecoverValue(int recoverValue, bool notify = true)
        {
            if (_currentValue >= _maxValue)
                _currentValue = _maxValue;
            else
            {
                _currentValue += recoverValue;
            }
            
            if (notify)
                NotifyChangeVitalityValue(recoverValue);
        }

        protected abstract void NotifyChangeVitalityValue(int damage);
    }
}