using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    [SerializeField] private Transform parentSpawn;

    private Dictionary<GameObject, Queue<GameObject>> _poolDict = new();

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        if (!_poolDict.ContainsKey(prefab))
            _poolDict[prefab] = new Queue<GameObject>();

        var pool = _poolDict[prefab];
        var obj  = pool.Count > 0 ? pool.Dequeue() : Instantiate(prefab, parentSpawn);

        obj.transform.SetPositionAndRotation(pos, rot);

        // Auto-inject the prefab reference into HiddenEffect so it despawns back into the correct queue.
        var hidden = obj.GetComponent<HiddenEffect>();
        if (hidden != null) hidden.Init(prefab);

        obj.SetActive(true);
        return obj;
    }

    public void Despawn(GameObject prefab, GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(parentSpawn);
        _poolDict[prefab].Enqueue(obj);
    }
}
