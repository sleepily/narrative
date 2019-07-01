using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnaroundShowcase : MonoBehaviour
{
    public Animator animator;

    float oscillationInterval = 6f;
    bool isWalking = true;

    float startTime, lastTime;

    private void Start()
    {
        startTime = Time.time;
        lastTime = startTime;
    }

    private void Update()
    {
        if (Time.time >= lastTime + oscillationInterval)
        {
            isWalking = !isWalking;
            lastTime += oscillationInterval;
        }

        float walkingSpeed = isWalking ? 1f : 0f;
        animator.SetFloat("walkingSpeed", walkingSpeed);
    }
}
