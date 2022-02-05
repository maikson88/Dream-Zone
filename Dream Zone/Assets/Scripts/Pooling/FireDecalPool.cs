using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDecalPool : MonoBehaviour
{
    public static FireDecalPool Instance { get; private set; }

    [SerializeField]
    private GameObject[] _fireDecalPrefabs;
    private Queue<GameObject> _avaliableObjects = new Queue<GameObject>();

    private void Awake()
    {
        Instance = this;
        GrowPool();
    }

    private void GrowPool()
    {
        for (int i = 0; i < 10; i++)
        {
            int randomPrefab = Random.Range(0, _fireDecalPrefabs.Length);
            var instanceToAdd = Instantiate(_fireDecalPrefabs[randomPrefab]);
            instanceToAdd.transform.SetParent(transform);
            AddToPool(instanceToAdd);
        }
    }

    public void AddToPool(GameObject instance)
    {
        _avaliableObjects.Enqueue(instance);
        instance.transform.SetParent(transform, true);
        instance.SetActive(false);
    }

    public GameObject GetFromPool()
    {
        if (_avaliableObjects.Count == 0)
        {
            GrowPool();
        }
        var instance = _avaliableObjects.Dequeue();
        instance.SetActive(true);
        return instance;
    }
}
