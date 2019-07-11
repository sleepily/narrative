using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ex = Tools.ExtensionMethods;

public class Fireflies : MonoBehaviour
{
    public Firefly firefly;

    public bool glowFromStart = false;

    [Header("Spawn and Movement")]
    [Range(1, 50)]
    public int amount = 30;
    [Range(.1f, 5f)]
    public float spawnRadius = 2f;
    [Range(.1f, 1f)]
    public float avoidanceDistance = .4f;
    [Range(4, 200)]
    public float rotationDamping = 120f;
    public bool applyJitter = true;
    [Range(0f, 2f)]
    public float jitterStrength = .2f;

    [Header("Firefly Properties")]
    [Tooltip("Minimum and maximum firefly speed.")]
    public Vector2 size = new Vector2(0.02f, .08f);
    public Vector2 speed = new Vector2(0.15f, 1.5f);
    public Vector2 intensity = new Vector2(0.01f, 0.14f);

    [Space]

    [Tooltip("Scale the speed with size.")]
    public bool speedRelativeToSize = true;

    [Tooltip("Scale the light intensity with speed.")]
    public bool intensityRelativeToSpeed = true;

    [HideInInspector]
    public Firefly[] fireflies { get; private set; }

    private void Start()
    {
        SetupFireflies();

        if (glowFromStart)
            StartGlowing();
    }

    void SetupFireflies()
    {
        if (amount < 1)
            amount = 1;

        fireflies = new Firefly[amount];

        for (int i = 0; i < fireflies.Length; i++)
        {
            fireflies[i] = Instantiate(firefly).SetFireflies(this);
            fireflies[i].transform.localPosition += Random.insideUnitSphere * spawnRadius;

            fireflies[i].size = Random.Range(size.x, size.y);

            fireflies[i].speed = speedRelativeToSize
                ? Ex.Map(fireflies[i].size, size.x, size.y, speed.y, speed.x)
                : Random.Range(speed.x, speed.y);

            fireflies[i].intensity = intensityRelativeToSpeed
                ? Ex.Map(fireflies[i].speed, speed.x, speed.y, intensity.x, intensity.y)
                : Random.Range(intensity.x, intensity.y);
        }
    }

    public void StartGlowing()
    {
        if (fireflies == null || fireflies.Length == 0)
            SetupFireflies();

        for (int i = 0; i < fireflies.Length; i++)
            fireflies[i].Glow();
    }

    public void StopGlowing()
    {
        if (fireflies == null || fireflies.Length == 0)
            SetupFireflies();

        for (int i = 0; i < fireflies.Length; i++)
            fireflies[i].Pause();
    }
}
