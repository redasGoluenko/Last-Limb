using UnityEngine;
using UnityEngine.AI;

[DisallowMultipleComponent]
[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class EnemyController : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] Transform player;

    [Header("Tuning")]
    [SerializeField] float repathSeconds = 0.1f;
    [SerializeField] float attackRange = 1.6f;
    [SerializeField] float attackDamage = 10f;
    [SerializeField] float attackCooldown = 1.0f;
    [SerializeField] string attackStateName = "Attack";
    [SerializeField] float turnRateDegPerSec = 360f;

    private HealthManager _targetHealthManager;
    private HealthManager _enemyHealthManager;

    static readonly int SpeedHash = Animator.StringToHash("Speed");
    static readonly int DistanceHash = Animator.StringToHash("Distance");
    static readonly int AttackHash = Animator.StringToHash("Attack");

    NavMeshAgent agent;
    Animator anim;
    float repathTimer;
    float nextAttackTime;
    bool isAlerted;

    void Awake()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        if (player != null)
            _targetHealthManager = player.GetComponent<HealthManager>();

        _enemyHealthManager = GetComponent<HealthManager>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        agent.updateRotation = false;
        agent.isStopped = true;
        isAlerted = false;
    }

    void Update()
    {
        if (!player) return;

        float dist = Vector3.Distance(transform.position, player.position);
        float speed = agent.velocity.magnitude;

        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        bool inAttackState = stateInfo.IsName(attackStateName);
        bool attackInProgress = inAttackState && stateInfo.normalizedTime < 1f;

        anim.SetFloat(DistanceHash, dist);

        if (!isAlerted)
        {
            anim.SetFloat(SpeedHash, 0f);
            agent.isStopped = true;
            return;
        }

        anim.SetFloat(SpeedHash, speed);

        agent.isStopped = attackInProgress;

        if (!attackInProgress && dist <= attackRange && Time.time >= nextAttackTime)
        {
            anim.ResetTrigger(AttackHash);
            anim.SetTrigger(AttackHash);
            nextAttackTime = Time.time + attackCooldown;
        }

        repathTimer += Time.deltaTime;
        if (!attackInProgress && repathTimer >= repathSeconds)
        {
            agent.SetDestination(player.position);
            repathTimer = 0f;
        }

        Vector3 desired = agent.desiredVelocity;
        if (!attackInProgress && desired.sqrMagnitude > 0.001f)
        {
            Quaternion look = Quaternion.LookRotation(desired.normalized, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                look,
                turnRateDegPerSec * Time.deltaTime
            );
        }
    }

    public void DealDamageToPlayer()
    {
        if (_targetHealthManager == null) return;

        _targetHealthManager.GetDamaged(attackDamage);
        Debug.Log("Player hit for " + attackDamage);
    }

    public void OnPlayerNoise()
    {
        isAlerted = true;
        agent.isStopped = false;
        Debug.Log("Enemy alerted by noise");
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Bullet")) return;

        BulletItem bullet = other.GetComponent<BulletItem>();

        if (bullet is null)
        {
            Debug.Log(other.name + " collided with enemy");
        }

        if (bullet != null && !bullet._reloadable)
        {
            Debug.Log(other.name + " collided with enemy");
            _enemyHealthManager.GetDamaged(bullet._damage);
            Destroy(bullet.gameObject);
        }
    }
}
