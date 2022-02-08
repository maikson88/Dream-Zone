using UnityEngine;

public class Tools
{
    public float lastActive { get; private set; }
    public float lastActiveOnce { get; private set; }
    public bool alreadyUsedOnce { get; private set; }

    public bool TimeHasPassed(float timer)
    {
        if (lastActive == 0) lastActive = Time.time;
        if (Time.time >= (lastActive + timer))
        {
            lastActive = 0;
            return true;
        }
        return false;
    }

    public void ResetTime() => lastActive = 0;

    public bool RunOnceTimer(float timer)
    {
        if (alreadyUsedOnce) return false;
        if (lastActiveOnce == 0) lastActiveOnce = Time.time;
        if (Time.time >= (lastActiveOnce + timer))
        {
            lastActiveOnce = 0;
            alreadyUsedOnce = true;
            return true;
        }
        return false;
    }

    public void RunOnceReset() => alreadyUsedOnce = false;
}
