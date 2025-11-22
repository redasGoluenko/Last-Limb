using UnityEngine;
using UnityEngine.AI;

[DisallowMultipleComponent]
[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class EnemyController : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] GameObject player;

    [Header("Tuning")]
    [SerializeField] float repathSeconds = 0.1f;   
    [SerializeField] float attackRange = 1.6f;    
    [SerializeField] float attackDamage = 25f;
    [SerializeField] string attackStateName = "Attack"; 
    [SerializeField] float turnRateDegPerSec = 360f;
    private HealthManager _targetHealthManager; // player
    private HealthManager _enemyHealthManager;

    // Animator parameters
    static readonly int SpeedHash = Animator.StringToHash("Speed");
    static readonly int DistanceHash = Animator.StringToHash("Distance");
    static readonly int AttackHash = Animator.StringToHash("Attack");

    NavMeshAgent agent;
    Animator anim;
    float repathTimer;

    void Awake()
    {
        _targetHealthManager = player.GetComponent<HealthManager>();
        _enemyHealthManager = GetComponent < HealthManager>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        agent.updateRotation = false;

        agent.isStopped = false;
    }

    void Update()
    {
        if (!player) return;

        float dist = Vector3.Distance(transform.position, player.transform.position);
        float speed = agent.velocity.magnitude;

        anim.SetFloat(SpeedHash, speed);
        anim.SetFloat(DistanceHash, dist);

        bool isAttacking = anim.GetCurrentAnimatorStateInfo(0).IsName(attackStateName);

        if (!isAttacking && dist <= attackRange)
        {
            anim.ResetTrigger(AttackHash);
            anim.SetTrigger(AttackHash);
            _targetHealthManager.GetDamaged(attackDamage);
        }

        agent.isStopped = isAttacking;

        repathTimer += Time.deltaTime;
        if (!isAttacking && repathTimer >= repathSeconds)
        {
            agent.SetDestination(player.transform.position);
            repathTimer = 0f;
        }

        Vector3 desired = agent.desiredVelocity;
        if (!isAttacking && desired.sqrMagnitude > 0.001f)
        {
            Quaternion look = Quaternion.LookRotation(desired.normalized, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, look, turnRateDegPerSec * Time.deltaTime);
        }
        if(_enemyHealthManager.isDead()){
            Destroy(this);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        BulletItem bullet = other.GetComponent<BulletItem>();

        if (bullet != null && !bullet._reloadable)
        {
            _enemyHealthManager.GetDamaged(bullet._damage);
            Destroy(bullet);
        }
    }
}
