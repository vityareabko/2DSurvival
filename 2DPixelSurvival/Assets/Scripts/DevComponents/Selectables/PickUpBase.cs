using System;
using System.Diagnostics.CodeAnalysis;
using DevSystems;
using Extensions;
using UnityEngine;

[SelectionBase]
public class PickUpBase : MonoBehaviour, ISelectable
{
    public event Action<ISelectable> PickUp;

    [AllowNull][SerializeField] private TargetFollower _targetFollower;
    
    [SerializeField] private PickUpType _pickUpType;
    
    public PickUpType SelectableType => _pickUpType;
    
    protected Transform _target;
    
    protected virtual void Awake()
    {
        if (_targetFollower != null)
            _targetFollower.OnReachedTarget += OnResourceReachedTarget;
    }

    protected virtual void OnDestroy()
    {
        if (_targetFollower != null)
            _targetFollower.OnReachedTarget -= OnResourceReachedTarget;
    }

    private void OnResourceReachedTarget()
    {
        EventAggregator.Post(this, new PickUpEvent() {PickUpType = _pickUpType});
            gameObject.SetActive(false);
        
    }
    
    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer(Layers.Player))
        {
            if (_targetFollower != null)
            {
                if (_targetFollower.IsFollowTarget == false)
                {
                    _target = collider.transform;
                    _targetFollower.Initialize(_target);
                }
            }
        }
    }
}