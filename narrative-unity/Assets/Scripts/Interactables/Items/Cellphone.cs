using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[RequireComponent(typeof(Flowchart))]
[RequireComponent(typeof(Item))]
[RequireComponent(typeof(Animator))]
public class Cellphone : MonoBehaviour
{
    Flowchart flowchart;
    Animator animator;
    Item cellphoneItem;
    int messageIntensity = 0;

    [Header("Message sending")]
    public bool sendMessagesAutomatically = true;

    [Range(2f, 24f)]
    public float minimumTimeBetweenTexts = 4f;

    float lastMessageReadTime = 0f;
    bool canTriggerNewMessage = true;

    [Tooltip("Set text message interval in seconds. If 0: texts can only be triggered from outside.")]
    public List<float> messageIntervals = new List<float>();

    private void Start()
    {
        flowchart = GetComponent<Flowchart>();
        cellphoneItem = GetComponent<Item>();
        animator = GetComponent<Animator>();

        lastMessageReadTime = Time.time;
    }

    private void Update() => SendNewIntervalMessage();

    public void AddMessageIntensity() => messageIntensity++;

    /*
     * Control when to send an automatic text to the player
     */
    bool SendNewIntervalMessage()
    {
        if (Input.GetKeyDown(KeyCode.G))
            TriggerNewMessage();

        // Exit if player is still in a message
        if (!canTriggerNewMessage)
            return false;

        // Don't send texts if the phone hasn't been picked up yet
        if (!cellphoneItem.isInInventory)
            return false;

        // Return if list cannot provide interval
        if (messageIntensity >= messageIntervals.Count)
            return false;

        float nextMessageInterval = messageIntervals[messageIntensity];

        // Only enable manual triggers or prevent spam
        if (nextMessageInterval <= minimumTimeBetweenTexts)
            return false;

        // Return if too early
        if (Time.time < lastMessageReadTime + nextMessageInterval)
        {
            float timeUntilNextMessage = Time.time - (lastMessageReadTime + nextMessageInterval);
            timeUntilNextMessage = Mathf.Abs(timeUntilNextMessage);

            return false;
        }

        TriggerNewMessage();
        return true;
    }

    /*
     * If possible, trigger a new cell phone message
     */
    public void TriggerNewMessage()
    {
        flowchart.SetIntegerVariable("messageIntensity", messageIntensity);
        AnimateScreen(true);

        if (!flowchart.ExecuteIfHasBlock("NewMessage"))
        {
            Debug.Log("Couldn't trigger cellphone message. Block \"NewMessage\" missing?");
            return;
        }

        Debug.Log("Triggered cellphone with intensity level " + messageIntensity);
        WaitForPlayerRead();
    }

    /*
     * Disable new texts until the player has finished the dialogue
     */
    void WaitForPlayerRead()
    {
        canTriggerNewMessage = false;

        // Prevent any player movement or interaction
        GameManager.GLOBAL.player.Lock();

        StartCoroutine(CheckPlayerRead());
    }

    IEnumerator CheckPlayerRead()
    {
        // Wait for the player to end all dialogue
        while (flowchart.HasExecutingBlocks())
            yield return null;

        // Enable new texts from this point on
        lastMessageReadTime = Time.time;
        canTriggerNewMessage = true;

        GameManager.GLOBAL.player.Unlock();

        AnimateScreen(false);
    }

    void AnimateScreen(bool screenOn = true)
    {
        animator.SetBool("hasScreenOn", screenOn);
    }
}
