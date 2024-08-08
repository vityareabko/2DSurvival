using System;
using UnityEngine;

public class TargetFollower : MonoBehaviour
{
    public event Action OnReachedTarget;
        
    [SerializeField] private Vector3 _targetOffset;
    
    [HideInInspector] public bool IsFollowTarget;

    [SerializeField] private bool CurveByX = true;
    [SerializeField] private bool CurveByY = true;
    
    private Transform _target;
    private Vector3 _initialScale;
    private Vector3 _currentTargetPosition = new Vector3(0f, 0.5f, 0f);
    private float _progressAnimation;
    private float _curveX = 0f;
    private float _curveY = 0f;

    
    public void Initialize(Transform target)
    {
        _target = target;
        _progressAnimation = 0f;
        IsFollowTarget = true;
        _curveX = 0f;
        _curveY = 0f;
        _initialScale = transform.localScale;
    }

    private void FixedUpdate()
    {
        if (IsFollowTarget)
        {
            UpdateMovement();
        }
    }

    private void UpdateMovement()
    {
        _progressAnimation += Time.deltaTime / 0.5f;
        _progressAnimation = Mathf.Clamp01(_progressAnimation);

        Vector3 startPosition = transform.position;
        Vector3 endPosition = _target.position + _targetOffset;
        
        if (_curveX == 0 && CurveByX)
            _curveX = UnityEngine.Random.Range(-3f, 3f);
        if (_curveY == 0 && CurveByY)
            _curveY = UnityEngine.Random.Range(1f, 2f);

        Vector3 controlPoint = (startPosition + endPosition) / 2 + new Vector3(_curveX, _curveY, 0);
        Vector3 midPoint = Vector3.Lerp(startPosition, controlPoint, _progressAnimation * 0.5f);
        Vector3 newPos = Vector3.Lerp(midPoint, endPosition, _progressAnimation);

        transform.position = newPos;

        _currentTargetPosition = endPosition;
        
        
        if (_progressAnimation < 0.5f)
        {
            float scaleMultiplier = 1 + 0.2f * (_progressAnimation / 0.5f);
            transform.localScale = _initialScale * scaleMultiplier;
        }
        else
        {
            float scaleMultiplier = 1.2f - 0.8f * ((_progressAnimation - 0.5f) / 0.5f);
            transform.localScale = _initialScale * scaleMultiplier;
        }

        if (Vector3.Distance(newPos, endPosition) < 0.1f)
        {
            StopMovement();
        }
    }

    private void StopMovement()
    {
        IsFollowTarget = false;
        _curveX = 0f;
        _curveY = 0f;
        OnReachedTarget?.Invoke();
    }

    private void OnDrawGizmos()
    {
        if (IsFollowTarget)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_currentTargetPosition, 0.1f);
        }
    }
}
