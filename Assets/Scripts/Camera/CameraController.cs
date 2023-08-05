using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;


    // 움직임 관련
    float moveSpeed = 10f;

    // 줌 관련
    CinemachineTransposer transposer;
    Vector3 camFollowOffset;
    const float MIN_FOLLOW_Y_OFFSET = 2f;
    const float MAX_FOLLOW_Y_OFFSET = 12f;



    private void Start()
    {
        transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();

        camFollowOffset = transposer.m_FollowOffset;
        if (virtualCamera == null) Debug.Log("virtual Camera null");
        if (transposer == null)
        {
            Debug.Log("transposer null");
        }
    }
    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleZoom();




    }


    private void HandleMovement()
    {
        Vector2 inputMoveDir = InputManager.Instance.GetCameraMoveVector();

        Vector3 moveVector = transform.forward * inputMoveDir.y + transform.right * inputMoveDir.x;
        transform.position += moveVector * moveSpeed * Time.deltaTime;

    }
    private void HandleRotation()
    {
        Vector3 rotationVector = Vector3.zero;

        rotationVector.y = InputManager.Instance.GetCameraRotateAmount();
        float rotationSpeed = 100f;
        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;
    }
    private void HandleZoom()
    {
        float zoomIncreaseAmount = 1f;
        camFollowOffset.y += InputManager.Instance.GetCameraZoomAmount() * zoomIncreaseAmount;
        camFollowOffset.y = Mathf.Clamp(camFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);


        const float ZOOM_SPEED = 1f;

        transposer.m_FollowOffset = Vector3.Lerp(transposer.m_FollowOffset, camFollowOffset, Time.deltaTime * ZOOM_SPEED);
    }
}
