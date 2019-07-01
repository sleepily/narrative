using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeRigidbodiesKinematic : MonoBehaviour
{
    public List<Rigidbody> rbs = new List<Rigidbody>();

    public void SetKinematicState(bool state)
    {
        foreach (Rigidbody rigidbody in rbs)
            rigidbody.isKinematic = state;
    }
}
