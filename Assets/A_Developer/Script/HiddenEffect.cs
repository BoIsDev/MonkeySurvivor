using UnityEngine;

public class HiddenEffect : MonoBehaviour
{
    private ParticleSystem ps;

    void OnEnable()
    {
        Invoke(nameof(Hide), 2f);
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }
}