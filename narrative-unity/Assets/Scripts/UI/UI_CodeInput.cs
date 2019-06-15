using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UI_CodeInput : MonoBehaviour
{
    public string receiver = "Puzzle_CodeInput Chest";
    public TextMeshProUGUI inputField;

    public int playerInput;

    Canvas canvas;
    Vector3 localScale;
    bool isVisible = true;

    public Interactable_CodeInput.CodeType codeType;

    public UnityEvent textFieldEvent;

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = GameManager.GLOBAL.player.thirdPersonCamera;

        localScale = canvas.transform.localScale;
        ToggleVisibility(setVisible: false);
    }

    /*
     * Invoke execution of SubmitAnswer because TMPro doesn't process the input in the same frame
     */
    public void InvokeSubmitAnswer()
    {
        Invoke("SubmitAnswer", .02f);
    }

    public void SubmitAnswer()
    {
        if (codeType == Interactable_CodeInput.CodeType.digits)
        {
            string formattedString = "";

            int charIndex = 0;
            foreach (char letter in inputField.text.ToCharArray())
            {
                if (!char.IsNumber(letter))
                    continue;

                charIndex++;
                formattedString += letter;
            }

            if (!int.TryParse(formattedString, out playerInput))
                return;

            if (charIndex < 4)
                return;
        }

        EventManager.Global.TriggerEvent(receiver, gameObject, playerInput.ToString());

        ToggleVisibility(setVisible: false);
    }

    public void ToggleVisibility(bool setVisible = true)
    {
        isVisible = setVisible;

        if (isVisible)
        {
            textFieldEvent.Invoke();
        }

        inputField.text = isVisible ? "" : inputField.text;
        canvas.transform.localScale = isVisible ? localScale : Vector3.zero;
    }
}
