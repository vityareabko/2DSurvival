using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayCamera : MonoBehaviour
{
    public Transform target;            
    public float smoothSpeed = 0.125f;  
    public Vector3 offset;              

    public Vector2 minBound;           
    public Vector2 maxBound;            

    
    private float halfHeight;           
    private float halfWidth;

    private void Start()
    {
        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;
    }

    private void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        float clampedX = Mathf.Clamp(smoothedPosition.x, minBound.x + halfWidth, maxBound.x - halfWidth);
        float clampedY = Mathf.Clamp(smoothedPosition.y, minBound.y + halfHeight, maxBound.y - halfHeight);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    private void OnDrawGizmos()
    {
        // Рисуем границы камеры в редакторе
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(minBound.x, minBound.y, transform.position.z), new Vector3(maxBound.x, minBound.y, transform.position.z));
        Gizmos.DrawLine(new Vector3(minBound.x, maxBound.y, transform.position.z), new Vector3(maxBound.x, maxBound.y, transform.position.z));
        Gizmos.DrawLine(new Vector3(minBound.x, minBound.y, transform.position.z), new Vector3(minBound.x, maxBound.y, transform.position.z));
        Gizmos.DrawLine(new Vector3(maxBound.x, minBound.y, transform.position.z), new Vector3(maxBound.x, maxBound.y, transform.position.z));
    }
}
