using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class TimeCounter : MonoBehaviour
{
    public Text timeCounter;

    private TimeSpan timeplaying;
    private bool timerGoing;
    private float elapsedTime;
    private float currentTime;
    void Start()
    {
        timerGoing = false;
        elapsedTime = 0f;
        currentTime = 0f;
        timeCounter.text = timeplaying.ToString("mm':'ss'.'ff");
    }

    public void BeginTimer()
    {
        elapsedTime = currentTime;
        timerGoing = true;

        StartCoroutine(UpdateTimer());
    }

    public void EndTimer()
    {
        timerGoing = false;
    }


    public void EndTimerWin()
    {
        currentTime = elapsedTime;
        timerGoing = false;
    }

    public string GetTimeSpent()
    {
        string timeplayingStr = timeplaying.ToString("mm':'ss'.'ff");
        return timeplayingStr;
    }

    public void SetSecond(float time)
    {
        elapsedTime = time;
    }

    public float GetSecond()
    {
        return elapsedTime;
    }

    private IEnumerator UpdateTimer()
    {
        string timeplayingStr;
        while (timerGoing)
        {
            elapsedTime += Time.deltaTime;
            timeplaying = TimeSpan.FromSeconds(elapsedTime);
            timeplayingStr = timeplaying.ToString("mm':'ss'.'ff");
            timeCounter.text = timeplayingStr;

            // Debug.Log(timeplayingStr);

            yield return null;
        }
        timeplaying = TimeSpan.FromSeconds(elapsedTime);
        timeplayingStr = timeplaying.ToString("mm':'ss'.'ff");
        timeCounter.text = timeplayingStr;

        // while(timerGoing)
        // {
        //     elapsedTime += Time.deltaTime;
        //     timeplaying = TimeSpan.FromSeconds(elapsedTime);
        //     string timeplayingStr = timeplaying.ToString("mm':'ss'.'ff");
        //     timeCounter.text = timeplayingStr;

        //     yield return null;
        // }
    }
}
