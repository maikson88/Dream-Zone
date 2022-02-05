using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalBehavior : MonoBehaviour
{
    public float max = 1f;
    [Range(0,1)]
    public float timeToReach = .3f;
    public float alphaValue;
    public SpriteRenderer decal;

    private void Awake()
    {
        decal.color = new Color(1f, 1f, 1f, 1f);
        alphaValue = 1;
    }

    void Update()
    {
        alphaValue = Mathf.Lerp(alphaValue, 0f, (Time.deltaTime / timeToReach));
        decal.color = new Color(1f, 1f, 1f, alphaValue);

        if (alphaValue <= 0.1f)
            ReturnToPool();
    }

    private void OnDisable()
    {
        decal.color = new Color(1f, 1f, 1f, 1f);
        alphaValue = 1;
    }

    private void ReturnToPool()
    {
        FireDecalPool.Instance.AddToPool(gameObject);
    }
}
