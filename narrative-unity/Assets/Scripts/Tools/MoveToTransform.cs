using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTransform : MonoBehaviour
{
    public Transform newParent;

    public void Move()
    {
        transform.parent = newParent;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }
}
