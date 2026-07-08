using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    [SerializeField] private Transform parentSpawn;

    private Dictionary<GameObject, Queue<GameObject>> poolDict = new();
    private Dictionary<GameObject, GameObject> sourcePrefab = new();   // instance → prefab it came from

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        if (!poolDict.ContainsKey(prefab))
            poolDict[prefab] = new Queue<GameObject>();

        var pool = poolDict[prefab];
        var obj  = pool.Count > 0 ? pool.Dequeue() : Instantiate(prefab, parentSpawn);

        sourcePrefab[obj] = prefab;          // remember which pool this instance belongs to

        obj.transform.SetPositionAndRotation(pos, rot);
        obj.SetActive(true);
        return obj;
    }

    // Despawn by instance — PoolManager looks up which pool it came from.
    public void Despawn(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(parentSpawn);
        if (sourcePrefab.TryGetValue(obj, out var prefab))
            poolDict[prefab].Enqueue(obj);
    }
}
