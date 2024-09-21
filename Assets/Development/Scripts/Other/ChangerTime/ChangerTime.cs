using UnityEngine;
using DateTime = System.DateTime;

public abstract class ChangerTime : MonoBehaviour
{
    public System.Action<DateTime> OnChangeTime;

    public bool Active { get; private set; }

    public virtual void Show(IClock clock)
    {
        Active = true;
        gameObject.SetActive(Active);
        OnShow(clock);
    }

    public virtual void Hide()
    {
        Active = false;
        gameObject.SetActive(Active);
        OnHide();
    }

    public abstract DateTime GetDateTime();

    protected void InvokeChangeTime() => OnChangeTime?.Invoke(GetDateTime());

    protected virtual void OnShow(IClock clock) { }
    protected virtual void OnHide() { }
}
