using UnityEngine;

public class ToggleVisibility : MonoBehaviour
{
    [Header("Check to make object visible, uncheck to hide")]
    public bool isVisible = true;

    private Renderer[] renderers;

    private void Awake()
    {
        // Get all renderers on this object and children (optional)
        renderers = GetComponentsInChildren<Renderer>();
        ApplyVisibility();
    }

    private void OnValidate()
    {
        // Update visibility in real time when changing in inspector
        if (renderers == null)
            renderers = GetComponentsInChildren<Renderer>();

        ApplyVisibility();
    }

    private void ApplyVisibility()
    {
        foreach (Renderer r in renderers)
        {
            r.enabled = isVisible;
        }
    }
}
