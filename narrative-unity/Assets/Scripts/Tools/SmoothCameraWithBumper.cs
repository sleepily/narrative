using UnityEngine;
using System.Collections;

public class SmoothCameraWithBumper : MonoBehaviour
{
    [Header("Properties")]

    [Range(0.02f, 4f)]
    [SerializeField]
    [Tooltip("Amount of camera lerp smoothing.")]
    private float damping = .04f;

    [SerializeField]
    [Tooltip("Move camera forward when bumping to prevent clipping.")]
    float clippingPreventionDistance = 1f;

    [Header("Debug")]

    [SerializeField]
    bool drawDebugRay = false;


    private Transform target;

    Vector3 initialPosition;

    Vector3 desiredPosition, targetBackDirection;
    float initialDistance, checkingDistance;

    RaycastHit hitInfo;
    Ray ray;

    private void Start()
    {
        GetAllComponents();
    }

    void GetAllComponents()
    {
        if (!target)
            target = transform.parent;

        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        SetVariables();
        CameraBumpCheck();
        LerpCamera();

        DrawDebugRay();
    }

    void SetVariables()
    {
        desiredPosition = target.TransformPoint(initialPosition);
        targetBackDirection = target.transform.forward * -1;

        hitInfo = new RaycastHit();
        ray = new Ray(target.transform.position, targetBackDirection);

        initialDistance = Mathf.Abs(initialPosition.z);
        checkingDistance = initialDistance + clippingPreventionDistance;
    }

    void DrawDebugRay()
    {
        if (!drawDebugRay)
            return;

        Debug.DrawRay(ray.origin, ray.direction * checkingDistance, Color.gray);

        if (hitInfo.point == Vector3.zero)
            return;

        Debug.DrawRay(ray.origin, hitInfo.point - ray.origin, Color.green);
    }

    void CameraBumpCheck()
    {
        if (Physics.Raycast(ray, out hitInfo, checkingDistance))
        {
            if (hitInfo.transform == target || hitInfo.transform == transform)
                return;

            CalculateNewCameraPosition();
        }
        else
            desiredPosition = target.transform.position + targetBackDirection * initialDistance;
    }

    void CalculateNewCameraPosition()
    {
        desiredPosition = hitInfo.point;

        desiredPosition += target.forward * clippingPreventionDistance;
    }

    void LerpCamera()
    {
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime / damping);
    }
}