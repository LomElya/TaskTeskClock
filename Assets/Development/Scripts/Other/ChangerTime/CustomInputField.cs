using System.Text;
using DateTime = System.DateTime;
using TMPro;
using UnityEngine;
using System.Globalization;

[RequireComponent(typeof(TMP_InputField))]
public class CustomInputField : ChangerTime
{
    private TMP_InputField _inputField;

    private const string _timeFormat = "00:00:00";

    private void Awake()
    {
        _inputField = GetComponent<TMP_InputField>();
        _inputField.text = _timeFormat;

        Subscribe();
    }

    protected override void OnShow(IClock clock)
    {
        _inputField.text = clock.GetString();
    }

    public override DateTime GetDateTime()
    {
        if (DateTime.TryParseExact(_inputField.text, "HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            return result;

        Debug.LogError("Не удалось проанализировать время " + _inputField.text);
        return DateTime.Now;
    }

    #region Events
    //Событие на изменение текста(удаление текста)
    private void OnValueChanged(string text)
    {
        if (_inputField.text.Length == _timeFormat.Length)
            return;

        ChangeTime(GetDeleteText(text));
    }

    //Событие на нажатие клавиш и изменение текста
    private char ValidateInput(string text, int charIndex, char addedChar)
    {
        if (charIndex + 1 > _timeFormat.Length)
            return '\0';

        if (!char.IsDigit(addedChar))
            return '\0';

        if (_timeFormat[charIndex] == ':')
        {
            _inputField.stringPosition = charIndex + 1;
            return ValidateInput(text, charIndex + 1, addedChar);
        }

        if (!IsValidTime(text, charIndex, addedChar))
            return '\0';

        _inputField.stringPosition = charIndex + 1;

        ChangeTime(ModifiedText(text, charIndex, addedChar));

        return '\0';
    }

    private void ChangeTime(string text)
    {
        _inputField.text = text;
        OnChangeTime?.Invoke(GetDateTime());
    }

    #endregion Events

    #region IsValidTime
    private bool IsValidTime(string text, int charIndex, char addedChar)
    {
        StringBuilder currentText = new(text);

        if (charIndex < currentText.Length)
            currentText[charIndex] = addedChar;
        else
            currentText.Append(addedChar);

        if (!IsValidHour(currentText, charIndex) ||
            !IsValidMinute(currentText, charIndex) ||
            !IsValidSecond(currentText, charIndex))
            return false;


        return true;
    }

    private const int maxHour = 24;
    private const int maxMinuteAndSecond = 60;

    private bool IsValidHour(StringBuilder text, int charIndex) => TryParceInt(text, charIndex, 0, 2, maxHour);
    private bool IsValidMinute(StringBuilder text, int charIndex) => TryParceInt(text, charIndex, 3, 5, maxMinuteAndSecond);
    private bool IsValidSecond(StringBuilder text, int charIndex) => TryParceInt(text, charIndex, 6, 8, maxMinuteAndSecond);

    private bool TryParceInt(StringBuilder text, int index, int startIndex, int endIndex, int maxValue)
    {
        if (endIndex > text.Length)
            return false;

        if (index >= startIndex && index < endIndex)
        {
            if (int.TryParse(text.ToString(startIndex, 2), out int seconds) && seconds >= maxValue)
                return false;
        }

        return true;
    }
    #endregion IsValidTime

    #region  Modifications
    private string ModifiedText(string text, int index, char symbol = '\0')
    {
        StringBuilder builder = new(text);

        if (index >= 0 && index < text.Length)
            builder[index] = symbol;

        return builder.ToString(); ;
    }

    private string GetDeleteText(string text)
    {
        if (text.Length == 0)
            return _timeFormat;

        StringBuilder builder = new(text);

        for (int i = 0; i < _timeFormat.Length; i++)
        {
            if (_timeFormat[i] == ':')
            {
                if (i >= builder.Length || builder[i] != ':')
                    builder.Insert(i, ':');
            }
            else
            {
                if (i >= builder.Length || !char.IsDigit(builder[i]))
                    builder.Insert(i - 1, '0');
            }
        }

        if (builder.Length > _timeFormat.Length)
            builder.Length = _timeFormat.Length;

        return builder.ToString();
    }
    #endregion  Modifications

    private void Subscribe()
    {
        _inputField.onValueChanged.AddListener(OnValueChanged);
        _inputField.onValidateInput += ValidateInput;
    }

    private void Unsubscribe()
    {
        _inputField.onValueChanged.RemoveListener(OnValueChanged);
        _inputField.onValidateInput -= ValidateInput;
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }
}