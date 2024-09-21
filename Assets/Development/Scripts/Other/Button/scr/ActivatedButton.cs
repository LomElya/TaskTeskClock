using UnityEngine;

public class ActivatedButton : CustomButton
{
    [SerializeField] private SettingButton _firstSetting;
    [SerializeField] private SettingButton _secondSetting;

    private bool _isActive = false;

    protected override void OnAwake()
    {
        ChangeButton();
    }

    public override bool GetActive() => _isActive;
    public override void SetActive(bool isActive)
    {
        _isActive = isActive;

        ChangeButton();
    }

    protected override void OnClickButton()
    {
        SetActive(!_isActive);
    }

    private void ChangeButton()
    {
        if (_isActive)
            ChangeButton(_firstSetting);
        else
            ChangeButton(_secondSetting);
    }

    private void ChangeButton(SettingButton setting)
    {
        ChangeColor(setting.Color);
        ChangeText(setting.Text);
    }


    [System.Serializable]
    public class SettingButton
    {
        [field: SerializeField] public Color Color = Color.white;
        [field: SerializeField] public string Text;
    }
}
