using UnityEngine;

public class PlayerNoiseEmitter : MonoBehaviour
{
    [Header("Movement → Noise")]
    [SerializeField] float minSpeedForNoise = 0.05f;
    [SerializeField] float maxSpeedForFullNoise = 2f;
    [SerializeField] float maxNoiseRadius = 15f;

    [Header("Emission")]
    [SerializeField] float emitInterval = 0.1f;
    [SerializeField] LayerMask enemyLayerMask;

    Vector3 _lastPosition;
    float _emitTimer;

    void Start()
    {
        _lastPosition = transform.position;
    }

    void Update()
    {
        float dt = Time.deltaTime;
        if (dt <= 0f) return;

        Vector3 delta = transform.position - _lastPosition;
        float speed = delta.magnitude / dt;

        _lastPosition = transform.position;
        _emitTimer += dt;

        if (_emitTimer < emitInterval) return;

        if (speed < minSpeedForNoise)
        {
            _emitTimer = 0f;
            return;
        }

        float t = Mathf.InverseLerp(minSpeedForNoise, maxSpeedForFullNoise, speed);
        float noiseRadius = Mathf.Lerp(0f, maxNoiseRadius, t);

        EmitNoise(transform.position, noiseRadius);

        _emitTimer = 0f;
    }

    void EmitNoise(Vector3 position, float radius)
    {
        Collider[] hits = Physics.OverlapSphere(position, radius, enemyLayerMask);
        foreach (var hit in hits)
        {
            var hearing = hit.GetComponent<EnemyHearing>();
            if (hearing != null)
            {
                hearing.OnHeardNoise(position, radius);
            }
        }
    }
}
