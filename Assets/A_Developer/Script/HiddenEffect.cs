using UnityEngine;

/// <summary>
/// Returns a pooled effect to its pool after lifetime seconds.
/// </summary>
public class HiddenEffect : MonoBehaviour
{
    [SerializeField] private float lifetime = 2f;

    private GameObject _prefab;

    // Called by PoolManager on Spawn — no manual assignment in the Inspector needed.
    public void Init(GameObject prefab) => _prefab = prefab;

    void OnEnable()
    {
        Invoke(nameof(Hide), lifetime);
    }

    void OnDisable()
    {
        CancelInvoke();
    }

    void Hide()
    {
        if (_prefab != null)
            PoolManager.Instance.Despawn(_prefab, gameObject);
        else
            gameObject.SetActive(false);
    }
}
