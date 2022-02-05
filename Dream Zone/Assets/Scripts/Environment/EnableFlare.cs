using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableFlare : MonoBehaviour
{
    public GameObject spawnPoint;
    private GameObject _instance;
    public bool _flareOn = false;
    public float _repeaterTimer;
    Tools tools;

    public enum State
    {
        Visible,
        Invisible,
    }
    public State state;


    private void Start()
    {
        tools = new Tools();
        spawnPoint = gameObject.transform.GetChild(0).gameObject;
        _repeaterTimer = Random.Range(3, 10);
        state = State.Invisible;
    }

    private void Update()
    {
        switch (state)
        {
            case State.Visible:
                if (tools.TimeHasPassed(_repeaterTimer))
                    FlareActiveTime();
                break;

            case State.Invisible:
                ReturnToPool();
                break;
        }
    }

    private void FlareActiveTime()
    {
        _flareOn = !_flareOn;

        if (_flareOn)
            FlareOn();
        else
            ReturnToPool();
    }

    private void FlareOn()
    {
        _instance = LensFlarePool.Instance.GetFromPool();
        _instance.transform.SetParent(spawnPoint.transform, false);
        float randomPosition = Random.Range(-3f, 3f);
        _instance.transform.position = new Vector3(
            _instance.transform.position.x + randomPosition,
            _instance.transform.position.y + randomPosition,
            _instance.transform.position.z + randomPosition
            );
    }

    private void ReturnToPool()
    {
        if (_instance == null) return;
        _instance.transform.position = Vector3.zero;
        LensFlarePool.Instance.AddToPool(_instance);
        _instance = null;
    }

    void OnBecameVisible()
    {
        state = State.Visible;
    }
    void OnBecameInvisible()
    {
        state = State.Invisible;
    }

    private void OnDisable()
    {

    }
}
