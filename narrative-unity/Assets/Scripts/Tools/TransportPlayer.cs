using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportPlayer : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        CheckCollision(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        CheckCollision(collision);
    }

    void CheckCollision(Collision collision)
    {
        Player player = collision.collider.GetComponent<Player>();

        if (!player)
            return;


    }
}
