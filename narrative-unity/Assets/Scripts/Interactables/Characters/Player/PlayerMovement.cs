using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public GameObject cameraArm;
    public GameObject playerModel;

    public bool isRunning = false;

    [SerializeField]
    [Range(1.4f, 2.2f)]
    float walkingSpeed = 1.8f;

    [SerializeField]
    [Range(2.2f, 3.4f)]
    float runningSpeed = 2.8f;

    float movementSpeed = 2.4f;

    [SerializeField]
    [Range(.1f, .5f)]
    float rotationSpeed = .14f;

    Vector3 lastMove = Vector3.forward;

    CharacterController controller;
    CapsuleCollider capsuleCollider;
    Vector2 inputAxis;

    private void Start()
    {
        GetAllComponents();
    }

    void GetAllComponents()
    {
        controller = GetComponent<CharacterController>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        DoPlayerMovement();
    }

    void DoPlayerMovement()
    {
        if (GameManager.GLOBAL.inventoryManager.isOpen)
            return;

        GetInputAxis();

        bool movementInput = inputAxis.magnitude > float.Epsilon;

        if (movementInput)
        {
            Move();
            RotatePlayerModel();
        }
    }

    void GetInputAxis()
    {
        inputAxis.x = Input.GetAxisRaw("Horizontal");
        inputAxis.y = Input.GetAxisRaw("Vertical");

        isRunning = Input.GetAxisRaw("Run") > float.Epsilon;
    }

    void Move()
    {
        Vector3 desiredForwardMotion = cameraArm.transform.forward * inputAxis.y;
        Vector3 desiredSidewardMotion = cameraArm.transform.right * inputAxis.x;

        Vector3 desiredMove = desiredForwardMotion + desiredSidewardMotion;

        /*
         * Toggle walking/running speed
         */
        movementSpeed = isRunning ? runningSpeed : walkingSpeed;

        /*
         * Force movement on XZ plane, introduce movement speed and apply gravity
         * since we are not using SimpleMove()
         */
        desiredMove = Vector3.ProjectOnPlane(desiredMove, Vector3.up).normalized;
        desiredMove *= movementSpeed;
        desiredMove += (Physics.gravity * Time.deltaTime);

        /*
         * Update class variable for rotation later on
         */
        lastMove = desiredMove * Time.deltaTime;

        /*
         * Let the controller handle movement for better collision control
         */
        controller.Move(lastMove);
    }

    void RotatePlayerModel()
    {
        Vector3 viewingRotation = lastMove.normalized;
        viewingRotation.y = 0f;

        playerModel.transform.forward = Vector3.Lerp
        (
            playerModel.transform.forward,
            viewingRotation,
            Time.deltaTime / rotationSpeed
        );
    }
}
