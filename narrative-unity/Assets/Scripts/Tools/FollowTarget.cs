using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target;

    Vector3 offset;
    public Vector3 lastPosition { get; private set; }
    public Vector3 desiredPosition { get; private set; }

    [Range(.01f, .2f)]
    public float allowedError = .1f;

    [Range(.01f, 1f)]
    public float dampingSpeed = .04f;

    [Range(.01f, 2f)]
    public float followInterval = .5f;

    float lastInterval;

    private void Start()
    {
        offset = transform.localPosition;
        lastPosition = desiredPosition = transform.position;
    }

    private void Update() => FetchNewPosition();

    private void LateUpdate() => LerpToNextPosition();

    void FetchNewPosition()
    {
        if (Time.time < lastInterval + followInterval)
            return;

        lastInterval += followInterval;

        desiredPosition = target.position;
        /*
            + offset.x * target.right
            + offset.y * target.up
            + offset.z * target.forward;
        */
    }

    void LerpToNextPosition()
    {
        transform.position = lastPosition;

        /*
        float distanceToNextPosition = Vector3.Distance(position, desiredPosition);

        if (distanceToNextPosition < allowedError)
            return;
            */

        // Skip lerping if levels are being changed
        if (!GameManager.GLOBAL.sceneLoader.hasFinishedLoading)
        {
            lastPosition = desiredPosition + offset;
            transform.position = lastPosition;
            return;
        }

        lastPosition = Vector3.Lerp
        (
            transform.position,
            desiredPosition + offset,
            Time.deltaTime / dampingSpeed
        );

        transform.position = lastPosition;
    }
}