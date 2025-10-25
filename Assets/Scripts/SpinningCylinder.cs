using UnityEngine;
using UnityEngine.XR;

public class SpinningCylinder : MonoBehaviour
{
    public Transform _cylinder;
    public float _spinSpeed = 60f;    // degrees per second
    public float _spinAmount = 60f;   // degrees per spin
    public XRNode controllerNode = XRNode.RightHand; // which hand triggers spin

    private bool _spinning = false;
    private Quaternion _targetRotation;
    private InputDevice _controller;
    private bool _buttonWasPressed = false;

    void Start()
    {
        InitializeController();
    }

    void InitializeController()
    {
        _controller = InputDevices.GetDeviceAtXRNode(controllerNode);
        if (!_controller.isValid)
        {
            Debug.LogWarning($"No controller found for {controllerNode}");
        }
    }

    void Update()
    {
        if (!_controller.isValid)
            InitializeController();

        bool buttonPressed = false;

        if (_controller.isValid)
        {
            _controller.TryGetFeatureValue(CommonUsages.gripButton, out buttonPressed);
        }

        if (buttonPressed && !_buttonWasPressed)
        {
            StartSpin();
        }
        _buttonWasPressed = buttonPressed;

        if (_spinning && _cylinder != null)
        {
            _cylinder.localRotation = Quaternion.RotateTowards(
                _cylinder.localRotation,
                _targetRotation,
                _spinSpeed * Time.deltaTime
            );

            if (Quaternion.Angle(_cylinder.localRotation, _targetRotation) < 0.1f)
            {
                _spinning = false;
                _cylinder.localRotation = _targetRotation;
            }
        }
    }

    public void StartSpin()
    {
        if (_cylinder == null) return;
        // cylinder local z
        _targetRotation = _cylinder.localRotation * Quaternion.Euler(0, 0, _spinAmount);
        _spinning = true;
        Debug.Log("Cylinder spinning!");
    }
}
