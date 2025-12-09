using UnityEngine;

public class SpawnOnBulletHit : MonoBehaviour
{
    [Header("Prefab to Spawn")]
    public GameObject objectToSpawn;

    [Header("Optional Spawn Offset")]
    public Vector3 spawnOffset = Vector3.zero;

    private bool hasSpawned = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (hasSpawned) return;

        // Check if the object that hit this has the "Bullet" tag
        if (collision.gameObject.CompareTag("Bullet"))
        {
            SpawnObject();
        }
    }

    private void SpawnObject()
    {
        if (objectToSpawn == null)
        {
            Debug.LogWarning("No prefab assigned to SpawnOnBulletHit!");
            return;
        }

        Vector3 spawnPosition = transform.position + spawnOffset;

        GameObject spawned = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
     
        hasSpawned = true;

        // Destroy the box (this object)
        Destroy(gameObject);
    }
}
