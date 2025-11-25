using UnityEngine;

public class KeyController : MonoBehaviour
{
    [Header("Keys to Manage")]
    public GameObject[] keys; // Keys that appear one by one

    [Header("Objects to Toggle After All Keys Are Visible")]
    public GameObject[] objectsToDeactivate; // Deactivate after all keys are visible

    public TeleportOnPlayerCollision teleporter; // Assign the teleporter in inspector

    void Start()
    {
        // Make all keys invisible initially
        foreach (GameObject key in keys)
        {
            key.SetActive(false);
        }

        // Ensure teleporter is initially inactive
        if (teleporter != null)
            teleporter.active = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object has the "Key" tag
        if (other.CompareTag("Key"))
        {
            Destroy(other.gameObject); // Destroy the key that collided

            // Make the first invisible key visible
            for (int i = 0; i < keys.Length; i++)
            {
                if (!keys[i].activeSelf)
                {
                    keys[i].SetActive(true);
                    break; // Only show one key at a time
                }
            }

            // Check if all keys are now visible
            bool allVisible = true; // Assume all are visible
            foreach (GameObject key in keys)
            {
                if (!key.activeSelf)
                {
                    allVisible = false; // Found one key still invisible
                    break;
                }
            }

            // If all keys are visible, activate teleporter and toggle objects
            if (allVisible)
            {
                if (teleporter != null)
                    teleporter.active = true;

                // Deactivate objects
                foreach (GameObject obj in objectsToDeactivate)
                {
                    obj.SetActive(false);
                }
            }
        }
    }
}
