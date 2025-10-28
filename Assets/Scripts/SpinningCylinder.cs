using UnityEngine;
using UnityEngine.XR;

public class SpinningCylinder : MonoBehaviour
{
    public Transform _cylinder;
    public float _spinSpeed = 240f;   // degrees per second
    public float _spinAmount = 60f;  // degrees per spin

    private bool _spinning = false;
    private Quaternion _targetRotation;
    private InputDevice _leftController;
    private InputDevice _rightController;
    private bool _buttonWasPressed = false;

    void Start()
    {
        InitializeControllers();
    }

    void InitializeControllers()
    {
        _leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        _rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        if (!_leftController.isValid)
            Debug.LogWarning("No left controller found!");
        if (!_rightController.isValid)
            Debug.LogWarning("No right controller found!");
    }

    void Update()
    {
        if (!_leftController.isValid || !_rightController.isValid)
            InitializeControllers();

        bool leftPressed = false;
        bool rightPressed = false;

        if (_leftController.isValid)
            _leftController.TryGetFeatureValue(CommonUsages.triggerButton, out leftPressed);

        if (_rightController.isValid)
            _rightController.TryGetFeatureValue(CommonUsages.triggerButton, out rightPressed);

        bool buttonPressed = leftPressed || rightPressed;

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
