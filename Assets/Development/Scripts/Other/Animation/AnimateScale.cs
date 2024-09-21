using DG.Tweening;
using UnityEngine;

public class AnimateScale : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private float _duration = 1f;
    [SerializeField] private Ease _ease;

    private Tweener _tween;

    private void OnEnable() => _tween = _rectTransform.transform.DOScale(1f, _duration).From(0f).SetEase(_ease);
    private void OnDisable() => _tween.Kill();
}
