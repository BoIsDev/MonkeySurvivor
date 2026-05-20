using UnityEngine;

public class HiddenEffect : MonoBehaviour
{
    [SerializeField] private float lifetime = 2f;

    private GameObject _prefab;

    // Gọi bởi PoolManager lúc Spawn — không cần gán tay trong Inspector
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
