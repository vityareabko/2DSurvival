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
            _curveX = UnityEngine.Random.Range(-10f, 10f);
        if (_curveY == 0 && CurveByY)
            _curveY = UnityEngine.Random.Range(2f, 3f);

        Vector3 controlPoint = CalculateControlPoint(startPosition, endPosition);
        Vector3 newPos = CalculateNewPosition(startPosition, controlPoint, endPosition);

        transform.position = newPos;

        _currentTargetPosition = endPosition;

        if (HasReachedTarget(newPos, endPosition))
        {
            StopMovement();
        }
    }

    private Vector3 CalculateControlPoint(Vector3 startPosition, Vector3 endPosition)
    {
        return (startPosition + endPosition) / 2 + new Vector3(_curveX, _curveY, 0);
    }

    private Vector3 CalculateNewPosition(Vector3 startPosition, Vector3 controlPoint, Vector3 endPosition)
    {
        Vector3 midPoint = Vector3.Lerp(startPosition, controlPoint, _progressAnimation * 0.3f);
        return Vector3.Lerp(midPoint, endPosition, _progressAnimation);
    }

    private bool HasReachedTarget(Vector3 currentPosition, Vector3 endPosition)
    {
        return Vector3.Distance(currentPosition, endPosition) < 0.1f;
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
