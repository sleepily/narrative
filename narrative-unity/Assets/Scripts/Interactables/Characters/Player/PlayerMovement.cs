using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public GameObject cameraArm;
    public GameObject playerModel;

    [SerializeField]
    [Range(2.6f, 3.6f)]
    float movementSpeed = 3.6f;

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
        GetInputAxis();
        Move();
        RotatePlayerModel();
    }

    void GetInputAxis()
    {
        inputAxis.x = Input.GetAxisRaw("Horizontal");
        inputAxis.y = Input.GetAxisRaw("Vertical");
    }

    void Move()
    {
        Vector3 desiredForwardMotion = cameraArm.transform.forward * inputAxis.y;
        Vector3 desiredSidewardMotion = cameraArm.transform.right * inputAxis.x;

        Vector3 desiredMove = desiredForwardMotion + desiredSidewardMotion;

        /*
         * Force movement on XZ plane, introduce movement speed and apply gravity
         * since we are not using SimpleMove()
         */
        desiredMove = Vector3.ProjectOnPlane(desiredMove, Vector3.up).normalized;
        desiredMove *= movementSpeed;
        desiredMove += (Physics.gravity * Time.deltaTime);

        /*
         * Let the controller handle movement for better collision control
         */
        controller.Move(desiredMove * Time.deltaTime);
    }

    void RotatePlayerModel()
    {
        Vector3 viewingRotation = cameraArm.transform.eulerAngles;
        Vector3 playerRotation = Vector3.up * viewingRotation.y; // prevent player tilt (Y only)
        playerModel.transform.eulerAngles = playerRotation;
    }
}
