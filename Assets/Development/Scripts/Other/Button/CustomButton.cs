using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(Image))]
public class CustomButton : MonoBehaviour
{
    public event System.Action OnClick;

    [SerializeField] private TMP_Text _text;

    private Button _button;
    private Image _imageButton;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _imageButton = GetComponent<Image>();
        OnAwake();
    }

    public void Show(bool isShow)
    {
        if (isShow)
            Show();
        else
            Hide();
    }

    public void ChangeColor(Color color) => _imageButton.color = color;
    public void ChangeText(string text) => _text.text = text;
    public void Show() => ChangeShow(true);
    public void Hide() => ChangeShow(false);
    public virtual bool GetActive() => _button.interactable;
    public virtual void SetActive(bool isActive) => _button.interactable = isActive;

    private void ClickButton()
    {
        OnClickButton();
        InvokeClick();
    }

    private void ChangeShow(bool isShow)
    {
        gameObject.SetActive(isShow);
        SetActive(isShow);
    }

    private void OnEnable() => _button.onClick.AddListener(ClickButton);
    private void OnDisable() => _button.onClick.RemoveListener(ClickButton);
    private void InvokeClick() => OnClick?.Invoke();

    protected virtual void OnClickButton() { }
    protected virtual void OnAwake() { }
}
