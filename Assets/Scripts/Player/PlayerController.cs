using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float movespeed;
    public float jumpPower;
    private Vector2 curMovementInput;
    public LayerMask groundmask;

    

    [Header("Look")]
    public Transform cameraContainer;
    public float minLook;
    public float maxLook;
    private float camCurXRot;
    public float lookSensitity; //민감도 
    private Vector2 MouseDeltea;
    public bool canLook = true;

    public Action inventory;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        Move();
    }
    private void LateUpdate()
    {
        if (canLook)
        {
            CamaraLook();
        }
        
    }
    private void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x; 
        dir *= movespeed; 
        dir.y = rb.velocity.y; // 이 존재하지 않음 , 스페이스(점프) 누를 경우 위아래로 이동 ,위아래 이동하기 전의 값을 넣어준다
        rb.velocity = dir; // 
    }

    void CamaraLook()
    {
        camCurXRot += MouseDeltea.y * lookSensitity;
        camCurXRot = Mathf.Clamp(camCurXRot, minLook, maxLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);
      
        transform.eulerAngles += new Vector3(0, MouseDeltea.x * lookSensitity, 0);// 카메라의 위치변경 , 컨테이너의값 변경  
        //transform.eulerAngles += new Vector3(-(MouseDeltea.y * lookSensitity), MouseDeltea.x * lookSensitity, 0);
    }


    public void Onmove(InputAction.CallbackContext context)
    {

        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if(context.phase ==InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        MouseDeltea =context.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context)

    {
        if(context.phase == InputActionPhase.Started && IsGrounded())
        {
            rb.AddForce(Vector2.up * jumpPower,ForceMode.Impulse);
        }
    }

    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        for(int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i],0.1f, groundmask))
            {
                return true;
            }
        }

        return false;
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCusor();
        }
    }

    void ToggleCusor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle; 
    }
}
