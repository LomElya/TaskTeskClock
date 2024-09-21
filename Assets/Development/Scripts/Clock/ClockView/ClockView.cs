using UnityEngine;

public abstract class ClockView : MonoBehaviour
{
    public event System.Action<System.DateTime> ChangeValueTime;

    private IClock _clock;
    private IClock _editerClock;

    protected IClock _currentClock;

    public void Init(IClock clock, IClock editerClock)
    {
        _clock = clock;
        _editerClock = editerClock;
    }

    public void SetClassicMode()
    {
        ChangeClock(_clock);

        ClassicMode();
    }

    public void SetEditorMode()
    {
        ChangeClock(_editerClock);

        EditorMode();
    }

    private void ChangeClock(IClock clock)
    {
        if (_currentClock != null)
            _currentClock.TimeUpdated -= OnTimeUpdated;

        _currentClock = clock;
        _currentClock.TimeUpdated += OnTimeUpdated;
    }

    protected abstract void ClassicMode();
    protected abstract void EditorMode();
    protected abstract void ChangeTime();

    protected void InvokeChangeTime(System.DateTime dateTime) => ChangeValueTime?.Invoke(dateTime);

    private void OnDestroy() => _clock.TimeUpdated -= OnTimeUpdated;
    private void OnTimeUpdated(IClock clock) => ChangeTime();
}
