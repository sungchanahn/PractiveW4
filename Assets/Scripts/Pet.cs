using UnityEngine;
using UnityEngine.AI;

public enum PetState
{
    Idle,
    Wander,
    Follow,
    Attack
}

public class Pet : MonoBehaviour, IDamagable
{
    private Animator animator;
    private NavMeshAgent agent;
    public PetState petState;

    public int health;
    public float walkSpeed;
    public float runSpeed;

    public float followRange;
    public float detectRange;
    public float fieldOfView = 120f;

    [Header("Wandering")]
    public float minWanderDistance;
    public float maxWanderDistance;
    public float minWanderWaitTime;
    public float maxWanderWaitTime;

    [Header("Combat")]
    public float attackRange;
    public int damage;
    public float attackRate;
    private float lastAttackTime;

    private float playerDistance;
    private float enemyDistance;
    public GameObject enemy;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        playerDistance = Vector3.Distance(CharacterManager.Instance.Player.transform.position, transform.position);
        if (enemy != null) enemyDistance = Vector3.Distance(enemy.transform.position, transform.position);
        animator.SetBool("Moving", petState != PetState.Idle);

        switch (petState)
        {
            case PetState.Idle:
            case PetState.Wander:
                PassiveUpdate();
                break;
            case PetState.Follow:
                FollowUpdate();
                break;
            case PetState.Attack:
                AttackUpdate();
                break;
        }
    }

    public void SetState(PetState state)
    {
        petState = state;

        switch (petState)
        {
            case PetState.Idle:
                agent.speed = walkSpeed;
                agent.isStopped = true;
                break;
            case PetState.Wander:
                agent.speed = walkSpeed;
                agent.isStopped = false;
                break;
            case PetState.Follow:
            case PetState.Attack:
                agent.speed = runSpeed;
                agent.isStopped = false;
                break;
        }

        animator.speed = agent.speed / walkSpeed;
    }

    private void PassiveUpdate()
    {
        if (petState == PetState.Wander && agent.remainingDistance < 0.1f)
        {
            SetState(PetState.Idle);
            Invoke("WanderToNewLocation", Random.Range(minWanderWaitTime, maxWanderWaitTime));
        }

        if (playerDistance > followRange)
        {
            SetState(PetState.Follow);
        }

        if (enemyDistance < detectRange)
        {
            SetState(PetState.Attack);
        }
    }

    private void FollowUpdate()
    {
        if (CanFollow())
        {
            agent.SetDestination(CharacterManager.Instance.Player.transform.position + Vector3.left);
        }
        else
        {
            transform.position = CharacterManager.Instance.Player.transform.position + Vector3.left;
        }

        if (playerDistance < followRange * 0.2f)
        {
            agent.SetDestination(transform.position);
            agent.isStopped = true;
            SetState(PetState.Wander);
        }

        if (enemyDistance < detectRange)
        {
            SetState(PetState.Attack);
        }
    }

    private void AttackUpdate()
    {
        if (enemy == null)
        {
            agent.SetDestination(transform.position);
            agent.isStopped = true;
            SetState(PetState.Wander);
            return;
        }

        if (enemyDistance < attackRange && IsEnemyInFieldOfView())
        {
            agent.isStopped = true;
            if (Time.time - lastAttackTime > attackRate)
            {
                lastAttackTime = Time.time;
                enemy.GetComponent<IDamagable>().TakePhysicalDamage(damage);
                animator.speed = 1;
                animator.SetTrigger("Attack");
            }
        }
        else
        {
            if (enemyDistance < detectRange)
            {
                agent.isStopped = false;
                NavMeshPath path = new NavMeshPath();
                if (agent.CalculatePath(enemy.transform.position, path))
                {
                    agent.SetDestination(enemy.transform.position);
                }
                else
                {
                    agent.SetDestination(transform.position);
                    agent.isStopped = true;
                    SetState(PetState.Wander);
                }
            }
            else
            {
                agent.SetDestination(transform.position);
                agent.isStopped = true;
                SetState(PetState.Wander);
            }
        }
    }

    private bool IsEnemyInFieldOfView()
    {
        Vector3 directionToEnemy = enemy.transform.position - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToEnemy);
        return angle < fieldOfView * 0.5;
    }

    private void WanderToNewLocation()
    {
        if (petState != PetState.Idle) return;

        SetState(PetState.Wander);
        agent.SetDestination(GetWanderLocation());
    }

    private Vector3 GetWanderLocation()
    {
        NavMeshHit hit;

        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)),
                                out hit, maxWanderDistance, NavMesh.AllAreas);

        int i = 0;

        while (Vector3.Distance(transform.position, hit.position) < detectRange)
        {
            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)),
                                out hit, maxWanderDistance, NavMesh.AllAreas);
            i++;
            if (i == 30) break;
        }

        return hit.position;
    }

    private bool CanFollow()
    {
        NavMeshPath path = new NavMeshPath();
        return agent.CalculatePath(CharacterManager.Instance.Player.transform.position, path);
    }

    public void TakePhysicalDamage(int _damage)
    {
        health -= _damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
