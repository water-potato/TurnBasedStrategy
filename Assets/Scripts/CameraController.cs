using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;


    // 움직임 관련
    float moveSpeed = 10f;

    // 회전 관련
    float rotationSpeed = 100f;
    // 줌 관련
    CinemachineTransposer transposer;
    Vector3 camFollowOffset;
    const float ZOOM_SPEED = 20f;
    const float MIN_FOLLOW_Y_OFFSET = 2f;
    const float MAX_FOLLOW_Y_OFFSET = 12f;



    private void Start()
    {
        transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();

        if (virtualCamera == null) Debug.Log("virtual Camera null");
        if(transposer == null)
        {
            Debug.Log("transposer null");
        }
    }
    void Update()
    {
        Vector3 inputMoveDir = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDir.z = +1f;
        }
        if(Input.GetKey(KeyCode.S))
        {
            inputMoveDir.z = -1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDir.x = -1f;
        }
        if ( Input.GetKey(KeyCode.D))
        {
            inputMoveDir.x = +1f;
        }



        Vector3 moveVector = transform.forward * inputMoveDir.z + transform.right * inputMoveDir.x;
        transform.position += moveVector * moveSpeed * Time.deltaTime;


        Vector3 rotationVector = Vector3.zero;

        if (Input.GetKey(KeyCode.Q))
        {
            rotationVector.y = +1f;
        }
        if(Input.GetKey(KeyCode.E))
        {
            rotationVector.y = -1f;
        }

        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;



        Vector3 camFollowOffset = transposer.m_FollowOffset;


        if (Input.mouseScrollDelta.y > 0)
        {
            camFollowOffset.y -= 1f;
        }
        if(Input.mouseScrollDelta.y < 0)
        {
            camFollowOffset.y += 1f;
        }
        camFollowOffset.y = Mathf.Clamp(camFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);
        transposer.m_FollowOffset.y = Mathf.Lerp(transposer.m_FollowOffset.y , camFollowOffset.y , Time.deltaTime * ZOOM_SPEED);

    }
}
