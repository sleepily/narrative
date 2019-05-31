using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public SpeechBubble speechBubblePrefab;
    SpeechBubble speechBubble;

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

        if (phrases.Count == 0)
        {
            EndDialogue();
            return;
        }

        string phrase = phrases.Dequeue();
        speechBubble.dialoguePhrase.text = phrase;
    }

    void EndDialogue()
    {
        Destroy(speechBubble.gameObject);
    }
}
