using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public float _healthPoints ;
    public float _maxHealth = 100f;

    void Start()
    {
        _healthPoints = _maxHealth;
    }

    /// <summary>
    ///  method to reduce health points to somekind of damage source
    /// </summary>
    /// <param name="damagePoints">amount of points of health entity loses when getting damaged</param>
    public void GetDamaged(float damagePoints)
    {
        _healthPoints -= damagePoints;
        _healthPoints = Mathf.Clamp(_healthPoints, 0, _maxHealth);
    }

    /// <summary>
    /// method to increase health points
    /// </summary>
    /// <param name="healPoints">amount of points of health entity gains health</param>
    public void GetHealed(float healPoints)
    {
        _healthPoints += healPoints;
        _healthPoints = Mathf.Clamp(_healthPoints, 0, _maxHealth);
    }
    /// <summary>
    /// Checks if entity is dead
    /// </summary>
    /// <returns>true if entity is dead</returns>
    public bool isDead()
    {
        return _healthPoints <= 0;        
    }

}
