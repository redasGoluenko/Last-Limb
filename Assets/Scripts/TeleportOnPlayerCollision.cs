using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportOnPlayerCollision : MonoBehaviour
{
    [Header("Name of the scene to load")]
    public string sceneToLoad;

    [Header("Check to lock teleportation")]
    public bool locked = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") && !locked)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
