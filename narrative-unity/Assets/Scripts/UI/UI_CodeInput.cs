using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UI_CodeInput : MonoBehaviour
{
    public TextMeshProUGUI inputField;

    [Tooltip("How long the code is shown after the input has been completed.")]
    public float lastCharDelay = .4f;

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

    /*
     * Format the answer and submit it to the puzzle
     */
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

            EventManager.Global.TriggerEvent(GetReceiverID(), gameObject, playerInput.ToString());
        }

        if (codeType == Interactable_CodeInput.CodeType.text)
        {
            string formattedString = inputField.text.Trim();
            formattedString = formattedString.ToLower();

            int length = formattedString.Length;

            if (length < 8)
                return;

            EventManager.Global.TriggerEvent(GetReceiverID(), gameObject, formattedString);
        }

        Invoke("Hide", lastCharDelay);
    }

    string GetReceiverID()
    {
        return "Puzzle_CodeInput " + codeType.ToString();
    }

    void Show()
    {
        ToggleVisibility(true);
    }

    void Hide()
    {
        ToggleVisibility(false);
    }

    public void ToggleVisibility(bool setVisible = true)
    {
        isVisible = setVisible;

        // When the UI is shown, get focus and reset text
        if (isVisible)
            textFieldEvent.Invoke();

        // Hide the canvas by setting its scale to 0
        canvas.transform.localScale = isVisible ? localScale : Vector3.zero;
    }
}
