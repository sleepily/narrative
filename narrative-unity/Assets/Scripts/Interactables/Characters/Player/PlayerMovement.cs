using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public GameObject viewingDirection;
    public GameObject playerModel;

    [SerializeField]
    [Range(2.6f, 3.6f)]
    float movementSpeed = 3.6f;

    [SerializeField]
    [Range(0f, .2f)]
    float groundCheckMargin = .1f;
    bool isGrounded = false;

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
        GetInput();
        GroundCheck();
        Move();
        RotatePlayerModel();
    }

    void GetInput()
    {
        inputAxis.x = Input.GetAxisRaw("Horizontal");
        inputAxis.y = Input.GetAxisRaw("Vertical");
    }

    void GroundCheck()
    {
        Ray groundCheckRay = new Ray(capsuleCollider.center, Vector3.down);
        float checkDistance = (capsuleCollider.height / 2) + groundCheckMargin;

        isGrounded = (capsuleCollider.Raycast(groundCheckRay, out RaycastHit hitInfo, checkDistance));
    }

    void Move()
    {
        Vector3 desiredForwardMotion = viewingDirection.transform.forward * inputAxis.y;
        Vector3 desiredSidewardMotion = viewingDirection.transform.right * inputAxis.x;

        Vector3 desiredMove = desiredForwardMotion + desiredSidewardMotion;

        desiredMove = Vector3.ProjectOnPlane(desiredMove, Vector3.up).normalized;
        
        desiredMove += (Physics.gravity * Time.deltaTime);

        controller.Move(desiredMove * movementSpeed * Time.deltaTime);
    }

    void RotatePlayerModel()
    {
        Vector3 playerRotation = viewingDirection.transform.eulerAngles;
        playerRotation.x = 0f; // lock X rotation so the player doesn't tilt forward
        playerModel.transform.eulerAngles = playerRotation;
    }
}
