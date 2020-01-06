using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float timeLeft = 300.0f;

    void Start() {
        InitClock(timeLeft);
    }
    void Update() {
        if (timeLeft > 0)
            timeLeft -= Time.deltaTime;
    }

    public void ResetClock(float time) {
        TimeManager.instance.SaveDate();
        InitClock(time);
    }
    public void InitClock(float time) {
        timeLeft = time;
        timeLeft -= TimeManager.instance.CheckDate();
    }
}
