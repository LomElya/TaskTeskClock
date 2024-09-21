using DG.Tweening;
using UnityEngine;

public class RotateAnimation : MonoBehaviour
{
    [SerializeField] private Transform _rotating;
    [SerializeField] private float _durationAnimation = 0.1f;

    private Tween _tween;

    public void Animate(float endZValue) => Animate(endZValue, _durationAnimation);

    public void Animate(float endZValue, float duration)
    {
        CheckTween();

        Vector3 endAnimateValue = new Vector3(0, 0, endZValue);

        _rotating.transform.DOLocalRotate(
            endAnimateValue,
            duration)
            .SetEase(Ease.Linear);
    }

    private void CheckTween()
    {
        if (_tween != null && _tween.IsActive())
            _tween.Kill();
    }

    private void OnDestroy() => CheckTween();
}
