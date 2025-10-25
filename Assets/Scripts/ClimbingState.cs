using UnityEngine;

public class ClimbingState : MonoBehaviour
{
    public ClimbingHand _leftHand;
    public ClimbingHand _rightHand;
    public Rigidbody _playerRigidbody;

    public bool IsClimbing { get; private set; }

    void Update()
    {
        if (_leftHand == null || _rightHand == null || _playerRigidbody == null)
        {
            IsClimbing = false;
            if (_playerRigidbody != null)
                _playerRigidbody.useGravity = true;
            return;
        }

        IsClimbing = _leftHand.IsGrabbing || _rightHand.IsGrabbing;

        _playerRigidbody.useGravity = !IsClimbing;
    }
}
