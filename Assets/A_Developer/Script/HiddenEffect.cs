using UnityEngine;

public class HiddenEffect : MonoBehaviour
{
    private GameObject _prefab;

    // Gọi bởi PoolManager lúc Spawn — không cần gán tay trong Inspector
    public void Init(GameObject prefab) => _prefab = prefab;

    void OnEnable()
    {
        Invoke(nameof(Hide), 2f);
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
