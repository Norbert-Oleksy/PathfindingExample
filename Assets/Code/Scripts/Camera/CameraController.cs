using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _cammeraTarget;
    [SerializeField] private CinemachineCamera _cinemaCamera;
    [SerializeField][Range(0f, 100f)] private float _cammeraSpeed;
    [SerializeField][Range(0f, 100f)] private float _zoomSpeed;

    private CinemachineFollow _cinemaFollow;
    private float _zoomStartTime;
    private Vector3 _startingFollowOffset;
    private Vector3 _startingTargetPosition;

    private void Awake()
    {
        _cinemaFollow = _cinemaCamera.GetComponent<CinemachineFollow>();
        _startingFollowOffset = _cinemaFollow.FollowOffset;
        _startingTargetPosition = _cammeraTarget.position;
    }

    private void Update()
    {
        HandlePanning();
        HandleZoom();

        if (Keyboard.current.backspaceKey.wasPressedThisFrame)
        {
            _cinemaFollow.FollowOffset = _startingFollowOffset;
            _cammeraTarget.position = _startingTargetPosition;
        }
    }

    private void HandlePanning()
    {
        Vector2 moveAmmount = Vector2.zero;

        if (Keyboard.current.upArrowKey.isPressed)
        {
            moveAmmount.y += _cammeraSpeed;
        }
        else if (Keyboard.current.downArrowKey.isPressed)
        {
            moveAmmount.y -= _cammeraSpeed;
        }

        if (Keyboard.current.rightArrowKey.isPressed)
        {
            moveAmmount.x += _cammeraSpeed;
        }
        else if (Keyboard.current.leftArrowKey.isPressed)
        {
            moveAmmount.x -= _cammeraSpeed;
        }
        moveAmmount *= Time.deltaTime;
        _cammeraTarget.position += new Vector3(moveAmmount.x, 0, moveAmmount.y);
    }

    private void HandleZoom()
    {
        Vector2 scroll = Mouse.current.scroll.ReadValue();
        if (scroll.y != 0)
        {
            float newY = _cinemaFollow.FollowOffset.y - scroll.y * _zoomSpeed;
            _cinemaFollow.FollowOffset = new Vector3(
                _cinemaFollow.FollowOffset.x,
                Mathf.Clamp(newY, 0, 100),
                _cinemaFollow.FollowOffset.z
            );
        }
    }
}
