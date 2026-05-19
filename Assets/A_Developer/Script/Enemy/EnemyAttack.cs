using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private EnemyDataSO data;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject magicProjectilePrefab;

    private float nextAttackTime;

    public void TryAttack(Transform player)
    {
        if (Time.time < nextAttackTime) return;
        nextAttackTime = Time.time + data.attackRate;

        if (data.enemyStyle == EnemyStyle.Melee)
            ExecuteMeleeAttack(player);
        else
            ExecuteMagicAttack(player);
    }

    private void ExecuteMeleeAttack(Transform player)
    {
        animator.SetBool("IsMelee", true);
        animator.SetTrigger("Attacking");

        player.GetComponent<IDamageable>()?.TakeDamage(data.damage);
    }

    private void ExecuteMagicAttack(Transform player)
    {
        animator.SetBool("IsMelee", false);
        animator.SetTrigger("Attacking");
        if (magicProjectilePrefab == null) return;

        var go = Instantiate(magicProjectilePrefab, transform.position, Quaternion.identity);
        go.GetComponent<DamageDealer>()?.SetDamage(data.damage);
    }
}
