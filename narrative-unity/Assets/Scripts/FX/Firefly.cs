using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ex = Tools.ExtensionMethods;

public class Firefly : MonoBehaviour
{
    public float size, speed, intensity;
    public bool hasJitter = true;

    bool canMove = false;

    Fireflies fireflies;
    Material material;

    private void Awake()
    {
        material = GetComponent<MeshRenderer>().material;
        Hide();
    }

    public Firefly SetFireflies(Fireflies fireflies)
    {
        this.fireflies = fireflies;
        transform.parent = fireflies.transform;
        transform.localPosition = Vector3.zero;
        return this;
    }

    public void Glow()
    {
        transform.localScale = Vector3.one * size;

        Color emissionColor = material.GetColor("_EmissionColor");
        emissionColor *= intensity;
        material.SetColor("_EmissionColor", emissionColor);

        canMove = true;
    }

    void Hide()
    {
        transform.localScale = Vector3.zero;
    }

    public void Pause()
    {
        canMove = false;

        transform.localScale = Vector3.zero;
    }

    private void Update() => Move();

    void Move()
    {
        if (!canMove)
            return;

        RotateByFlockingRules();
        Vector3 jitter = ApplyJitter(speed * intensity, speed * size);

        transform.Translate(jitter + Vector3.forward * Time.deltaTime * speed);
    }

    void RotateByFlockingRules()
    {
        Vector3 avoidanceVector = Vector3.zero;

        Vector3 centerPosition = transform.parent.position;

        foreach (Firefly firefly in fireflies.fireflies)
        {
            if (firefly == this)
                continue;

            // add fish vector to center position (if not following mouse)
            centerPosition += firefly.transform.position;

            bool fireflyIsNearby = Vector3.Distance(firefly.transform.position, this.transform.position) <= fireflies.avoidanceDistance;

            if (fireflyIsNearby)
                avoidanceVector += this.transform.position - firefly.transform.position;
        }

        if (fireflies.fireflies.Length != 0)
            centerPosition /= fireflies.fireflies.Length;

        Vector3 newDirection = (centerPosition + avoidanceVector) - this.transform.position;

        transform.rotation =
            Quaternion.Slerp
            (
                this.transform.rotation,
                Quaternion.LookRotation(newDirection),
                Time.deltaTime / (fireflies.rotationDamping * size)
            );
    }

    Vector3 ApplyJitter(float multiplierX, float multiplierY)
    {
        if (!hasJitter)
            return Vector3.zero;

        float noiseX = Mathf.PerlinNoise(Time.time * multiplierX, Time.time * multiplierY);
        float noiseY = Mathf.PerlinNoise(Time.time * multiplierY, Time.time * multiplierX);

        float noiseZ = Random.Range(noiseX, noiseY);

        float jitterScale = Ex.Map(speed, fireflies.speed.x, fireflies.speed.y, 0, fireflies.jitterStrength);

        Vector3 jitter = new Vector3(noiseX, noiseY, noiseZ).normalized;
        jitter -= Vector3.one * .5f;
        jitter *= jitterScale;

        return jitter * Time.deltaTime;
    }
}
