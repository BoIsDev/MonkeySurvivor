using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    public enum StateEnemy { Idle, Run, Attack, Dead }

    [Header("Refs")]
    [SerializeField] private Animator animator;
    [SerializeField] private EnemyDataSO data;
    [SerializeField] private Health health;
    [SerializeField] private EnemyAttack attack;
    private Transform player;
    private StateEnemy currentState;

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        health.OnDied += OnDead;
        SetState(StateEnemy.Idle);
    }

    private void OnDisable() => health.OnDied -= OnDead;

    private void Update()
    {
        if (player == null || currentState == StateEnemy.Dead) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > data.attackRange)
            SetState(StateEnemy.Run);
        else
            SetState(StateEnemy.Attack);

        switch (currentState)
        {
            case StateEnemy.Run:
                MoveToPlayer();
                break;
            case StateEnemy.Attack:
                attack.TryAttack(player);
                break;
        }
    }

    private void SetState(StateEnemy newState)
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
            case StateEnemy.Dead:
                animator.SetFloat("EnemyMoving_BL", 0);
                break;
        }
    }

    private void MoveToPlayer()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0;
        transform.position += dir * data.moveSpeed * Time.deltaTime;

        if (dir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    private void OnDead()
    {
        SetState(StateEnemy.Dead);
        gameObject.SetActive(false);
    }
}
