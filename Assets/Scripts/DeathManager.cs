using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathManager : MonoBehaviour
{
    private HealthManager _healthManager;
    private string _deathSceneName = "Death scene";


    void Start()
    {
        _healthManager = GetComponent<HealthManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_healthManager != null && _healthManager.isDead())
        {
            LoadDeathScene();
        }
    }
    private void LoadDeathScene()
    {
        SceneManager.LoadScene(_deathSceneName);
    }
}
