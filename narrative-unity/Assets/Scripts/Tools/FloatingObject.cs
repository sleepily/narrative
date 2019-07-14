using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    public float amp = 40f;
    public float frequency = .2f;

    Vector3 pos;

    private void Start()
    {
        pos = transform.position;
    }

    private void Update()
    {
        pos.y += Mathf.Sin((Time.time / Mathf.PI) * frequency) * amp;
        transform.position = pos;
    }
}
