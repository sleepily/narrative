using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

public class PlayerMovement : MonoBehaviour
{
    public GameObject cameraArm;
    public GameObject playerModel;

    public Animator animator;

    public PlayerMovementSettings movementSettings;

    float movementSpeed;

    // Set forward so the player doesn't start laying on the ground
    Vector3 lastMove = Vector3.forward;

    CharacterController controller;
    Vector2 inputAxis;

    private void Start() => GetAllComponents();

    void GetAllComponents()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        DoPlayerMovement();
        UpdateAnimator();
    }

    /*
     * Do all checks and calculations, then move the player
     */
    void DoPlayerMovement()
    {
        bool allowMovement = true;

        // Don't allow movement when player is giving input/in dialogue
        if (GameManager.GLOBAL.player.IsLocked())
            allowMovement = false;

        if (!GameManager.GLOBAL.sceneLoader.HasFinishedLoading())
            allowMovement = false;

        // Don't allow player movement when the inventory is open
        if (GameManager.GLOBAL.inventory.isOpen)
            allowMovement = false;

        if (!allowMovement)
        {
            OverrideAnimator(0f);
            return;
        }

        GetInputAxis();

        // Only proceed further if there is any input
        bool movementInput = inputAxis.magnitude > float.Epsilon;

        // Only apply gravity if there is no input 
        if (!movementInput)
        {
            controller.Move(Physics.gravity * Time.deltaTime);
            movementSpeed = 0f;
            return;
        }

        // Calculate desired move, 
        controller.Move(CalculatePlayerMove());

        // Calculate player rotation and apply
        playerModel.transform.forward = NewPlayerRotation();
    }

    void GetInputAxis()
    {
        inputAxis.x = Input.GetAxisRaw("Horizontal");
        inputAxis.y = Input.GetAxisRaw("Vertical");

        movementSettings.isRunning = Input.GetAxisRaw("Run") > float.Epsilon;
    }

    /*
     * Calculate desired movement and physics and send it to the character controller
     */
    public Vector3 CalculatePlayerMove()
    {
        // Calculate desired movement from input and forward direction
        Vector3 desiredForwardMotion = cameraArm.transform.forward * inputAxis.y;
        Vector3 desiredSidewardMotion = cameraArm.transform.right * inputAxis.x;

        Vector3 desiredMove = desiredForwardMotion + desiredSidewardMotion;

        // Toggle walking/running speed
        movementSpeed = movementSettings.isRunning ? movementSettings.runningSpeed : movementSettings.walkingSpeed;

        // Force movement on XZ plane, introduce movement speed and apply gravity
        desiredMove = Vector3.ProjectOnPlane(desiredMove, Vector3.up).normalized;
        desiredMove *= movementSpeed;
        desiredMove += (Physics.gravity);

        // Update class variable for rotation later on
        lastMove = desiredMove * Time.deltaTime;

        // Return move vector to use with the character controller
        return lastMove;
    }

    /*
     * Lerp the player facing towards the last movement direction
     */
    public Vector3 NewPlayerRotation()
    {
        // Get last movement direction and set Y to 0 to prevent tilt
        Vector3 viewingRotation = lastMove.normalized;
        viewingRotation.y = 0f;

        // Lerp forward vector to prevent sudden value changes
        viewingRotation = Vector3.Lerp
        (
            playerModel.transform.forward,
            viewingRotation,
            Time.deltaTime / movementSettings.rotationSpeed
        );

        return viewingRotation;
    }

    void UpdateAnimator()
    {
        float walkingSpeed = ExtensionMethods.Map01(movementSpeed, 0f, movementSettings.walkingSpeed);
        OverrideAnimator(walkingSpeed);
    }

    void OverrideAnimator(float newWalkingSpeed) => animator.SetFloat("walkingSpeed", newWalkingSpeed);

    public void StopMoving()
    {
        movementSpeed = 0f;
        OverrideAnimator(0f);
    }
}
