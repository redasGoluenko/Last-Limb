using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
public class EnemyAnimatorDriver : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float attackRange = 1.6f;
    [SerializeField] private float attackCooldown = 0.9f; 

    private Animator anim;
    private NavMeshAgent agent;
    private float nextAttackTime;

    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int DistanceHash = Animator.StringToHash("Distance");
    private static readonly int AttackHash = Animator.StringToHash("Attack");

    void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false; 
    }

    void Update()
    {
        if (!player) return;

        // Drive the Animator
        float speed = agent.velocity.magnitude;         
        float dist = Vector3.Distance(transform.position, player.position);

        anim.SetFloat(SpeedHash, speed);
        anim.SetFloat(DistanceHash, dist);

        Vector3 desired = agent.desiredVelocity;
        if (desired.sqrMagnitude > 0.001f)
        {
            Quaternion look = Quaternion.LookRotation(desired.normalized, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, look, 360f * Time.deltaTime);
        }

        if (dist <= attackRange && Time.time >= nextAttackTime)
        {

            agent.isStopped = true;
            anim.ResetTrigger(AttackHash); 
            anim.SetTrigger(AttackHash);
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    public void ResumeAgent() => agent.isStopped = false;
}
