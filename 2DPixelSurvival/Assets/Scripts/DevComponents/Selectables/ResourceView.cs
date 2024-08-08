using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

[SelectionBase]
public class ResourceView : PickUpBase
{
    private bool _animatedDropSpawnIsComplete;

    protected override void Awake()
    {
        base.Awake();
        
        AnimateDropResourceSpawn();
    }
    
    public void AnimateDropResourceSpawn()
    {
        // Устанавливаем начальную позицию
        Vector3 startPosition = transform.position;
        var initialScale = transform.localScale;

        // Рассчитываем случайное направление и силу броска
        float randomX = Random.Range(-1f, 1f);
        float randomY = Random.Range(-1f, 1f);
        float throwForceMin = 1f;
        float throwForceMax = 2f;
        float duration = 0.4f;
        
        float randomForce = Random.Range(throwForceMin, throwForceMax);
        Vector3 endPosition = startPosition + new Vector3(randomX, randomY, 0).normalized * randomForce;

        // Рассчитываем контрольную точку для создания кривой (поднимаем вверх по Y)
        Vector3 controlPoint = (startPosition + endPosition) / 2 + Vector3.up;

        // Создаем массив из трех точек для кривой
        Vector3[] path = new Vector3[] { startPosition, controlPoint, endPosition };

        // Запускаем анимацию перемещения по кривой с увеличением масштаба в верхней точке
        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DOPath(path, duration, PathType.CatmullRom)
            .SetEase(Ease.OutQuad));

        // Увеличиваем масштаб к середине пути
        sequence.Insert(0, transform.DOScale(initialScale + (initialScale * 0.4f), duration / 2).SetEase(Ease.OutQuad));
        // Уменьшаем масштаб к концу пути
        sequence.Insert(duration / 2, transform.DOScale(initialScale, duration / 2).SetEase(Ease.InQuad));

        sequence.OnComplete(() => 
        {
            // Действия после завершения анимации (опционально)
            _animatedDropSpawnIsComplete = true;
            Debug.Log("Animation complete!");
        });
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if (_animatedDropSpawnIsComplete) 
            base.OnTriggerEnter2D(collider);
        
    }
 
}