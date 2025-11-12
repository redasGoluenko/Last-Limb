using UnityEngine;
using UnityEngine.AI;

public class ChasePlayer : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float repathRate = 0.05f; 

    private NavMeshAgent agent;
    private float timer;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = 0.0f;
        agent.autoBraking = false;
        agent.updateRotation = true;  
        agent.updateUpAxis = false;
    }

    void Update()
    {
        if (player == null) return;

        timer += Time.deltaTime;
        if (timer >= repathRate || agent.pathPending || agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(player.position); 
            timer = 0f;
        }
    }
}
