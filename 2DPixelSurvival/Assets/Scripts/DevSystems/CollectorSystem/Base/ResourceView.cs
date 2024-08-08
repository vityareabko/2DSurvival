using DG.Tweening;
using UnityEngine;


[SelectionBase]
public class ResourceView : PickUpBase
{
    protected override void Awake()
    {
        base.Awake();
        
        var scaleInitial = transform.localScale;
        transform.localScale = Vector3.zero;
        transform.DOScale(scaleInitial, 0.1f).SetEase(Ease.InOutBounce);
    }
}

// [SelectionBase]
// public class ResourceView : MonoBehaviour, ISelectable
// {
//     public event Action<ISelectable> PickUp;
//
//     [field: SerializeField] public PickUpType SelectableType { get; private set; }
//     [SerializeField] private TargetFollower _targetFollower;
//     
//     private Transform _target;
//
//     private void Awake()
//     {
//         var scaleInitial = transform.localScale;
//         transform.localScale = Vector3.zero;
//         transform.DOScale(scaleInitial, 0.1f).SetEase(Ease.InOutBounce);
//         
//         _targetFollower.OnReachedTarget += OnResourceReachedTarget;
//     }
//
//     private void OnDestroy()
//     {
//         _targetFollower.OnReachedTarget -= OnResourceReachedTarget;
//     }
//
//     private void OnTriggerEnter2D(Collider2D collider)
//     {
//         if (collider.gameObject.layer == LayerMask.NameToLayer(Layers.Player))
//         {
//             _target = collider.transform;
//             _targetFollower.Initialize(_target);
//         }
//     }
//
//     private void OnResourceReachedTarget()
//     {
//         EventAggregator.Post(this, new PickUpEvent());
//     }
// }