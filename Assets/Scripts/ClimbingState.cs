using UnityEngine;

public class ClimbingState : MonoBehaviour
{
    public ClimbingHand leftHand;
    public ClimbingHand rightHand;
    public Rigidbody playerRigidbody;

    public bool IsClimbing { get; private set; }

    void Update()
    {
        if (leftHand == null || rightHand == null || playerRigidbody == null)
        {
            IsClimbing = false;
            if (playerRigidbody != null)
                playerRigidbody.useGravity = true;
            return;
        }

        // Player is climbing if at least one hand is grabbing
        IsClimbing = leftHand.IsGrabbing || rightHand.IsGrabbing;

        // Only enable gravity when neither hand is grabbing
        playerRigidbody.useGravity = !IsClimbing;
    }
}
