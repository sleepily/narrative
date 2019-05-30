using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public GameObject viewingDirection;
    public GameObject playerModel;

    [Range(2.6f, 3.6f)]
    public float movementSpeed = 3.6f;

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
        GetInput();
        Move();
    }

    void GetInput()
    {
        inputAxis.x = Input.GetAxisRaw("Horizontal");
        inputAxis.y = Input.GetAxisRaw("Vertical");
    }

    void Move()
    {
        Vector3 desiredMove = viewingDirection.transform.forward * inputAxis.y + viewingDirection.transform.right * inputAxis.x;
        desiredMove = Vector3.ProjectOnPlane(desiredMove, Vector3.up).normalized;

        Vector3 playerRotation = viewingDirection.transform.eulerAngles;
        playerRotation.x = 0f;
        playerModel.transform.eulerAngles = playerRotation;

        controller.Move(desiredMove * movementSpeed * Time.deltaTime);
    }
}
