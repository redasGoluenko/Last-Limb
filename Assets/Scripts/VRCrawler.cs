using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRDirectInteractor))]
public class ClimbingHand : MonoBehaviour
{
    public Transform _trackingSpace; 
    public string _climbableTag = "Climbable";
    public float _climbStrength = 0.2f;
    public bool IsGrabbing => _isGrabbing;
    private XRDirectInteractor _interactor;
    private Rigidbody _playerRigidbody;
    private bool _isGrabbing = false;
    private Collider _currentClimbable;
    private Vector3 _lastPos;
    private Vector3 _grabPosition;

    void Awake()
    {
        _interactor = GetComponent<XRDirectInteractor>();
        _interactor.selectEntered.AddListener(OnGrabStart);
        _interactor.selectExited.AddListener(OnGrabEnd);

        _playerRigidbody = _trackingSpace.GetComponent<Rigidbody>();
        if (_playerRigidbody == null)
        {
            Debug.LogWarning($"[{name}] No Rigidbody found on trackingSpace!");
        }
        else
        {
            _playerRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            _playerRigidbody.freezeRotation = true;
        }
    }

    void OnDestroy()
    {
        if (_interactor != null)
        {
            _interactor.selectEntered.RemoveListener(OnGrabStart);
            _interactor.selectExited.RemoveListener(OnGrabEnd);
        }
    }

    void FixedUpdate()
    {
        if (_isGrabbing && _playerRigidbody != null)
        {
            Vector3 delta = transform.position - _lastPos;
            Vector3 targetPos = _playerRigidbody.position - delta * _climbStrength;
             _playerRigidbody.MovePosition(Vector3.Lerp(_playerRigidbody.position, targetPos, 0.8f));

            transform.position = _grabPosition;
        }
        _lastPos = transform.position;
    }

    private void OnGrabStart(SelectEnterEventArgs args)
    {
        GameObject grabbedObject = args.interactableObject.transform.gameObject;

        if (grabbedObject.CompareTag(_climbableTag))
        {
            _isGrabbing = true;
            _grabPosition = transform.position;
            _currentClimbable = grabbedObject.GetComponent<Collider>();
        }
    }

    private void OnGrabEnd(SelectExitEventArgs args)
    {
        if (_isGrabbing)
        {
            _isGrabbing = false;
            _currentClimbable = null;
        }
    }
}
