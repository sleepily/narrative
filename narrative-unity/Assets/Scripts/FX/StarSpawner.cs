using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EX = Tools.ExtensionMethods;

public class StarSpawner : MonoBehaviour
{
    public GameObject starPrefab;

    public enum TriggerType
    {
        Manual,
        Start,
        AfterSceneFullyLoaded
    }

    public TriggerType triggerType = TriggerType.Manual;

    [Space]

    [Range(0, 1000)]
    public int amount = 350;

    [Space]

    [SerializeField]
    Color starGlow = Color.white;
    [SerializeField]
    float glowIntensity = 2.5f;

    [Space]

    [Header("MinMax")]
    public Vector2 earthDistance = new Vector2(300f, 1000f);
    public Vector2 starSize = new Vector2(30f, 80f);

    [Tooltip("How much stars should be scaled relative to their distance.")]
    public Vector2 distanceBias = new Vector2(.2f, 2f);

    [Range(0f, 1000f)]
    public float minStarToStarDistance = 30f;

    [Space]

    Vector3[] starPositions;
    GameObject[] stars;

    private void Start()
    {
        if (triggerType != TriggerType.Start)
            return;

        GenerateStars();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DeleteStars();
            GenerateStars();
        }
    }

    void DeleteStars()
    {
        for (int i = 0; i < stars.Length; i++)
            Destroy(stars[i]);
    }

    public void GenerateStars()
    {
        GenerateStarPositions();
        InstantiateStars();
    }

    void GenerateStarPositions()
    {
        starPositions = new Vector3[amount];

        for (int star = 0; star < starPositions.Length; star++)
        {
            starPositions[star] = RandomStarPosition();

            for (int previous = 0; previous < star; previous++)
            {
                float distance = Vector3.Distance(starPositions[star], starPositions[previous]);

                int retries = 0;

                while (distance < minStarToStarDistance)
                {
                    if (++retries > 5)
                        break;

                    starPositions[star] = RandomStarPosition();

                    distance = Vector3.Distance(starPositions[star], starPositions[previous]);
                }
            }
        }
    }

    Vector3 RandomStarPosition()
    {
        Vector3 northernPoint = Random.onUnitSphere * Random.Range(earthDistance.x, earthDistance.y);
        northernPoint.y = Mathf.Abs(northernPoint.y);
        return northernPoint;
    }

    void InstantiateStars()
    {
        stars = new GameObject[amount];

        for (int i = 0; i < starPositions.Length; i++)
        {
            stars[i] = Instantiate(starPrefab, transform);
            stars[i].transform.localPosition = starPositions[i];
            stars[i].transform.localScale = RandomStarScale(starPositions[i].magnitude);
            // stars[i].GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", starGlow * glowIntensity);
        }
    }

    Vector3 RandomStarScale(float distance)
    {
        float randomScale = Random.Range(starSize.x, starSize.y);
        float bias = EX.Map(distance, earthDistance.x, earthDistance.y, distanceBias.x, distanceBias.y);
        return Vector3.one * randomScale * bias;
    }
}
