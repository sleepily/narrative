using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_Globe : Interactable
{
    [SerializeField]
    float angleIncrement = 36f;

    [SerializeField]
    float rotationSpeed = 1.4f;

    float currentRotationAngle = 0f;
    float desiredRotationAngle = 0f;
    float allowedAngleDifference = 2f;

    protected override void StartFunctions()
    {
        base.StartFunctions();
    }

    protected override void UpdateFunctions()
    {
        base.UpdateFunctions();

        LerpRotation();
    }

    void LerpRotation()
    {
        float angleDifference = desiredRotationAngle - currentRotationAngle;

        bool rotationFinished = angleDifference < allowedAngleDifference;

        if (rotationFinished)
            return;

        float newRotationAngle = Mathf.LerpAngle
            (
                currentRotationAngle,
                desiredRotationAngle,
                Time.deltaTime * rotationSpeed
            );

        float rotationInThisFrame = newRotationAngle - currentRotationAngle;

        currentRotationAngle += rotationInThisFrame;

        transform.Rotate(Vector3.up, rotationInThisFrame);
    }

    public override void Interact()
    {
        AddGlobeRotation();
    }

    void AddGlobeRotation()
    {
        desiredRotationAngle += angleIncrement;
    }
}
