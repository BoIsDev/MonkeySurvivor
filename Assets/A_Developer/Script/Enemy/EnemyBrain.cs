using System;
using System.Collections;
using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    public enum StateEnemy { Run, Idle, Attack, Damaged, Dead }

    [Header("Refs")]
    [SerializeField] private Animator animator;
    [SerializeField] private EnemyDataSO data;
    [SerializeField] private Health health;
    private EnemyAttackBase attack;
    private Transform player;
    private PlayerStats playerStats;
    private StateEnemy currentState;

    private GameObject _sourcePrefab;
    private Action _onDeactivated;

    public void InitSpawner(GameObject sourcePrefab, Action onDeactivated)
    {
        _sourcePrefab = sourcePrefab;
        _onDeactivated = onDeactivated;
    }

    private void OnEnable()
    {
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        player = playerObj?.transform;
        playerStats = playerObj?.GetComponent<PlayerStats>();

        attack = GetComponent<EnemyAttackBase>();
        attack?.Init(data);
        health.OnDied += OnDead;
        health.OnDamageTaken += OnDamaged;

        currentState = StateEnemy.Dead; // force reset 
        SetState(StateEnemy.Run);
    }

    private void OnDisable()
    {
        health.OnDied -= OnDead;
        health.OnDamageTaken -= OnDamaged;
    }

    private void Update()
    {
        if (player == null || currentState == StateEnemy.Dead || currentState == StateEnemy.Damaged) return;

        bool isExecuting = attack != null && attack.IsExecuting;

        // Luôn nhìn về player trừ khi đang charge (Hulk đã lock hướng rồi)
        if (!isExecuting) FacePlayer();

        // Đang trong attack → không check state, không move, chờ xong mới re-evaluate
        if (isExecuting) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > data.attackRange)
            SetState(StateEnemy.Run);
        else if (attack != null && attack.IsExecuting)
            SetState(StateEnemy.Attack);
        else
            SetState(StateEnemy.Idle);

        switch (currentState)
        {
            case StateEnemy.Run:
                MoveToPlayer();
                break;
            case StateEnemy.Idle:
                bool fired = attack != null && attack.TryAttack(player);
                if (fired) animator.SetTrigger("Attacking");
                break;
            case StateEnemy.Attack:
                break;
        }
    }

    private void SetState(StateEnemy newState)
    {
        if (currentState == newState) return;
        currentState = newState;

        switch (currentState)
        {
            case StateEnemy.Run:
                animator.SetBool("isWalking", true);
                break;
            case StateEnemy.Idle:
                animator.SetBool("isWalking", false);
                break;
            case StateEnemy.Attack:
                animator.SetBool("isWalking", false);
                break;
            case StateEnemy.Damaged:
                animator.SetTrigger("Damaging");
                break;
            case StateEnemy.Dead:
                animator.SetBool("isDied", true);
                break;
        }
    }

    private void MoveToPlayer()
    {
        if (attack != null && attack.IsExecuting) return;
        MoveTo(player.position, data.moveSpeed);
    }

    // EnemyBrain làm chủ toàn bộ movement — attack scripts gọi vào đây thay vì tự move transform
    public void MoveTo(Vector3 target, float speed)
    {
        Vector3 flatTarget = new Vector3(target.x, transform.position.y, target.z);
        transform.position = Vector3.MoveTowards(transform.position, flatTarget, speed * Time.deltaTime);
    }

    // Rotation tách riêng — Update() quyết định khi nào gọi
    private void FacePlayer()
    {
        Vector3 dir = (player.position - transform.position);
        dir.y = 0;
        if (dir.sqrMagnitude < 0.001f) return;

        Quaternion targetRotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
    }

    private void OnDamaged(float amount)
    {
        if (currentState == StateEnemy.Dead) return;
        attack?.Cancel(); // dừng mọi coroutine attack đang chạy
        SetState(StateEnemy.Damaged);
        StartCoroutine(DamagedRoutine());
    }

    private IEnumerator DamagedRoutine()
    {
        yield return null;
        float duration = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(duration);
        SetState(StateEnemy.Run);
    }

    private void OnDead()
    {
        attack?.Cancel();
        playerStats?.AddExp(data.expReward);
        SetState(StateEnemy.Dead);
        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        yield return null;
        float duration = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(duration);

        _onDeactivated?.Invoke();
        _onDeactivated = null;

        if (_sourcePrefab != null)
            PoolManager.Instance.Despawn(_sourcePrefab, gameObject);
        else
            gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        if (data == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, data.attackRange);
    }
}
