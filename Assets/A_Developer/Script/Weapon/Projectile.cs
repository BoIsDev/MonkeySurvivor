using UnityEngine;

/// <summary>
/// Flies straight in a fixed direction at constant speed.
/// Hit detection and pooling are handled by DamageDealer + HiddenEffect on the same prefab.
/// </summary>
public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;

    private Vector3 _direction;

    // Called by the weapon right after spawn to set the flight direction.
    public void Launch(Vector3 direction) => _direction = direction.normalized;

    private void Update()
    {
        transform.position += _direction * (speed * Time.deltaTime);
    }
}