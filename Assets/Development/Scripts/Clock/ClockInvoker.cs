using System;
using System.Collections;
using GameHandler;
using UnityEngine;

public class ClockInvoker : IDisposable
{
    public event Action SecondsUpdates;

    private IEnumerator _countdown;
    private MonoBehaviour _context;
    private UpdateHandler _updateHandler;

    private float _oneSecond = 0;

    public ClockInvoker(UpdateHandler updateHandler) => _updateHandler = updateHandler;
    public ClockInvoker(MonoBehaviour contex) => _context = contex;

    public void StartUpdate(float starcSeconds = 0)
    {
        _oneSecond = starcSeconds;

        _countdown = Countdown();

        if (_context != null)
            _context.StartCoroutine(_countdown);
        else if (_updateHandler != null)
            _updateHandler.startCoroutine(_countdown);
    }

    public void Stop()
    {
        if (_countdown == null)
            return;

        if (_context != null)
            _context.StopCoroutine(_countdown);
        else if (_updateHandler != null)
            _updateHandler.stopCoroutine(_countdown);

        _countdown = null;
    }

    private void InvokeUpdate() => SecondsUpdates?.Invoke();

    private IEnumerator Countdown()
    {
        while (true)
        {
            float delta = Time.deltaTime;

            _oneSecond += delta;

            if (_oneSecond >= 1f)
            {
                _oneSecond -= _oneSecond;
                InvokeUpdate();
            }

            yield return null;
        }
    }

    public void Dispose() => Stop();
}
