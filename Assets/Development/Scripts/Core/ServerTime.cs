using Action = System.Action;
using DateTime = System.DateTime;

using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using GameHandler;

public class ServerTime : MonoBehaviour
{
    public event Action TimeSuccess;

    [SerializeField] private string URL = "https://yandex.com/time/sync.json";
    [SerializeField] private float _checkTimeMinutes = 60f;

    public bool IsActive { get; private set; } = true;

    private DateTime _currentDateTime;


    private IEnumerator _checkTime;
    private IEnumerator _getNetworkTime;

    public void Init()
    {
        StartCheck();
    }

    public void StartCheck()
    {
        StopAllCoroutine();

        _checkTime = CheckTime();
        _getNetworkTime = GetNetworkTime();

        IsActive = true;
        StartCoroutine(_checkTime);
    }

    public void StopCheck()
    {
        IsActive = false;

        StopAllCoroutine();
    }

    public DateTime GetCurrentDateTime() => _currentDateTime;

    private IEnumerator CheckTime()
    {
        while (true)
        {
            Debug.Log("Проверка времени");

            float seconds = _checkTimeMinutes * 60f;

            if (!IsActive)
                StopCoroutine(_checkTime);

            StartCoroutine(_getNetworkTime);
            yield return new WaitForSeconds(seconds);
        }
    }

    private IEnumerator GetNetworkTime()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(URL);

        yield return webRequest.SendWebRequest();
        string requestText = webRequest.downloadHandler.text;

        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            TimeData timeData = JsonUtility.FromJson<TimeData>(requestText);

            if (string.IsNullOrWhiteSpace(timeData.datetime))
                _currentDateTime = GetISODateTime(timeData.time);
            else
                _currentDateTime = ParseDateTime(timeData.datetime);

            Debug.Log("Готово");
        }
        else
        {
            Debug.Log("Ошибка: " + webRequest.error);
            _currentDateTime = DateTime.Now;
        }

        TimeSuccess?.Invoke();
    }

    private DateTime ParseDateTime(string datetime)
    {
        string date = Regex.Match(datetime, @"^\d{4}-\d{2}-\d{2}").Value;
        string time = Regex.Match(datetime, @"\d{2}:\d{2}:\d{2}").Value;

        return DateTime.Parse(string.Format("{0} {1}", date, time));
    }

    private DateTime GetISODateTime(long milliSeconds)
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc)
            .AddMilliseconds(milliSeconds)
            .ToLocalTime();

        return dateTime;
    }

    private void StopAllCoroutine()
    {
        if (_checkTime != null)
            StopCoroutine(_checkTime);

        if (_getNetworkTime != null)
            StopCoroutine(_checkTime);
    }

    public struct TimeData
    {
        public long time;
        public string datetime;
    }
}
