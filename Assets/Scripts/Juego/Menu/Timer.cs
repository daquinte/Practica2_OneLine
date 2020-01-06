using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer 
{
    public float timeLeft = 300.0f;
    public float initTime = 300.0f; 
    public void Update() {
        timeLeft -= Time.deltaTime;
    }

    public void SetTime(float time) {
        timeLeft = initTime = time;
    }

    public void ResetClock() {
        initTime = timeLeft;
        TimeManager.instance.SaveDate();
        timeLeft -= TimeManager.instance.CheckDate();
    }
}
