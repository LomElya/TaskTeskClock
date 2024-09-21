using TMPro;
using UnityEngine;

public class DigitalClockView : ClockView
{
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private CustomInputField _inputField;

    protected override void ChangeTime()
    {
        _timeText.text = _currentClock.GetString();

        if (_inputField.Active)
            _inputField.Show(_currentClock);
    }

    protected override void ClassicMode()
    {
        _inputField.OnChangeTime -= InvokeChangeTime;
        _inputField.Hide();
    }

    protected override void EditorMode()
    {
        _inputField.Show(_currentClock);
        _inputField.OnChangeTime += InvokeChangeTime;
    }
}
