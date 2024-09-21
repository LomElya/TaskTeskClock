using System;
using UnityEngine;

public class ClockViewManager : MonoBehaviour
{
    [SerializeField] ClockTypeState _firstTypeState = ClockTypeState.ClassicMode;
    [SerializeField] private ClockView[] _clocks;

    private DateTime _dateTime;

    protected IClock _classicClock;
    private Clock _editorClock;

    public void Init(IClock clock)
    {
        _dateTime = clock.TimeDate;

        _classicClock = clock;
        _editorClock = new(this);
        _editorClock.SetTime(_dateTime);

        for (int i = 0; i < _clocks.Length; i++)
            _clocks[i].Init(clock, _editorClock);

        Subscribe();
        SetState(_firstTypeState);
    }

    public void SetState(ClockTypeState type)
    {
        switch (type)
        {
            case ClockTypeState.ClassicMode:
                SetClassicMode();
                break;

            case ClockTypeState.EditorMode:
                SetEditorMode();
                break;
        }
    }

    public DateTime GetDateTime() => _dateTime;

    private void ChangeTime(DateTime dateTime)
    {
        _dateTime = dateTime;
        _editorClock.SetTime(_dateTime);
    }

    private void SetClassicMode()
    {
        for (int i = 0; i < _clocks.Length; i++)
            _clocks[i].SetClassicMode();
    }

    private void SetEditorMode()
    {
        _editorClock.SetTime(_classicClock.TimeDate);

        for (int i = 0; i < _clocks.Length; i++)
            _clocks[i].SetEditorMode();
    }

    private void Subscribe()
    {
        for (int i = 0; i < _clocks.Length; i++)
            _clocks[i].ChangeValueTime += ChangeTime;
    }

    private void Unsubscribe()
    {
        for (int i = 0; i < _clocks.Length; i++)
            _clocks[i].ChangeValueTime -= ChangeTime;
    }

    private void OnDestroy() => Unsubscribe();
}
