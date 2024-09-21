using UnityEngine;

public class AnalogClockView : ClockView
{
    [SerializeField] private HandClockPresenter _handPresenter;

    protected override void ChangeTime() => _handPresenter.AnimateAll(_currentClock);

    protected override void ClassicMode()
    {
        _handPresenter.OnChangeTime -= InvokeChangeTime;
        _handPresenter.Hide();
    }

    protected override void EditorMode()
    {
        _handPresenter.Show(_currentClock);
        _handPresenter.OnChangeTime += InvokeChangeTime;
    }
}