using GameHandler;
using UnityEngine;

public class ClockPresenter : MonoBehaviour
{
    [SerializeField] private ClockViewManager _clockViewManager;

    [Header("Buttons")]
    [SerializeField] private CustomButton _editButton;
    [SerializeField] private CustomButton _resetButton;
    [SerializeField] private CustomButton _saveButton;

    private ServerTime _secverTime;
    private Clock _clock;

    public void Init(UpdateHandler updateHandler, ServerTime serverTime)
    {
        _secverTime = serverTime;

        Subscribe();

        _clock = new(updateHandler);
        _clockViewManager.Init(_clock);
        // _inputField.Hide();
    }

    private void OnTimeSuccess()
    {
        _clock.SetTime(_secverTime.GetCurrentDateTime());
        _clock.Start();
        _editButton.SetActive(true);
        OnClickEditButton();
    }

    private void OnClickEditButton()
    {
        bool isEdit = _editButton.GetActive();
        _clock.SetPause(!isEdit);

        _resetButton.Show(isEdit);
        _saveButton.Show(!isEdit);

        if (isEdit)
            _clockViewManager.SetState(ClockTypeState.ClassicMode);
        else
            _clockViewManager.SetState(ClockTypeState.EditorMode);
    }

    private void OnClickResetButton()
    {
        _secverTime.StartCheck();
    }

    private void OnClickSaveButton()
    {
        _secverTime.StopCheck();

        _clock.SetTime(_clockViewManager.GetDateTime());

        _editButton.SetActive(true);
        OnClickEditButton();
    }

    #region Subscribe
    private void Subscribe()
    {
        _secverTime.TimeSuccess += OnTimeSuccess;
        _editButton.OnClick += OnClickEditButton;
        _resetButton.OnClick += OnClickResetButton;
        _saveButton.OnClick += OnClickSaveButton;
    }

    private void Unsubscribe()
    {
        _secverTime.TimeSuccess -= OnTimeSuccess;
        _editButton.OnClick -= OnClickEditButton;
        _resetButton.OnClick -= OnClickResetButton;
        _saveButton.OnClick -= OnClickSaveButton;
    }
    #endregion Subscribe

    private void OnDestroy() => Unsubscribe();
}
