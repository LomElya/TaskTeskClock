using DateTime = System.DateTime;
using GameHandler;
using UnityEngine;

public class Clock : IClock
{
    public event System.Action<IClock> TimeUpdated;

    private ClockInvoker _clockInvoker;
    private DateTime _dateTime;

    private double _seconds = 0;

    public DateTime TimeDate => GetCurrentDateTime();
    public float Hour => TimeDate.Hour;
    public float Minute => TimeDate.Minute;
    public float Second => TimeDate.Second;

    public bool IsActive { get; private set; }
    public bool IsPaused { get; private set; }


    #region CustomUpdate
    public Clock(UpdateHandler updateHandler) => SetTimeInvoker(updateHandler);
    public Clock(UpdateHandler updateHandler, DateTime dateTime)
    {
        SetTimeInvoker(updateHandler);
        SetTime(dateTime);
    }
    #endregion CustomUpdate

    public Clock(MonoBehaviour context) => SetTimeInvoker(context);
    public Clock(MonoBehaviour context, DateTime dateTime)
    {
        SetTimeInvoker(context);
        SetTime(dateTime);
    }

    public void SetTime(DateTime dateTime)
    {
        _seconds = 0;
        _dateTime = dateTime;

        InvokeTime();
    }

    public void Start()
    {
        if (IsActive)
            return;

        IsActive = true;
        IsPaused = false;
        Subscribe();

        _clockInvoker.StartUpdate(_dateTime.Second);

        InvokeTime();
    }

    public void Start(DateTime dateTime)
    {
        if (IsActive)
            return;

        SetTime(dateTime);
        Start();
    }

    public void SetPause(bool isPaused)
    {
        if (isPaused)
            Pause();
        else
            Unpause();
    }

    public void Pause()
    {
        if (IsPaused || !IsActive)
            return;

        IsPaused = true;

        Unsubscribe();
        InvokeTime();
    }

    public void Unpause()
    {
        if (!IsPaused || !IsActive)
            return;

        IsPaused = false;

        Subscribe();
        InvokeTime();
    }

    private void OnSecondsUpdates()
    {
        if (IsPaused)
            return;

        _seconds++;
        InvokeTime();
    }

    private void SetTimeInvoker(MonoBehaviour context) => _clockInvoker = new ClockInvoker(context);
    private void SetTimeInvoker(UpdateHandler updateHandler) => _clockInvoker = new ClockInvoker(updateHandler);

    private DateTime GetCurrentDateTime() => _dateTime.AddSeconds(_seconds);

    private void Subscribe() => _clockInvoker.SecondsUpdates += OnSecondsUpdates;
    private void Unsubscribe() => _clockInvoker.SecondsUpdates -= OnSecondsUpdates;

    private void InvokeTime() => TimeUpdated?.Invoke(this);

    public string GetString() => string.Format("{0:00}:{1:00}:{2:00}", Hour, Minute, Second);
}

public interface IClock
{
    event System.Action<IClock> TimeUpdated;

    DateTime TimeDate { get; }

    float Hour { get; }
    float Minute { get; }
    float Second { get; }

    bool IsActive { get; }
    bool IsPaused { get; }

    string GetString();
}
