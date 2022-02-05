using UnityEngine;

public class Tools
{
    public float lastActive { get; private set; }

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

    public void RunOnce()
    {

    }
}
