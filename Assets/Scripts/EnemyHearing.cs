using UnityEngine;

public class EnemyHearing : MonoBehaviour
{
    [SerializeField] float hearingSensitivity = 1f;
    [SerializeField] float memoryTime = 3f;

    EnemyController _controller;
    float _lastHeardTime;

    void Awake()
    {
        _controller = GetComponent<EnemyController>();
    }

    public void OnHeardNoise(Vector3 noisePosition, float noiseRadius)
    {
        float dist = Vector3.Distance(transform.position, noisePosition);
        if (dist > noiseRadius * hearingSensitivity) return;

        _lastHeardTime = Time.time;

        if (_controller != null)
            _controller.OnPlayerNoise();
    }

    public bool HasRecentNoise()
    {
        return Time.time - _lastHeardTime <= memoryTime;
    }
}
