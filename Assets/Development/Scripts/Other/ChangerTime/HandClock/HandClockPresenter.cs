using System;
using System.Globalization;
using UnityEngine;

public class HandClockPresenter : ChangerTime
{
    [SerializeField] private HandClock[] _hands;

    private const float halfMinute = 30f;
    private const float degreesHour = 0.5f;
    private const float degreesMinuteAndSecond = 6f;

    private float _hour;
    private float _minute;
    private float _second;

    private string _time => string.Format("{0:00}:{1:00}:{2:00}", _hour, _minute, _second);

    private void Awake()
    {
        Camera camera = Camera.main;

        for (int i = 0; i < _hands.Length; i++)
            _hands[i].Init(camera);
    }

    public override DateTime GetDateTime()
    {
        if (DateTime.TryParseExact(_time, "HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            return result;

        Debug.LogError("Не удалось проанализировать время " + _time);
        return DateTime.Now;
    }

    public void AnimateAll(IClock clock)
    {
        _hour = clock.Hour % 12;
        _minute = clock.Minute;
        _second = clock.Second;

        for (int i = 0; i < _hands.Length; i++)
            Animate(_hands[i]);
    }

    public override void Show(IClock clock)
    {
        for (int i = 0; i < _hands.Length; i++)
            _hands[i].SetActive(true);

        Subscribe();
    }

    public override void Hide()
    {
        for (int i = 0; i < _hands.Length; i++)
            _hands[i].SetActive(false);

        Unsubscribe();
    }

    private void OnDragHand(HandClock clock)
    {
        float angle = clock.Angle;
        angle = NormalizeAngle(angle);

        if (clock.HandType == TypeHandClock.Hour)
        {
            float hours = angle / halfMinute;
            _hour = hours % 12;

            if (_hour > 24)
                _hour = 0;
        }
        else if (clock.HandType == TypeHandClock.Minute)
        {
            float minutes = angle / degreesMinuteAndSecond;
            _minute = minutes % (halfMinute * 2f);

            if (_minute > 59)
                _minute = 0;
        }
        else if (clock.HandType == TypeHandClock.Second)
        {
            float seconds = angle / degreesMinuteAndSecond;
            _second = seconds % (halfMinute * 2f);

            if (_second > 59)
                _second = 0;

            Debug.Log(_second);
        }

        InvokeChangeTime();
    }

    private float NormalizeAngle(float angle)
    {
        angle = angle % 360f;

        if (angle < 0)
            angle += 360f;

        return angle;
    }

    private void Animate(HandClock hand)
    {
        if (hand.HandType == TypeHandClock.Hour)
        {
            float endZValueHour = -_hour * halfMinute - _minute * degreesHour;
            hand.Animate(endZValueHour);
            return;
        }

        else if (hand.HandType == TypeHandClock.Minute)
        {
            float endZValueHourMinute = -_minute * degreesMinuteAndSecond;
            hand.Animate(endZValueHourMinute);
            return;
        }

        else if (hand.HandType == TypeHandClock.Second)
        {
            float endZValueHourSecond = -_second * degreesMinuteAndSecond;
            hand.Animate(endZValueHourSecond);
            return;
        }
    }

    private void Subscribe()
    {
        for (int i = 0; i < _hands.Length; i++)
        {
            _hands[i].Drag += OnDragHand;
        }
    }

    private void Unsubscribe()
    {
        for (int i = 0; i < _hands.Length; i++)
        {
            _hands[i].Drag -= OnDragHand;
        }
    }

    private void OnDestroy() => Unsubscribe();
}
