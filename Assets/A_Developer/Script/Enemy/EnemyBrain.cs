using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    public enum StateEnemy
    {
        Idle,
        Run,
        Attack,
        Dead
    }

    [Header("Refs")] [SerializeField] private Animator animator;
    [SerializeField] private EnemyData data;

    private Transform player;
    private StateEnemy currentState;

    private float nextAttackTime;

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        SetState(StateEnemy.Idle);
    }

    void Update()
    {
        if (player == null || currentState == StateEnemy.Dead) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Decide state
        if (distance > data.attackRange)
        {
            SetState(StateEnemy.Run);
        }
        else
        {
            SetState(StateEnemy.Attack);
        }

        // Handle state
        switch (currentState)
        {
            case StateEnemy.Run:
                MoveToPlayer();
                break;

            case StateEnemy.Attack:
                TryAttack();
                break;
        }
    }

    // STATE CONTROL
    void SetState(StateEnemy newState)
    {
        if (currentState == newState) return;

        currentState = newState;

        switch (currentState)
        {
            case StateEnemy.Idle:
                animator.SetFloat("EnemyMoving_BL", 0);
                break;

            case StateEnemy.Run:
                animator.SetFloat("EnemyMoving_BL", 1);
                break;

            case StateEnemy.Attack:
                break;

            case StateEnemy.Dead:
                animator.SetFloat("EnemyMoving_BL", 0);
                break;
        }
    }

    // MOVEMENT
    void MoveToPlayer()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0;

        // Move
        transform.position += dir * data.moveSpeed * Time.deltaTime;

        // Rotate (smooth)
        if (dir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * 10f
            );
        }
    }

    // Set time Attack
    void TryAttack()
    {
        SetState(StateEnemy.Idle);
        if (Time.time < nextAttackTime) return;

        nextAttackTime = Time.time + data.attackRate;

        Attack();
    }

    void Attack()
    {
        if (data.enemyStyle == EnemyStyle.magic)
        {
            animator.SetBool("IsMelee",false);
            animator.SetTrigger("Attacking");
            Debug.Log("Enemy Attack: " + data.damage);
        }
        else if(data.enemyStyle == EnemyStyle.Melee)
        {
            animator.SetBool("IsMelee", true);
            animator.SetTrigger("Attacking");
        }
    }

    // ======================
    // EXTERNAL (CALL FROM HEALTH)
    // ======================
    public void OnDead()
    {
        SetState(StateEnemy.Dead);
        gameObject.SetActive(false);
    }
}