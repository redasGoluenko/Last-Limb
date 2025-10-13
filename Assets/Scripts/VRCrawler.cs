using UnityEngine;

public class ClimbingHand : MonoBehaviour
{
    public enum HandType { Left, Right }
    public HandType _hand = HandType.Left;
    public Transform _trackingSpace;
    public bool _debugMode = true;

    [Header("Grab Settings")]
    public string _climbableTag = "Climbable";
    public float _climbStrength = 0.2f;

    private float _debugMoveSpeed = 0.02f;
    private Vector3 _lastPos;
    private bool _isGrabbing = false;
    private bool _canGrab = false;
    private Vector3 _grabPosition;
    private Collider _currentClimbable;

    void Start()
    {
        _lastPos = transform.position;
    }

    void Update()
    {
        Vector3 move = Vector3.zero;


        // added for testing without VR
        if (_debugMode)
        {
            bool grabInput = (_hand == HandType.Left) ? Input.GetKey(KeyCode.Q) : Input.GetKey(KeyCode.E);

            if (grabInput && _canGrab && !_isGrabbing)
            {
                _isGrabbing = true;
                Debug.Log("Grabbing");
                _grabPosition = transform.position; 
            }
            else
            {
                _isGrabbing = false;
                Debug.Log($"Not Grabbing - {_canGrab}");

            }
            if (_hand == HandType.Left)
            {
                if (Input.GetKey(KeyCode.W)) move += transform.forward * _debugMoveSpeed;
                if (Input.GetKey(KeyCode.S)) move -= transform.forward * _debugMoveSpeed;
                if (Input.GetKey(KeyCode.A)) move -= transform.right * _debugMoveSpeed;
                if (Input.GetKey(KeyCode.D)) move += transform.right * _debugMoveSpeed;
                if (Input.GetKey(KeyCode.G)) move += transform.up * _debugMoveSpeed; // up
                if (Input.GetKey(KeyCode.H)) move -= transform.up * _debugMoveSpeed; // down
            }
            else
            {
                if (Input.GetKey(KeyCode.UpArrow)) move += transform.forward * _debugMoveSpeed;
                if (Input.GetKey(KeyCode.DownArrow)) move -= transform.forward * _debugMoveSpeed;
                if (Input.GetKey(KeyCode.LeftArrow)) move -= transform.right * _debugMoveSpeed;
                if (Input.GetKey(KeyCode.RightArrow)) move += transform.right * _debugMoveSpeed;
                if (Input.GetKey(KeyCode.Period)) move += transform.up * _debugMoveSpeed; // up
                if (Input.GetKey(KeyCode.Comma)) move -= transform.up * _debugMoveSpeed; // down
            }

            transform.position += move;
        }

        if (_isGrabbing)
        {
            Vector3 delta = transform.position - _lastPos;
            _trackingSpace.position -= delta * _climbStrength;

            transform.position = _grabPosition;
        }

        _lastPos = transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_climbableTag))
        {
            _canGrab = true;
            _currentClimbable = other;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other == _currentClimbable)
        {
            _canGrab = false;
            _currentClimbable = null;
        }
    }
}
