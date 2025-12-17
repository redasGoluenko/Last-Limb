using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public float _healthPoints;
    public float _maxHealth = 100f;

    [SerializeField] private AudioSource damageAudio;

    void Start()
    {
        _healthPoints = _maxHealth;
    }

    /// <summary>
    /// Method to reduce health points due to damage
    /// </summary>
    public void GetDamaged(float damagePoints)
    {
        // Play sound only if an AudioSource is assigned
        if (damageAudio != null)
        {
            damageAudio.Play();
        }

        _healthPoints -= damagePoints;
        _healthPoints = Mathf.Clamp(_healthPoints, 0, _maxHealth);
    }

    /// <summary>
    /// Method to increase health points
    /// </summary>
    public void GetHealed(float healPoints)
    {
        _healthPoints += healPoints;
        _healthPoints = Mathf.Clamp(_healthPoints, 0, _maxHealth);
    }

    /// <summary>
    /// Checks if entity is dead
    /// </summary>
    public bool isDead()
    {
        return _healthPoints <= 0;
    }
}
