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

    // Set forward so the player doesn't start laying on the ground
    Vector3 lastMove = Vector3.forward;

    CharacterController controller;
    Vector2 inputAxis;

    private void Start()
    {
        GetAllComponents();
    }

    void GetAllComponents()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        DoPlayerMovement();
    }

    /*
     * Do all checks and calculations to move the player
     */
    void DoPlayerMovement()
    {
        // Don't allow player movement when the inventory is open
        if (GameManager.GLOBAL.inventoryManager.isOpen)
            return;

        GetInputAxis();

        // Only proceed further if there is any input
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

    /*
     * Calculate desired movement and physics and send it to the character controller
     */
    void Move()
    {
        // Calculate desired movement from input and forward direction
        Vector3 desiredForwardMotion = cameraArm.transform.forward * inputAxis.y;
        Vector3 desiredSidewardMotion = cameraArm.transform.right * inputAxis.x;

        Vector3 desiredMove = desiredForwardMotion + desiredSidewardMotion;

        // Toggle walking/running speed
        movementSpeed = isRunning ? runningSpeed : walkingSpeed;

        // Force movement on XZ plane, introduce movement speed and apply gravity
        desiredMove = Vector3.ProjectOnPlane(desiredMove, Vector3.up).normalized;
        desiredMove *= movementSpeed;
        desiredMove += (Physics.gravity * Time.deltaTime);

        // Update class variable for rotation later on
        lastMove = desiredMove * Time.deltaTime;

        // Let the controller handle movement for better collision control
        controller.Move(lastMove);
    }

    /*
     * Lerp the player facing towards the last movement direction
     */
    void RotatePlayerModel()
    {
        // Get last mmovement direction and set Y to 0 to prevent tilt
        Vector3 viewingRotation = lastMove.normalized;
        viewingRotation.y = 0f;

        // Lerp forward vector to prevent sudden value changes
        playerModel.transform.forward = Vector3.Lerp
        (
            playerModel.transform.forward,
            viewingRotation,
            Time.deltaTime / rotationSpeed
        );
    }
}
