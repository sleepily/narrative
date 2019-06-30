using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidPuzzle : MonoBehaviour
{
    public bool hideOnStart = true;
    Vector3 startScale;

    private void Start()
    {
        startScale = transform.localScale;

        if (hideOnStart)
            Hide();
    }

    public void Hide() => SetScale(Vector3.zero);

    public void Show() => SetScale(startScale);

    void SetScale(Vector3 scale) => transform.localScale = scale;
}
