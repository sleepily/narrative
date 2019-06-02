using UnityEngine;
using System.Collections;

public class SmoothCameraWithBumper : MonoBehaviour
{
    private Transform target;

    Vector3 initialPosition;

    [SerializeField]
    bool drawDebugRay = false;

    [Range(0.02f, 4f)]
    [SerializeField]
    private float damping = .04f;

    [SerializeField]
    float clippingPreventionDistance = 1f;

    private void Start()
    {
        GetAllComponents();
    }

    void GetAllComponents()
    {
        if (!target)
            target = transform.parent;

        initialPosition = transform.position;
    }

    private void Update()
    {
        Vector3 desiredPosition = target.TransformPoint(initialPosition);
        
        Vector3 targetBackDirection = target.transform.forward * -1;

        RaycastHit hitInfo;
        Ray ray = new Ray(target.transform.position, targetBackDirection);

        float initialDistance = Mathf.Abs(initialPosition.z);

        float checkingDistance = initialDistance + clippingPreventionDistance;

        if (drawDebugRay)
            Debug.DrawRay(ray.origin, ray.direction * checkingDistance, Color.gray);

        if (Physics.Raycast(ray, out hitInfo, checkingDistance))
        {
            if (hitInfo.transform == target || hitInfo.transform == transform)
                return;

            desiredPosition = hitInfo.point;

            desiredPosition += target.forward * clippingPreventionDistance;
            
            if (drawDebugRay)
                Debug.DrawRay(ray.origin, hitInfo.point - ray.origin, Color.green);
        }
        else
            desiredPosition = target.transform.position + targetBackDirection * initialDistance;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime / damping);
    }
}