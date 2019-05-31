using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public float charactersPerSecond = 10f;

    public SpeechBubble speechBubblePrefab;
    SpeechBubble speechBubble;
    bool textFinished = true;

    Queue<string> phrases;

    private void Start()
    {
        phrases = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue, Interactable interactable)
    {
        if (speechBubble)
            return;

        phrases.Clear();

        foreach (string sentence in dialogue.phrases)
        {
            phrases.Enqueue(sentence);
        }

        speechBubble = Instantiate(speechBubblePrefab, interactable.transform);

        speechBubble.dialogueName.text = dialogue.name;

        DisplayNextPhrase();
    }

    public void DisplayNextPhrase()
    {
        if (!speechBubble)
            return;

        if (!textFinished)
            return;

        if (phrases.Count == 0)
        {
            EndDialogue();
            return;
        }

        string phrase = phrases.Dequeue();
        StartCoroutine(TextAnimation(phrase));
    }

    void EndDialogue()
    {
        Destroy(speechBubble.gameObject);
    }

    IEnumerator TextAnimation(string sentence)
    {
        bool skip = false;
        bool textActive = false; // avoid skipping message due to key check in same frame
        speechBubble.dialoguePhrase.text = "";
        textFinished = false;

        foreach (char letter in sentence.ToCharArray())
        {
            speechBubble.dialoguePhrase.text += letter;

            if (Input.GetKeyDown(KeyCode.Space) && textActive)
            {
                skip = true;
                textFinished = true;
            }

            textActive = true;

            if (!skip)
                yield return null; // new WaitForSeconds(1f / charactersPerSecond);
        }

        textFinished = true;

        yield return null;
    }
}
