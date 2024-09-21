using GameHandler;
using UnityEngine;

public class Startup : MonoBehaviour
{
    [SerializeField] private UpdateHandler _updateHandler;
    [SerializeField] private ServerTime _secverTime;
    [SerializeField] private ClockPresenter _clockPresenter;

    private void Awake()
    {
        Register();
    }

    private void Register()
    {
        _clockPresenter.Init(_updateHandler, _secverTime);
        _secverTime.Init();
    }
}
