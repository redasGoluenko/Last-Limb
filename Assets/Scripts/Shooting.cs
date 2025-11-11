using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Shooting : MonoBehaviour
{
    public Transform _shootingPosition;
    public GameObject _bulletGameObject;
    public float _shootingSpeed = 25f;
    public AudioSource _audioSource;
    private InputDevice _leftController;
    private InputDevice _rightController;
    private bool _buttonWasPressed = false;
    private bool _isHeld = false;
    private XRGrabInteractable grabInteractable;
    public ParticleSystem _flash;
    void Start()
    {
        InitializeControllers();
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void InitializeControllers()
    {
        _leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        _rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        if (!_leftController.isValid)
            Debug.LogWarning("No left controller found.");
        if (!_rightController.isValid)
            Debug.LogWarning("No right controller found.");
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

        if (_isHeld && buttonPressed && !_buttonWasPressed)
        {
            Shoot();
        }
        _buttonWasPressed = buttonPressed;
    }

    private void Shoot()
    {
        _audioSource.Play();
        if (_bulletGameObject == null || _shootingPosition == null)
        {
            Debug.LogWarning("Missing bullet prefab or shooting position.");
            return;
        }

        GameObject bullet = Instantiate(_bulletGameObject, _shootingPosition.position, _shootingPosition.rotation);
        _flash.Play();
        // Give bullet forward velocity along local Z-axis
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = _shootingPosition.forward * _shootingSpeed;
        }

        Destroy(bullet, 5f);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        _isHeld = true;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        _isHeld = false;
    }
}
