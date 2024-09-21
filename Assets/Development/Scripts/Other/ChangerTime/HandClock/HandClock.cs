using UnityEngine;
using UnityEngine.EventSystems;

public class HandClock : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public System.Action<HandClock> Drag;

    [SerializeField] private RotateAnimation _rotateAnimation;
    [SerializeField] private TypeHandClock _handType;

    public float Angle => _currentAngle;
    public TypeHandClock HandType => _handType;

    public bool Selected { get; private set; } = false;

    private bool _isActive = false;

    private float _currentAngle;

    private Camera _camera;

    public void Init(Camera camera)
    {
        _camera = camera;
    }

    public void SetActive(bool isActive) => _isActive = isActive;

    public void Animate(float angle)
    {
        _rotateAnimation.Animate(angle);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!_isActive)
            return;

        Selected = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_isActive)
            return;

        if (!Selected)
            return;

        Vector3 objectPosition = _camera.WorldToScreenPoint(_rotateAnimation.transform.position);
        Vector2 objectPosition2D = new Vector2(objectPosition.x, objectPosition.y);

        Vector2 currentPointerPosition = eventData.position;
        Vector2 direction = currentPointerPosition - objectPosition2D;

        _currentAngle = Vector2.SignedAngle(Vector2.up, direction);

        if (_handType == TypeHandClock.Hour)
            _currentAngle = Mathf.Clamp(_currentAngle, -360f, 360f);

        _currentAngle *= -1f;

        // Animate(_currentAngle);
        Drag?.Invoke(this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!_isActive)
            return;

        if (!Selected)
            return;

        Selected = false;
    }
}
