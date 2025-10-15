using UnityEngine;
using UnityEngine.XR;

public class ClimbingHand : MonoBehaviour
{
    public enum HandType { Left, Right }
    public HandType _hand = HandType.Left;
    public Transform _trackingSpace;
    public bool _debugMode = true;

    [Header("Grab Settings")]
    public string _climbableTag = "Climbable";
    public float _climbStrength = 0.2f;

    private Vector3 _lastPos;
    private bool _isGrabbing = false;
    private bool _canGrab = false;
    private Vector3 _grabPosition;
    private Collider _currentClimbable;

    private InputDevice _controller;

    void Start()
    {
        _lastPos = transform.position;
        InitializeController();
    }

    void InitializeController()
    {
        var handCharacteristic = _hand == HandType.Left ? InputDeviceCharacteristics.Left : InputDeviceCharacteristics.Right;
        var devices = new System.Collections.Generic.List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Controller | handCharacteristic, devices);

        if (devices.Count > 0)
        {
            _controller = devices[0];
            Debug.Log($"{_hand} controller found: {_controller.name}");
        }
    }

    void Update()
    {
        Vector3 move = Vector3.zero;

        bool grabInput = false;

        if (_controller.isValid)
        {
            if (_controller.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerValue))
            {
                grabInput = triggerValue;
            }
        }

        if (_debugMode)
        {
            grabInput = (_hand == HandType.Left) ? Input.GetKey(KeyCode.Q) : Input.GetKey(KeyCode.E);

            if (_hand == HandType.Left)
            {
                if (Input.GetKey(KeyCode.W)) move += transform.forward * 0.02f;
                if (Input.GetKey(KeyCode.S)) move -= transform.forward * 0.02f;
                if (Input.GetKey(KeyCode.A)) move -= transform.right * 0.02f;
                if (Input.GetKey(KeyCode.D)) move += transform.right * 0.02f;
                if (Input.GetKey(KeyCode.G)) move += transform.up * 0.02f; // up
                if (Input.GetKey(KeyCode.H)) move -= transform.up * 0.02f; // down
            }
            else
            {
                if (Input.GetKey(KeyCode.UpArrow)) move += transform.forward * 0.02f;
                if (Input.GetKey(KeyCode.DownArrow)) move -= transform.forward * 0.02f;
                if (Input.GetKey(KeyCode.LeftArrow)) move -= transform.right * 0.02f;
                if (Input.GetKey(KeyCode.RightArrow)) move += transform.right * 0.02f;
                if (Input.GetKey(KeyCode.Period)) move += transform.up * 0.02f; // up
                if (Input.GetKey(KeyCode.Comma)) move -= transform.up * 0.02f; // down
            }

            transform.position += move;
        }

        // Handle grabbing
        if (grabInput && _canGrab && !_isGrabbing)
        {
            _isGrabbing = true;
            _grabPosition = transform.position;
        }
        else if (!grabInput)
        {
            _isGrabbing = false;
        }

        if (_isGrabbing)
        {
            Vector3 delta = transform.position - _lastPos;
            _trackingSpace.position -= delta * _climbStrength;

            transform.position = _grabPosition;
        }

        _lastPos = transform.position;

        // Re-initialize controller if disconnected
        if (!_controller.isValid)
        {
            InitializeController();
        }
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
