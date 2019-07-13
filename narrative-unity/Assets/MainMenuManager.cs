using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainMenuManager : MonoBehaviour
{
    bool showTitle = true;

    public UnityEvent hideTitle;

    private void Update()
    {
        bool spaceEnter = Input.GetAxisRaw("Submit") > float.Epsilon;
        bool mouse = Input.GetMouseButtonDown(0);

        if (spaceEnter || mouse)
            HideTitle();
    }

    void HideTitle()
    {
        if (!showTitle)
            return;

        showTitle = false;

        hideTitle.Invoke();
    }
}
