using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace DevPlayer
{
    public abstract class BaseWorldUI : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        
        protected virtual void Awake()
        {
            if (_canvas.worldCamera == null)
                _canvas.worldCamera = Camera.main;
        }
    }
}