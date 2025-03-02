﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using CodeType = Interactable_CodeInput.CodeType;

public class UI_CodeInput : MonoBehaviour
{
    public TextMeshProUGUI inputField;

    [Tooltip("Date = 8, Text = 4/8 (help/iloveyou), Digits: 4")]
    public int inputLength = 4;

    [Tooltip("How long the code is shown after the input has been completed.")]
    float lastCharDelay = 0f;

    [Range(.01f, 1f)]
    public float popupLerpTime = .1f;

    Canvas canvas;
    Vector3 localScale;
    bool isVisible = true;

    public Puzzle_CodeInput puzzle;

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
    public void InvokeSubmitAnswer() => Invoke("SubmitAnswer", .02f);

    /*
     * Format the answer and submit it to the puzzle
     */
    public void SubmitAnswer()
    {
        string answer = "";

        if (puzzle.codeType == CodeType.digits
            || puzzle.codeType == CodeType.DeskDrawer
            || puzzle.codeType == CodeType.RadioFrequency)
        {
            int charIndex = 0;
            foreach (char letter in inputField.text.ToCharArray())
            {
                if (!char.IsNumber(letter))
                    continue;

                charIndex++;
                answer += letter;
            }

            if (charIndex < inputLength)
                return;
        }

        if (puzzle.codeType == CodeType.MILK
            || puzzle.codeType == CodeType.TAYLOR
            || puzzle.codeType == CodeType.HEART)
        {
            // Only include letters, not spacings or symbols
            foreach (char letter in inputField.text.ToCharArray())
                if (char.IsLetter(letter))
                    answer += char.ToUpper(letter);

            answer.ToUpper();

            int length = answer.Length;

            if (length < inputLength)
                return;
        }

        GameManager.GLOBAL.dialogue.SetCodeInputInProgress(false);
        puzzle.PuzzleCheck(answer);
        Invoke("Hide", lastCharDelay);
    }

    void Show() => ToggleVisibility(true);

    void Hide() => ToggleVisibility(false);

    public void ToggleVisibility(bool setVisible = true)
    {
        isVisible = setVisible;

        if (isVisible)
            GameManager.GLOBAL.player.SetMovementLock(isVisible);

        GameManager.GLOBAL.dialogue.SetCodeInputInProgress(isVisible);

        // When the UI is shown, get focus and reset text
        if (isVisible)
            textFieldEvent.Invoke();

        // Animate the canvas popping up/hiding
        transform.localScale = isVisible ? localScale : Vector3.zero;
    }
}
