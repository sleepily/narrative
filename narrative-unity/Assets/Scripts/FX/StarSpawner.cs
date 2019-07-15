using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EX = Tools.ExtensionMethods;

public class StarSpawner : MonoBehaviour
{
    [Header("Spawning")]
    public GameObject starPrefab;

    public enum TriggerType
    {
        Manual,
        Start,
        StartVisible
    }

    public TriggerType triggerType = TriggerType.Manual;

    [Space]

    [Range(0, 1000)]
    public int amount = 350;

    [Space]

    [Header("Colors")]
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

    [Header("Blending")]
    [SerializeField]
    bool allowBlend = true;
    [SerializeField]
    int blendSteps = 5;
    [SerializeField]
    float lerpFactor = 2f;

    float blend = 0f;
    int currentBlendStep = 0;
    float targetBlend = 0f;
    float blendInterval = .7f;

    Star[] stars;

    public struct Star
    {
        public GameObject go;
        public Vector3 position;
        public Vector3 scale;
        public Vector3 currentScale;

        public Star(GameObject go, Vector3 position, Vector3 scale)
        {
            this.go = go;
            this.position = position;
            this.scale = scale;
            this.currentScale = Vector3.zero;

            this.go.transform.localPosition = this.position;

            SetVisibility(false);
        }

        public void SetVisibility(bool visible)
        {
            this.currentScale = visible ? this.scale : Vector3.zero;
            go.transform.localScale = currentScale;
        }
    }

    private void Start()
    {
        if (triggerType != TriggerType.Manual)
            GenerateStars();

        if (triggerType == TriggerType.StartVisible)
            MakeVisible();
        else
            StartCoroutine(Coroutine_BlendTimer());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DeleteStars();
            GenerateStars();
        }

        LerpToBlend();
    }

    void DeleteStars()
    {
        for (int i = 0; i < stars.Length; i++)
            Destroy(stars[i].go);
    }

    public void MakeVisible()
    {
        allowBlend = false;

        for (int i = 0; i < stars.Length; i++)
            stars[i].SetVisibility(true);
    }

    public void BlendInStars(float blend)
    {
        if (!allowBlend)
            return;

        // Debug.Log($"Blending to {blend}...");

        blend = Mathf.Clamp01(blend);
        float easedBlend = LeanTween.easeInCirc(0f, 1f, blend);

        int easedMax = (int)Mathf.Floor(EX.Map(easedBlend, 0, 1, 0, stars.Length));
        int max = (int)Mathf.Floor(EX.Map(blend, 0, 1, 0, stars.Length));

        if (easedMax <= 0)
            return;

        for (int i = 0; i < stars.Length; i++)
            stars[i].SetVisibility((i < easedMax) ? true : false);

        // Calculate how many stars are drawn compared to how many could be
        // Essentially the value of the tween at this moment
        int effectiveness = (int)((easedBlend / blend) * 100) + 1;
        Debug.Log($"Making {easedMax}:{max} stars visible ({effectiveness}%).");
    }

    void GenerateStars()
    {
        stars = new Star[amount];

        for (int star = 0; star < stars.Length; star++)
        {
            Vector3 position = RandomStarPosition();

            for (int previous = 0; previous < star; previous++)
            {
                float distance = Vector3.Distance(position, stars[previous].position);

                int retries = 0;

                while (distance < minStarToStarDistance)
                {
                    if (++retries > 5)
                        break;

                    stars[star].position = RandomStarPosition();

                    distance = Vector3.Distance(position, stars[previous].position);
                }
            }

            GameObject spawned = Instantiate(starPrefab, transform);
            Vector3 scale = RandomStarScale(position.magnitude);

            stars[star] = new Star(spawned, position, scale);
        }
    }

    Vector3 RandomStarPosition()
    {
        Vector3 northernPoint = Random.onUnitSphere * Random.Range(earthDistance.x, earthDistance.y);
        northernPoint.y = Mathf.Abs(northernPoint.y);
        return northernPoint;
    }

    Vector3 RandomStarScale(float distance)
    {
        float randomScale = Random.Range(starSize.x, starSize.y);
        float bias = EX.Map(distance, earthDistance.x, earthDistance.y, distanceBias.x, distanceBias.y);
        
        return Vector3.one * randomScale * bias;
    }

    public void IncreaseBlendStep()
    {
        if (++currentBlendStep > blendSteps)
            currentBlendStep = blendSteps;

        targetBlend = EX.Map01(currentBlendStep, 0, blendSteps);
        Debug.Log($"Increased target blend to {targetBlend} with {currentBlendStep}:{blendSteps}.");
    }

    IEnumerator Coroutine_BlendTimer()
    {
        allowBlend = true;

        float lastTime = Time.time;

        while (gameObject.activeSelf)
        {
            if (Time.time <= lastTime + blendInterval)
                yield return null;

            lastTime += blendInterval;

            BlendInStars(blend);
        }
    }

    void LerpToBlend()
    {
        if (blend == targetBlend)
            return;

        blend = Mathf.Lerp
        (
            blend,
            targetBlend,
            Time.deltaTime / lerpFactor
        );
    }
}
